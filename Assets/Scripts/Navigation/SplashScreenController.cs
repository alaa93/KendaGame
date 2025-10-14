using UnityEngine;
using KendaGame.Navigation;
using System.Threading.Tasks;
using Spine.Unity;
using KendaGame.Helpers;

namespace KendaGame.UI {
  public class SplashScreenController : MonoBehaviour {

    [SerializeField]
    [Range(2, 15)]
    int logoDuration = 8;
    [SerializeField]
    SkeletonAnimation logoSpineAnim;

    private void Start() {
      SceneManager.Instance.OpenCloudsManually(onComplete: (animated) => {
        if (animated) {
          LoadNextScreen();
        }
      });
    }

    private async void LoadNextScreen() {
      logoSpineAnim.AnimationState.SetAnimation(0, "animation", false);
      int lastLetterCompletted = int.Parse(StorageHelper.ReadStorage(StorageHelper.LastLetterCompleted, "0"));
      if (lastLetterCompletted == 0 && StorageHelper.ReadStorage(StorageHelper.CanWatchIntro, "true") == "true") {
        string videoName = IslandVideos.FirstLandIntro;
        StreamingVideoPlayer.instance.StartVideo(videoName, logoDuration);
      } else if (lastLetterCompletted == 8 && StorageHelper.ReadStorage(StorageHelper.CanWatchOutro, "true") == "true") {
        string videoName = IslandVideos.FirstLandOutro;
        StreamingVideoPlayer.instance.StartVideo(videoName, logoDuration);
      } else {
        await Task.Delay(logoDuration * 1000);
        if (SceneManager.Instance) {
          CheckUserLoginSession();
        }
      }
    }

      public void CheckUserLoginSession() {
      if (CheckEmailSession()) {
        SceneManager.Instance.MoveTo(SceneManager.Scenes.HomeScreen.ToString());
      } else {
        SceneManager.Instance.MoveTo(SceneManager.Scenes.LoginScene.ToString());
      }
    }

    private bool CheckEmailSession() {
      return !string.IsNullOrEmpty(StorageHelper.ReadStorage(StorageHelper.UserSession));
    }

  }
}
