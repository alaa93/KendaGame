using UnityEngine;
using KendaGame.Helpers;
using TMPro;
using I2.Loc;
using UnityEngine.UI;
using DG.Tweening;
using KendaGame.Navigation;

namespace KendaGame.UI {
  public class SettingsPopUp : MonoBehaviour {
    [SerializeField]
    UIButtonAnimator musicButton;
    [SerializeField]
    Image musicMute;
    [SerializeField]
    UIButtonAnimator sfxButton;
    [SerializeField]
    Image sfxMute;
    [SerializeField]
    UIButtonAnimator exitButton;
    [SerializeField]
    UIButtonAnimator parentsDashbordBtn;
    private bool isMusicMuted;
    private bool isSfxMuted;
    [SerializeField] Image arabicLanguageIcon;
    [SerializeField] Image englishLanguageIcon;

    private void Start() {
      isMusicMuted = !AudioController.Instance.GetMusicState();
      isSfxMuted = !AudioController.Instance.GetSfxState();

      InitializeButtons();
      UpdateButtonIcon();
      UpdateLanguageButtonIcon();
    }

    private void InitializeButtons() {
      musicButton.AddClickListener(OnMusicButtonPressed);
      sfxButton.AddClickListener(OnSfxButtonPressed);
      exitButton.AddClickListener(ShowPopUp);
      parentsDashbordBtn.AddClickListener(OnParentsDashboardClicked);
    }

    private void OnMusicButtonPressed() {
      AudioController.Instance.SetMusicState(isMusicMuted);
      isMusicMuted = !isMusicMuted;
      UpdateButtonIcon();
    }

    private void OnSfxButtonPressed() {
      AudioController.Instance.SetSfxState(isSfxMuted);
      isSfxMuted = !isSfxMuted;
      UpdateButtonIcon();
    }

    public void UpdateButtonIcon() {
      sfxMute.enabled = isSfxMuted;
      musicMute.enabled = isMusicMuted;
    }

    public void UpdateLanguageButtonIcon() {
      int langIndex = (LocalizationManager.CurrentLanguageCode == "ar") ? 1 : 0;

      arabicLanguageIcon.enabled=(langIndex == 0);
      englishLanguageIcon.enabled=(langIndex == 1);
    }


    public void OnLogoutButtonPressed() {
      PlayerPrefs.DeleteAll();
      SceneManager.Instance.MoveTo(SceneManager.Scenes.LoginScene.ToString());

    }

    public void ChangeLanguage(int langIndex){
      string languageCode = (langIndex == 0 ? "ar" : "en");
      if (LocalizationManager.CurrentLanguageCode != languageCode) {
        LocalizationManager.CurrentLanguageCode = languageCode;
        LocalizationManager.UpdateSources();
        UpdateLanguageButtonIcon();
      }
    }

    public void ShowPopUp() {
      if (!gameObject.activeInHierarchy) {
        gameObject.SetActive(true);
        transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
      } else {
        transform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() => {

          gameObject.SetActive(false);
        });
      }
    }

    public void ClosePopUp() {
      gameObject.SetActive(false);
    }

    private void OnParentsDashboardClicked() {
      PopUpsManager.Instance.GetParentsDashboardPopUp().ShowPopUp();
      ShowPopUp();
    }
  }
}
