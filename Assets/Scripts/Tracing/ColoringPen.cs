using KendaGame.Helpers;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using static KendaGame.Navigation.SceneManager;

namespace KendaGame.UI {
  public class ColoringPen : MonoBehaviour {
    private bool selected;
    [Range(0, 9)]
    public int colorId;


    private void Awake() {
      GetComponent<UIButtonAnimator>().AddClickListener(OnPenClicked, false);
    }

    private void OnPenClicked() {
      if (SceneManager.GetActiveScene().name == Scenes.ColoringEngine.ToString()) {
        ColoringUI.Instance.OnPenClicked(this);
      } else {
        TracingUI.Instance.OnPenClicked(this);
      }
    }

    public void SelectionChanged(bool forceDeselect = false) {
      selected = !selected;

      if (forceDeselect) {
        selected = false;
      }
      transform.DOLocalMoveY(selected ? -330.74f : -427.74f, 0.2f).SetEase(Ease.Linear);
      ColoringBookManager.Instance.OnChangeColorButtonClicked(selected ? colorId : 0);

      //if(colorId == 9) { // eraser make brush size larger for easier cleaning
      //  ColoringBookManager.Instance.OnChangeBrushSizeButtonClicked(selected ? 2 : 1);
      //}
    }
  }
}