using UnityEngine;
using KendaGame.Helpers;
using TMPro;
using I2.Loc;
using UnityEngine.UI;
using DG.Tweening;


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
    private bool isMusicMuted;
    private bool isSfxMuted;

    private void Start() {
      isMusicMuted = !AudioController.Instance.GetMusicState();
      isSfxMuted = !AudioController.Instance.GetSfxState();

      InitializeButtons();
      UpdateButtonIcon();
    }

    private void InitializeButtons() {
      musicButton.AddClickListener(OnMusicButtonPressed);
      sfxButton.AddClickListener(OnSfxButtonPressed);
      exitButton.AddClickListener(ShowPopUp);
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
    private void OnLogoutButtonPressed() {
    }

    public void ShowPopUp() {
      if (!gameObject.activeInHierarchy) {
        gameObject.SetActive(true);
        transform.DOScale(1, 0.5f).SetEase(Ease.Flash);
      } else {
        transform.DOScale(0, 0.5f).SetEase(Ease.Flash).OnComplete(() => {

          gameObject.SetActive(false);
        });
      }
    }
  }
}
