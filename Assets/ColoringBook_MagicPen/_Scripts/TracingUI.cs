using System.Collections;
using System.Collections.Generic;
using KendaGame.Helpers;
using UnityEngine;

namespace KendaGame.UI {
  public class TracingUI : MonoBehaviour {
    [SerializeField]
    ColoringPen[] penBtns;

    private static TracingUI _instance;
    public static TracingUI Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<TracingUI>();
        }
        return _instance;
      }
    }


    private void Awake() {
      penBtns = FindObjectsOfType<ColoringPen>();
    }

    public void OnPenClicked(ColoringPen pen) {
      for (int x = 0; x < penBtns.Length; x++) {
        int temp = x;
        if (penBtns[temp] != pen) {
          penBtns[temp].SelectionChanged(true);
        }
      }
      pen.SelectionChanged();
    }
  }
}