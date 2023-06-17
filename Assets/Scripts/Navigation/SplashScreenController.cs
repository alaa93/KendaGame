using UnityEngine;
using KendaGame.Navigation;
using System.Threading.Tasks;
using Spine.Unity;
using Firebase.Auth;

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
      await Task.Delay(logoDuration * 1000);
      if (SceneManager.Instance) {
        CheckUserLoginSession();
      }
    }

    private void CheckUserLoginSession() {
      SceneManager.Instance.MoveTo(SceneManager.Scenes.LoginScene.ToString());
      return;
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
        SceneManager.Instance.MoveTo(SceneManager.Scenes.LoginScene.ToString());
      }
    }
  }
}
