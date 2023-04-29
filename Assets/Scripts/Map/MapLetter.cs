using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KendaGame.Helpers;

namespace KendaGame.Navigation {
  [RequireComponent(typeof(UIButtonAnimator))]
  public class MapLetter : MonoBehaviour {
    TextMeshProUGUI letterText;
    public int id;

    private void Awake() {
      GetComponent<UIButtonAnimator>().AddClickListener(() => {
        SceneManager.Instance.MoveTo(SceneManager.Scenes.GameScene.ToString());
      });
    }
  }
}