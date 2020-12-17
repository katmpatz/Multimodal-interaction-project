using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showScene : MonoBehaviour
{
    public Button changeButton;
    public GameObject windowPlane;

    // Start is called before the first frame update
    void Start()
    {
        changeButton.onClick.AddListener(() => ChangeView("season", "spring"));
    }

    void ChangeView(string actionType, string parameter)
    {
        var videoPlayer = windowPlane.GetComponent<UnityEngine.Video.VideoPlayer>();

        switch (actionType)
        {
            case "season":
                switch (parameter)
                {
                case "summmer":
                    videoPlayer.url = "Assets/Media/video-sky.mp4";
                    break;
                case "winter":
                    videoPlayer.url = "Assets/Media/video-sky.mp4";
                    break;
                case "spring":
                    videoPlayer.url = "Assets/Media/video-forest.mp4";
                    break;
                case "autumn":
                    videoPlayer.url = "Assets/Media/video-sky.mp4";
                    break;
                }
                break;
            case "weather":
                switch (parameter)
                {
                case "snowing":
                    videoPlayer.url = "Assets/Media/video-sky.mp4";
                    break;
                }
                break;  
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
