using KendaGame.Helpers;
using KendaGame.Navigation;
using UnityEngine;

namespace KendaGame.RegistrationScreens {
  public class RegistrationManager : MonoBehaviour {
    [SerializeField]
    GameObject [] RegistrationScreens;
    [SerializeField]
    UIButtonAnimator skipBtn;

    private void Awake() {
      skipBtn.AddClickListener(() => {
        AutoLogin();
      }, true);
    }

    public void ShowScreen(int screenId) {
      ClearActiveScreens();
      RegistrationScreens[screenId].SetActive(true);
    }

    public void ClearActiveScreens() {
      for (int x = 0; x < RegistrationScreens.Length; x++) {
        RegistrationScreens[x].SetActive(false);
      }
    }

    public void ShowForgetPassword() {

    }

    public void AutoLogin() {
      if (SceneManager.Instance) {
        SceneManager.Instance.MoveTo(SceneManager.Scenes.HomeScreen.ToString());
      }
    }
  }
}