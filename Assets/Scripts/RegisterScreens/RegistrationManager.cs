using UnityEngine;

namespace KendaGame.RegistrationScreens {
  public class RegistrationManager : MonoBehaviour {
    [SerializeField]
    GameObject [] RegistrationScreens;

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
      Debug.Log("auto login clicked");
    }
  }
}