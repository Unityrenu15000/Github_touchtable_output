using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class AppManager : MonoBehaviour
{
    public GameObject bgVideo, mainMenu, videoMenu, bookMenu;

    public VideoPlayer bgVideoPlayer, buttonVideoPlayer;

    public string bgVideoPath, buttonVideoPath;

    private void Awake()
    {
        ShowMainMenu();

        bgVideoPlayer.url = Path.Combine(Application.streamingAssetsPath, bgVideoPath);
        buttonVideoPlayer.url = Path.Combine(Application.streamingAssetsPath, buttonVideoPath);

    }

    public void OnButtonPressed(string bName)
    {
        switch(bName)
        {
            case "1":
                {
                    TCPTestClient.instance.SendMyMessage("1");
                    ShowBook();
                    break;
                }
            case "2":
                {
                    TCPTestClient.instance.SendMyMessage("2");
                    ShowBook();
                    break;
                }
            case "3":
                {
                    TCPTestClient.instance.SendMyMessage("3");
                    ShowBook();
                    break;
                }
            case "4":
                {
                    TCPTestClient.instance.SendMyMessage("4");
                    ShowBook();
                    break;
                }
            case "5":
                {
                    TCPTestClient.instance.SendMyMessage("5");
                    PlayVideo();
                    break;
                }
            case "Pause":
                {
                    TCPTestClient.instance.SendMyMessage("Pause");
                    break;
                }
            case "Play":
                {
                    TCPTestClient.instance.SendMyMessage("Play");
                    break;
                }

            case "Prvs":
                {
                    TCPTestClient.instance.SendMyMessage("Prvs");
                    break;
                }
            case "Next":
                {
                    TCPTestClient.instance.SendMyMessage("Next");
                    break;
                }
            case "Back":
                {
                    TCPTestClient.instance.SendMyMessage("Back");
                    ShowMainMenu();
                    break;
                }
        }    
    }

    public void PlayVideo()
    {
        bgVideo.transform.SetAsLastSibling();
        videoMenu.transform.SetAsLastSibling();
        videoMenu.SetActive(true);
        bookMenu.SetActive(false);
    }

    public void ShowBook()
    {
        bgVideo.transform.SetAsLastSibling();
        videoMenu.SetActive(false);
        bookMenu.SetActive(true);
        bookMenu.transform.SetAsLastSibling();
    }

    public void ShowMainMenu()
    {
        bgVideo.transform.SetAsLastSibling();
        mainMenu.transform.SetAsLastSibling();
        videoMenu.SetActive(false);
        bookMenu.SetActive(false);
    }
}
