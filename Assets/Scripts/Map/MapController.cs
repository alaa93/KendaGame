using UnityEngine;
using KendaGame.Navigation;
namespace KendaGame.UI {
  public class MapController : MonoBehaviour {

    [SerializeField]
    Canvas mapCanvas;
    private void Awake() {
      if (mapCanvas && Camera.main) {
        mapCanvas.worldCamera = Camera.main;
      }
    }
  }
}