using System;
using KendaGame.Helpers;
using UnityEngine;
using DG.Tweening;

namespace KendaGame.UI {
  public class Offer3MonthsPopup : MonoBehaviour {

    [SerializeField]
    UIButtonAnimator closeBtn;
    Action closeCallback = null;

    private void Start() {
      closeBtn.AddClickListener(ClosePopUp, true);
    }

    public void ShowPopUp(Action onCloseCallback = null) {
      if (!gameObject.activeInHierarchy) {
        gameObject.SetActive(true);
        transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
      }

      this.closeCallback = onCloseCallback;
    }

    public void ClosePopUp() {
      transform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() => {
        gameObject.SetActive(false);
      });
      closeCallback?.Invoke();
    }
  }
}