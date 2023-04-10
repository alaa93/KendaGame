using TMPro;
using UnityEngine;


namespace KendaGame.Localization {
  public class LocalizationController : MonoBehaviour {
    public void OnLanguageChanged(TMP_Dropdown languageCode) {
      if(I2.Loc.LocalizationManager.CurrentLanguageCode != languageCode.options[languageCode.value].text) {
        I2.Loc.LocalizationManager.CurrentLanguageCode = languageCode.options[languageCode.value].text;
        I2.Loc.LocalizationManager.UpdateSources();
      }
    }
  }
}
