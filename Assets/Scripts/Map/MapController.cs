using DG.Tweening;
using KendaGame.Helpers;
using KendaGame.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace KendaGame.UI {
  public class MapController : MonoBehaviour {

    private static MapController _instance;
    public static MapController Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<MapController>();
        }
        return _instance;
      }
    }

    public Sprite coloredStar, emptyStar;

    private void Awake() {
      LettersHelper.PreparingLettersStatuses(onComplete => {
        if (onComplete) {
          MapLetter[] mapLetters = GameObject.FindObjectsOfType<MapLetter>();
          for (int i = 0; i < mapLetters.Length; i++) {
            mapLetters[i].CheckLevel();
          }
        }
      });
    }
  }
}