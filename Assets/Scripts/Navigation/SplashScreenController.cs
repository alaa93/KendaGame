using UnityEngine;
using KendaGame.Navigation;
using System.Threading.Tasks;
using Spine.Unity;
using Firebase.Auth;
using KendaGame.Helpers;

namespace KendaGame.UI {
  public class SplashScreenController : MonoBehaviour {

    [SerializeField]
    [Range(2, 15)]
    int logoDuration = 8;
    [SerializeField]
    SkeletonAnimation logoSpineAnim;

    private void Start() {
      LettersHelper.PreparingLettersStatuses();
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
#if !UNITY_EDITOR
      FirebaseAuth auth = FirebaseAuth.DefaultInstance;
      FirebaseUser user = auth.CurrentUser;
      if (auth != null && user != null) {
        string name = user.DisplayName;
        string email = user.Email;
        System.Uri photo_url = user.PhotoUrl;
        // The user's Id, unique to the Firebase project.
        // Do NOT use this value to authenticate with your backend server, if you
        // have one; use User.TokenAsync() instead.
        string uid = user.UserId;
        Debug.Log("Logged in user " + uid + " name " + name + "email " + email);
        SceneManager.Instance.MoveTo(SceneManager.Scenes.HomeScreen.ToString());
      } else {
        if (CheckEmailSession()) {
          SceneManager.Instance.MoveTo(SceneManager.Scenes.HomeScreen.ToString());
        } else {
          SceneManager.Instance.MoveTo(SceneManager.Scenes.LoginScene.ToString());
        }
      }
      #else
      if (CheckEmailSession()) {
        SceneManager.Instance.MoveTo(SceneManager.Scenes.HomeScreen.ToString());
      } else {
        SceneManager.Instance.MoveTo(SceneManager.Scenes.LoginScene.ToString());
      }
      #endif
    }

    private bool CheckEmailSession() {
      if (string.IsNullOrEmpty(StorageHelper.ReadStorage(StorageHelper.UserSession))) {
        return true;
      } else {
        return false;
      }
    }

  }
}
