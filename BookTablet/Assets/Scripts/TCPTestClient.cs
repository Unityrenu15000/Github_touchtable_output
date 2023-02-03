// This work is licensed under the Creative Commons Attribution-ShareAlike 4.0 International License. 
// To view a copy of this license, visit http://creativecommons.org/licenses/by-sa/4.0/ 
// or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;

public class TCPTestClient : MonoBehaviour
{
    public static TCPTestClient instance;

    public TMP_Text debugText;
    public string message;

    public ConnectionInfo setting;

    #region private members 	
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    #endregion
    // Use this for initialization 	

    #region Read From txt

    public string fileName;

    public void ReadFromJson()
    {
        string json = ReadString(Path.Combine(Application.streamingAssetsPath, fileName));
        setting = JsonUtility.FromJson<ConnectionInfo>(json);
    }

    static string ReadString(string path)
    {
        string str;
        //Read the text directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        str = reader.ReadToEnd();
        reader.Close();
        return str;
    }

    #endregion

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ReadFromJson();
        ConnectToTcpServer();
    }
    // Update is called once per frame
    void Update()
    {       
        debugText.text = message;
        message = "";
    }
    /// <summary> 	
    /// Setup socket connection. 	
    /// </summary> 	
    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }
    /// <summary> 	
    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    /// </summary>     
    private void ListenForData()
    {
        try
        {
            Debug.Log("Connecting to : " + setting.ip + " " + setting.port);
            socketConnection = new TcpClient(setting.ip, setting.port);
            Debug.Log("Connected to : " + setting.ip + " " + setting.port);
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 						
                        string serverMessage = Encoding.ASCII.GetString(incommingData);
                        Debug.Log("server message received as: " + serverMessage);
                        message = "server message received as: " + serverMessage;
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
    /// <summary> 	
    /// Send message to server using socket connection. 	
    /// </summary> 	
    public void SendMyMessage(string myMsg)
    {
        print("sending message " + myMsg);
        if (socketConnection == null)
        {
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                string clientMessage = myMsg;
                // Convert string message to byte array.                 
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                // Write byte array to socketConnection stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                Debug.Log("Client sent his message - should be received by server");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void DestroyPort()
    {
        //SendStopRunningSignal();
        if (clientReceiveThread != null)
        {
            clientReceiveThread.Abort();
        }
    }

    private void OnDestroy()
    {
        DestroyPort();
    }
}

[System.Serializable]
public class ConnectionInfo
{
    public string ip;
    public int port;
}