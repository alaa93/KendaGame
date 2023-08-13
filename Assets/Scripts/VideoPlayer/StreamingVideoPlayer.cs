using KendaGame.Helpers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace KendaGame.UI {
  public class StreamingVideoPlayer : MonoBehaviour {

    [SerializeField]
    private AudioSource backGroundMusic;
    private string videoName;
    [SerializeField]
    ScrollRect map;

    void Start() {
      int lastLetterCompletted = int.Parse(StorageHelper.ReadStorage(StorageHelper.LastLetterCompleted, "0"));
      if (lastLetterCompletted == 0 && StorageHelper.ReadStorage(StorageHelper.CanWatchIntro, "true") == "true") {
        videoName = IslandVideos.FirstLandIntro;
        StartVideo(videoName);
      } else if (lastLetterCompletted == 8 && StorageHelper.ReadStorage(StorageHelper.CanWatchOutro, "true") == "true") {
        videoName = IslandVideos.FirstLandOutro;
        StartVideo(videoName);
      }
    }

    void StartVideo(string videoName) {
      backGroundMusic.Pause();
      map.enabled=false;
      GameObject camera = GameObject.Find("Main Camera");
      var videoPlayer = camera.AddComponent<VideoPlayer>();
      videoPlayer.playOnAwake = false;
      videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
      videoPlayer.url = videoName;
      videoPlayer.isLooping = true;
      videoPlayer.loopPointReached += EndReached;
      videoPlayer.Play();
    }

    void EndReached(VideoPlayer vp) {
      if (videoName == IslandVideos.FirstLandIntro) {
        StorageHelper.WriteStorage(StorageHelper.CanWatchIntro, "false");
      } else {
        StorageHelper.WriteStorage(StorageHelper.CanWatchOutro, "false");
      }
      Destroy(vp);
      backGroundMusic.Play();
      map.enabled=true;
    }
  }
}
