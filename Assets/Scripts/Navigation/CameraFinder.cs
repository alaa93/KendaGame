using UnityEngine;

namespace KendaGame.Navigation {
  public class CameraFinder : MonoBehaviour {
    private void Awake() {
      if (SceneManager.Instance && Camera.main) {
        this.GetComponent<Canvas>().worldCamera = Camera.main;
      }
    }
  }
}
