using System;
using I2.Loc;
using KendaGame.Helpers;
using TMPro;
using UnityEngine;
using DG.Tweening;


namespace KendaGame.UI {
  public class FeedbackPopUp : MonoBehaviour {

    [SerializeField]
    UIButtonAnimator closeBtn;
    [SerializeField]
    Localize title, message;
    [SerializeField]
    GameObject titleHolder;
    Action closeCallback = null;
    private void Start() {
      closeBtn.AddClickListener(ClosePopUp, true);
    }

    public void ShowPopUp(string title = null, string message = null, Action onCloseCallback = null) {
      if (!gameObject.activeInHierarchy) {
        gameObject.SetActive(true);
        transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
      } 

      this.closeCallback = onCloseCallback;
      titleHolder.SetActive(!string.IsNullOrEmpty(title));
      this.title.Term = !string.IsNullOrEmpty(title) ? title : "";
      this.message.Term = !string.IsNullOrEmpty(message) ? message : "";
    }

    public void ClosePopUp() {
      transform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() => {
        gameObject.SetActive(false);
      });
      closeCallback?.Invoke();
    }
  }
}