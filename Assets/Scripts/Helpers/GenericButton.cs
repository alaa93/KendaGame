using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;

namespace KendaGame.UI {
  public class GenericButton : MonoBehaviour {

    [SerializeField]
    UnityEvent onComplete;

    private const float targetScale = 0.8f;
    private const float duration = 0.16f;

    public void OnClick(Action onComplete) {
      float originalScale = gameObject.transform.localScale.x;
      gameObject.transform.DOScale(targetScale, duration).OnComplete(() => {
        gameObject.transform.DOScale(originalScale, duration).OnComplete(() => {
          onComplete?.Invoke();
        });
      });
    }

    public void OnClick() {
      float originalScale = gameObject.transform.localScale.x;
      gameObject.transform.DOScale(targetScale, duration).OnComplete(() => {
        gameObject.transform.DOScale(originalScale, duration).OnComplete(() => {
          onComplete?.Invoke();
        });
      });
    }
  }
}