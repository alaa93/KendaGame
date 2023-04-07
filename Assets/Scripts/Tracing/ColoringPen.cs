using System.Collections;
using System.Collections.Generic;
using KendaGame.Helpers;
using UnityEngine;
using DG.Tweening;

namespace KendaGame.UI {
  public class ColoringPen : MonoBehaviour {
    private bool selected;

    private void Awake() {
      GetComponent<UIButtonAnimator>().AddClickListener(OnPenClicked, false);
    }

    private void OnPenClicked() {
      TracingUI.Instance.OnPenClicked(this);
    }

    public void SelectionChanged(bool forceDeselect = false) {
      selected = !selected;

      if (forceDeselect) {
        selected = false;
      }
      transform.DOLocalMoveY(selected ? -226f : -316.2f, 0.2f).SetEase(Ease.Linear);
    }
  }
}