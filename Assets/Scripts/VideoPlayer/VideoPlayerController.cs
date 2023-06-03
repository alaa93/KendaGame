using KendaGame.Helpers;
using KendaGame.Navigation;
using UnityEngine;
using UnityEngine.Video;

namespace KendaGame.UI {
  public class VideoPlayerController : MonoBehaviour {
    [SerializeField]
    GameObject VideoRenderer;
    [SerializeField]
    VideoPlayer videoPlayer;

    private void Start() {
      InitVideoPlayer();
    }

    private void InitVideoPlayer() {
      PlayerPrefs.SetInt("PlayIntro1", 1);
      videoPlayer.Prepare();
      videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source) {
      SceneManager.Instance.SetBackBtnState(false);
      SceneManager.Instance.SetCoinsObjectState(false);
      SceneManager.Instance.OpenCloudsManually(animated: true);
      videoPlayer.Play();
      videoPlayer.loopPointReached += VideoCompletedPlaying;
    }

    private void VideoCompletedPlaying(VideoPlayer source) {
      SceneManager.Instance.CloseCloudsManually(animated: true, onComplete: (state) => {
        videoPlayer.Stop();
        SceneManager.Instance.OnVideoCompletedMoveTo();
      });
    }

  }
}
