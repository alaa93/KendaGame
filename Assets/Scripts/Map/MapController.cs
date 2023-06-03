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

    [SerializeField]
    float fadeDuration = 2f;
    [SerializeField]
    Image[] overlay;

    private void Start() {
      //PlayOverlayAnimation(overlay[0]);
      //PlayOverlayAnimation(overlay[1]);
      //PlayOverlayAnimation(overlay[2]);
    }

    private void PlayOverlayAnimation(Image image) {
      image.DOFade(0.5f, fadeDuration).OnComplete(() => {
        image.DOFade(1f, fadeDuration).OnComplete(() => {
          PlayOverlayAnimation(image);
        });
      });
    }
  }
}