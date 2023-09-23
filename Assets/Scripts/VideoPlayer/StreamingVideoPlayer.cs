using KendaGame.Helpers;
using KendaGame.Navigation;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace KendaGame.UI {
  public class StreamingVideoPlayer : MonoBehaviour {
    [SerializeField] private SplashScreenController splashScreenController;

    private string videoName;
    private LoadingBar loadingBar;
    public static StreamingVideoPlayer instance;

    private void Awake() {
      HTTPRequestHandler.Instance.GetProfileInformationInterface();
      if (instance == null) {
        instance = this;
      } else {
        Destroy(instance);
      }
    }

    public void StartVideo(string videoName, float timeToShowVideo) {
      this.videoName = videoName;
      GameObject camera = GameObject.Find("Main Camera");
      var videoPlayer = camera.AddComponent<VideoPlayer>();
      videoPlayer.playOnAwake = false;
      videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
      videoPlayer.url = videoName;
      videoPlayer.isLooping = true;
      videoPlayer.loopPointReached += EndReached;
      videoPlayer.Prepare();
      StartCoroutine(PlayVideoAfterPrepare(videoPlayer, timeToShowVideo));
    }

    private IEnumerator PlayVideoAfterPrepare(VideoPlayer videoPlayer, float timeToShowVideo) {
      yield return new WaitForSeconds(timeToShowVideo);
      if (!videoPlayer.isPrepared) {
        loadingBar = PopUpsManager.Instance.LoadLoadingBar();
        loadingBar.ShowLoadingBar();

        while (!videoPlayer.isPrepared) {
          loadingBar.IncreaseLoadingBar(0.1f);
          yield return new WaitForSeconds(1f);
        }

        loadingBar.HideLoadingBar();
      }
      videoPlayer.Play();
    }

    private void EndReached(VideoPlayer vp) {
      if (videoName == IslandVideos.FirstLandIntro) {
        StorageHelper.WriteStorage(StorageHelper.CanWatchIntro, "false");
      } else {
        StorageHelper.WriteStorage(StorageHelper.CanWatchOutro, "false");
      }
      Destroy(vp);
      if (SceneManager.Instance) {
        splashScreenController.CheckUserLoginSession();
      }
    }
  }
}
