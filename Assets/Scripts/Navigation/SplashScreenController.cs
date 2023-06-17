using UnityEngine;
using KendaGame.Navigation;
using System.Threading.Tasks;
using Spine.Unity;

namespace KendaGame.UI {
  public class SplashScreenController : MonoBehaviour {

    [SerializeField]
    [Range(2,15)]
    int logoDuration = 8;
    [SerializeField]
    SkeletonAnimation logoSpineAnim;

    private void Start() {
      SceneManager.Instance.OpenCloudsManually(onComplete:(animated)=> {
        if (animated) {
          LoadNextScreen();
        }
      });
    }

    private async void LoadNextScreen() {
      logoSpineAnim.AnimationState.SetAnimation(0, "animation", false);
     await Task.Delay(logoDuration * 1000);
      if (SceneManager.Instance) {
        SceneManager.Instance.MoveTo(SceneManager.Scenes.LoginScene.ToString());
      }
    }
  }
}
