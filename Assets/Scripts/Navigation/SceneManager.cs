using UnityEngine;
using UnityEngine.SceneManagement;
namespace KendaGame.Navigation {
  public class SceneManager : MonoBehaviour {

    [SerializeField]
    ScreenLoading screenLoading;
    bool initialized;
    private static SceneManager _instance;
    public static SceneManager Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<SceneManager>();
        }
        return _instance;
      }
    }
    public enum Scenes {
      LoginScene, SplashScreen
    }

    private void Awake() {
      DontDestroyOnLoad(gameObject);
    }

    private void Start() {
      MoveTo(Scenes.LoginScene);
      Invoke(nameof(Test), 6f);
    }

    private void Test() {
      MoveTo(Scenes.SplashScreen);
    }

    private void OnEnable() {
      UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadMode) {
      if (!initialized) {
        initialized = true;
      } else {
        screenLoading.OpenClouds(null);
      }
    }

    public void MoveTo(Scenes targetSceneName) {
      if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != targetSceneName.ToString()) {
        screenLoading.CloseClouds((animated) => {
          UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(targetSceneName.ToString());
        });
      } else {
        screenLoading.OpenClouds(null, false);
      }
    }

    public void OnBackClicked() {
      //TODO implement back button navigation logic
      //all scenes should be maintained inside a stack
    }
  }
}
