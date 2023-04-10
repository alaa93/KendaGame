using TMPro;
using I2.Loc;
using UnityEngine;

namespace KendaGame {
  public class LanguageSelector : MonoBehaviour {
    [SerializeField]
    TMP_Dropdown languageMenu;

    private void Start() {
      languageMenu.SetValueWithoutNotify(LocalizationManager.IsRTL(LocalizationManager.CurrentLanguageCode) ? 1 : 0);
    }

    public void OnLanguageChanged(TMP_Dropdown languageCode) {
      if (LocalizationManager.CurrentLanguageCode != languageCode.options[languageCode.value].text) {
        LocalizationManager.CurrentLanguageCode = languageCode.options[languageCode.value].text;
        LocalizationManager.UpdateSources();
      }
    }
  }
}