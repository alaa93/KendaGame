using System.Collections;
using System.Collections.Generic;
using KendaGame.Helpers;
using UnityEngine;
using DG.Tweening;

namespace KendaGame.UI {
  public class ColoringPen : MonoBehaviour {
    private bool selected;
    [Range(0, 9)]
    public int colorId;
    [SerializeField]
    ColoringBookManager coloringBookManager;

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
      transform.DOLocalMoveY(selected ? -284f : -360.8f, 0.2f).SetEase(Ease.Linear);
      coloringBookManager.OnChangeColorButtonClicked(selected ? colorId : 0);

      if(colorId == 9) { // eraser make brush size larger for easier cleaning
        coloringBookManager.OnChangeBrushSizeButtonClicked(selected ? 2 : 1);
      } else { // otherwise make brush smaller for more smooth coloring
        coloringBookManager.OnChangeBrushSizeButtonClicked(1);
      }
    }
  }
}