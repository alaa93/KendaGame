using KendaGame.Helpers;
using KendaGame.Navigation;
using KendaGame.UI;
using UnityEngine;

namespace KendaGame.UI {
  public class HomeScreenController : MonoBehaviour {

    [SerializeField]
    UIButtonAnimator playBtn;
    [SerializeField]
    UIButtonAnimator settingsBtn;
    [SerializeField]
    UIButtonAnimator menuBtn;
    [SerializeField]
    UIButtonAnimator storeBtn;

    private void Awake() {
      playBtn.AddClickListener(OnPlayBtnClicked);
      settingsBtn.AddClickListener(OnSettingsBtnClicked);
      menuBtn.AddClickListener(OnMenuButtonClicked);
      storeBtn.AddClickListener(OnStoreButtonClicked);
      if (PlayerPrefs.GetInt("GenderSelected", 0) == 0) {
        PopUpsManager.Instance.GetGenderPopUp().ShowPopUp();
      }
    }

    private void OnPlayBtnClicked() {
      SceneManager.Instance.MoveTo(SceneManager.Scenes.Map.ToString());
    }
    
    private void OnSettingsBtnClicked(){
      PopUpsManager.Instance.GetSettingsPopUp().ShowPopUp();
    }

    private void OnMenuButtonClicked(){
      PopUpsManager.Instance.GetDashBoardPopUp().ShowPopUp();
    }
    private void OnStoreButtonClicked(){
      PopUpsManager.Instance.GetStorePopUp().ShowPopUp();
    }
  }
}
