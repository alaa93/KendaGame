using UnityEngine;
using KendaGame.Helpers;
using TMPro;
using I2.Loc;

public class SettingsPopUp : MonoBehaviour {
  [SerializeField]
  UIButtonAnimator musicButton;
  [SerializeField]
  UIButtonAnimator musicMute;
  [SerializeField]
  UIButtonAnimator sfxButton;
  [SerializeField]
  UIButtonAnimator sfxMute;
  [SerializeField]
  UIButtonAnimator exitButton;

  [SerializeField]
  TMP_Dropdown languageDropDown;
  private bool isMutedMusic;
  private bool isMutedSfx;
  private void Start() {
    isMutedMusic = !AudioController.Instance.GetMusicState();
    isMutedSfx = !AudioController.Instance.GetSfxState();
    InitializeButtons();
  }
  private void InitializeButtons() {
    languageDropDown.onValueChanged.AddListener(OnLanguageDropdown);
    musicButton.AddClickListener(OnMusicButtonPressed);
    musicMute.AddClickListener(OnMusicButtonPressed);
    sfxButton.AddClickListener(OnSfxButtonPressed);
    sfxMute.AddClickListener(OnSfxButtonPressed);
    exitButton.AddClickListener(OnExitButtonPressed);
  }
  private void OnMusicButtonPressed() {
    isMutedMusic = !isMutedMusic;
    AudioController.Instance.SetMusicMuteState(isMutedMusic);
    musicMute.gameObject.SetActive(isMutedMusic);
  }
  private void OnSfxButtonPressed() {
    if (!isMutedSfx) {
      AudioController.Instance.SetSfxState(false);
      sfxMute.gameObject.SetActive(true);
      isMutedSfx = true;
    } else {
      AudioController.Instance.SetSfxState(true);
      sfxMute.gameObject.SetActive(false);
      isMutedSfx = false;
    }
  }
  private void OnLanguageDropdown(int langIndex) {
    string languageCode = (langIndex == 0 ? "ar" : "en");
    if (LocalizationManager.CurrentLanguageCode != languageCode) {
      LocalizationManager.CurrentLanguageCode = languageCode;
      LocalizationManager.UpdateSources();
    }
  }
  private void OnLogoutButtonPressed() {
    //Will do that after everything is figured out with the database and authentication
  }
  private void OnExitButtonPressed() {
    gameObject.SetActive(false);
  }
  public void ShowPopUp() {
    gameObject.SetActive(true);
  }
}
