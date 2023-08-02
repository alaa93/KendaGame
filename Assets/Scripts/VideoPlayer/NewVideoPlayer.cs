using KendaGame.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class NewVideoPlayer : MonoBehaviour
{

    [SerializeField]
    private AudioSource backGroundMusic;
    private string videoName;
    void Start()
    {
        int lastLetterCompletted = int.Parse(StorageHelper.ReadStorage(StorageHelper.LastLetterCompleted, "0"));

        if (lastLetterCompletted == 0 && StorageHelper.ReadStorage(StorageHelper.CanWatchIntro, "true") == "true")
        {
            videoName = IslandVideos.firstLandIntro;
            StartVideo(videoName);
            backGroundMusic.Pause();
        }
        else if (lastLetterCompletted == 8 && StorageHelper.ReadStorage(StorageHelper.CanWatchOutro, "true") == "true")
        {
            videoName=IslandVideos.firstLandIntro;
            StartVideo(videoName);
            backGroundMusic.Pause();
        }
    }

    void StartVideo(string videoName)
    {
        GameObject camera = GameObject.Find("Main Camera");
        var videoPlayer = camera.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        videoPlayer.url = videoName;
        videoPlayer.isLooping = true;
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.Play();
    }

    void EndReached(VideoPlayer vp)
    {
        if (videoName == IslandVideos.firstLandIntro)
        {

            StorageHelper.WriteStorage(StorageHelper.CanWatchIntro, "false");
        }
        else
        {
            StorageHelper.WriteStorage(StorageHelper.CanWatchOutro, "false");

        }
        Debug.Log("The video Is Over");
        Destroy(vp);
        backGroundMusic.Play();
    }
}

