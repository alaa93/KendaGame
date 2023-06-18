using I2.Loc;
using KendaGame.Helpers;
using KendaGame.Navigation;
using KendaGame.UI;
using TMPro;
using UnityEngine;

namespace KendaGame.RegistrationScreens {
  public class RegistrationManager : MonoBehaviour {
    [SerializeField]
    GameObject [] RegistrationScreens;
    [SerializeField]
    UIButtonAnimator skipBtn;
    [SerializeField]
    EmailSignInController emailSignInController;
    [Header("Login Menu")]
    [SerializeField]
    TMP_InputField loginEmailField;
    [SerializeField]
    TMP_InputField loginPasswordField;
    [Header("Signup Menu")]
    [SerializeField]
    TMP_InputField signUpEmailField;
    [SerializeField]
    TMP_InputField signUpPasswordField;
    [SerializeField]
    TMP_InputField signUpPasswordConfirmField;

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

    public void OnEmailLoginBtnPressed() {
      LoadingBar loadingBar = PopUpsManager.Instance.LoadLoadingBar();
      loadingBar.ShowLoadingBar("");
      loadingBar.IncreaseloadingBarByAnim(0.1f);
      emailSignInController.SignInWithEmail(loginEmailField.text, loginPasswordField.text, (success, status) => {
        Debug.Log("OnEmailLoginBtnPressed - Sign in with email completed");
        loadingBar.HideLoadingBar();
        if (success) {
          AutoLogin();
        } else {
          PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp("registration_screens.login_title", status);
        }
      });
    }

    public void OnEmailSignUpBtnPressed() {
      emailSignInController.SignUpWithEmail(signUpEmailField.text, signUpPasswordField.text, signUpPasswordConfirmField.text, (success, status) => {
        Debug.Log("OnEmailLoginBtnPressed - Sign in with email completed");
        PopUpsManager.Instance.GetFeedbackPopUp().ShowPopUp("registration_screens.signup_title", status, () => {
          if (success) {
            Debug.Log("Is Success " + success);
            AutoLogin();
          }
        });
      });
    }

    public void AutoLogin() {
      if (SceneManager.Instance) {
        SceneManager.Instance.MoveTo(SceneManager.Scenes.HomeScreen.ToString());
      }
    }
  }
}