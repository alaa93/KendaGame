using UnityEngine;

namespace KendaGame.Navigation {
  public class SceneManager : MonoBehaviour {
    private static SceneManager _instance;
    public static SceneManager Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<SceneManager>();
        }
        return _instance;
      }
    }

    private void Awake() {
      DontDestroyOnLoad(gameObject);
    }

    public void MoveTo(string targetSceneName) {
      //TODO implement scene navigation logic
    }

    public void OnBackClicked() {
      //TODO implement back button navigation logic
      //all scenes should be maintained inside a stack
    }
  }
}
