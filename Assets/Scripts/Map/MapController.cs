using DG.Tweening;
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
  }
}