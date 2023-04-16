using UnityEngine;
using KendaGame.Navigation;
using System.Collections;

namespace KendaGame.UI {
  public class SplashScreenController : MonoBehaviour {

    private IEnumerator Start() {
      yield return new WaitForSeconds(2f);
      LoadNextScreen();
    }

    private void LoadNextScreen() {
      if (SceneManager.Instance) {
        SceneManager.Instance.MoveTo(SceneManager.Scenes.LoginScene);
      }
    }
  }
}
