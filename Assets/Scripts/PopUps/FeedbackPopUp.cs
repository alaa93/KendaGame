using System;
using KendaGame.Helpers;
using TMPro;
using UnityEngine;

namespace KendaGame.UI {
  public class FeedbackPopUp : MonoBehaviour {

    [SerializeField]
    UIButtonAnimator closeBtn;
    [SerializeField]
    TextMeshProUGUI title, message;
    [SerializeField]
    GameObject titleHolder;
    Action closeCallback = null;
    private void Start() {
      closeBtn.AddClickListener(ClosePopUp, true);
    }

    public void ShowPopUp(string title = null, string message = null, Action onCloseCallback = null) {
      this.closeCallback = onCloseCallback;
      titleHolder.SetActive(!string.IsNullOrEmpty(title));
      this.title.text = !string.IsNullOrEmpty(title) ? title : "";
      this.message.text = !string.IsNullOrEmpty(message) ? message : "";
      gameObject.SetActive(true);
    }

    public void ClosePopUp() {
      gameObject.SetActive(false);
      closeCallback?.Invoke();
    }
  }
}