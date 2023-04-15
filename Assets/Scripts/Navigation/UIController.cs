using UnityEngine;
using UnityEngine.UI;
using System;
using KendaGame.Helpers;

namespace KendaGame.Navigation {
  public class UIController : MonoBehaviour {
    [SerializeField]
    LoadingController loadingController;
    [SerializeField]
    UIButtonAnimator backBtn;
    [SerializeField]
    GameObject coinsObject;

    private void Awake() {
      InitUI();
    }

    private void InitUI() {
      coinsObject.SetActive(false);
      InitBackBtn();
    }

    private void InitBackBtn() {
      backBtn.gameObject.SetActive(false);
      backBtn.AddClickListener(() => {
        SceneManager.Instance.OnBackClicked();
      });
    }

    public void SetBackBtnState(bool state) {
      backBtn.gameObject.SetActive(state);
    }
  }
}
