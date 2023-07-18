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
    isMutedMusic = !AudioController.Instance.GetMusicOnState();
    isMutedSfx = !AudioController.Instance.GetSfxOnState();
    if(isMutedMusic){
      musicMute.gameObject.SetActive(true);
    }
    if(isMutedSfx){
      sfxMute.gameObject.SetActive(true);
    }
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
    if(!isMutedMusic){
      MuteMusic();
    }
    else{
      UnMuteMusic();
    }
  }
  private void OnSfxButtonPressed() {
    if(!isMutedSfx){
      MuteSfx();
    }
    else{
      UnMuteSfx();
    }
  }
  private void MuteMusic(){
    isMutedMusic = true;
    AudioController.Instance.SetMusicOnState(!isMutedMusic);
    musicMute.gameObject.SetActive(isMutedMusic);
  }
  private void UnMuteMusic(){
    isMutedMusic = false;
    AudioController.Instance.SetMusicOnState(!isMutedMusic);
    musicMute.gameObject.SetActive(isMutedMusic);
  }
  private void MuteSfx(){
    isMutedSfx = true;
    AudioController.Instance.SetSfxOnState(!isMutedSfx);
    sfxMute.gameObject.SetActive(isMutedSfx);
  }
  private void UnMuteSfx(){
    isMutedSfx = false;
    AudioController.Instance.SetSfxOnState(!isMutedSfx);
    sfxMute.gameObject.SetActive(isMutedSfx);
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
