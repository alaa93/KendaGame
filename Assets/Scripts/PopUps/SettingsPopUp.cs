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
  UIButtonAnimator dropDownButton;

  [SerializeField]
  GameObject viewPort;
  [SerializeField]
  UIButtonAnimator arabicSelectButton;
  [SerializeField]
  UIButtonAnimator englishSelectButton;
  [SerializeField]
  GameObject dropDownArabicLabel;
  [SerializeField]
  GameObject dropDownEnglishLabel;
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
    if(LocalizationManager.CurrentLanguageCode == "ar"){
      dropDownArabicLabel.SetActive(true);
      dropDownEnglishLabel.SetActive(false);
    }
    else{
      dropDownArabicLabel.SetActive(false);
      dropDownEnglishLabel.SetActive(true);
    }
  }
  private void InitializeButtons() {
    dropDownButton.AddClickListener(ShowAndHideViewPort);
    englishSelectButton.AddClickListener(() => {SelectLanguage("en");});
    arabicSelectButton.AddClickListener(() => {SelectLanguage("ar");});
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
  private void ShowAndHideViewPort(){
    if(viewPort.activeSelf){
      viewPort.SetActive(false);
    }
    else{
      viewPort.SetActive(true);
    }
  }
  private void SelectLanguage(string languageCode) {
    
    if (LocalizationManager.CurrentLanguageCode != languageCode) {
      LocalizationManager.CurrentLanguageCode = languageCode;
      LocalizationManager.UpdateSources();
    }
    if(languageCode == "ar"){
      dropDownArabicLabel.SetActive(true);
      dropDownEnglishLabel.SetActive(false);
    }
    else{
      dropDownArabicLabel.SetActive(false);
      dropDownEnglishLabel.SetActive(true);
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
