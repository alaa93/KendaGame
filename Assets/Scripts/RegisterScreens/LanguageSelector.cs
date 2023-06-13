using TMPro;
using I2.Loc;
using UnityEngine;
using KendaGame.Helpers;

namespace KendaGame {
  public class LanguageSelector : MonoBehaviour {

    [SerializeField]
    UIButtonAnimator[] languageBtns;

    private void Start() {
      //languageMenu.SetValueWithoutNotify(LocalizationManager.IsRTL(LocalizationManager.CurrentLanguageCode) ? 1 : 0);
      languageBtns[0].AddClickListener(() => {
        //En Btn Pressed
        OnLanguageChanged("en");
      }, animate: true);
      languageBtns[1].AddClickListener(() => {
        //AR Btn Pressed
        OnLanguageChanged("ar");
      }, animate: true);
    }

    public void OnLanguageChanged(string languageCode) {
      if (LocalizationManager.CurrentLanguageCode != languageCode) {
        LocalizationManager.CurrentLanguageCode = languageCode;
        LocalizationManager.UpdateSources();
      }
    }
  }
}