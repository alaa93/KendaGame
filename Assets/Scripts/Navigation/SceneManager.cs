using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace KendaGame.Navigation {
  public class SceneManager : MonoBehaviour {

    public static SceneManager Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<SceneManager>();
        }
        return _instance;
      }
    }
    [SerializeField]
    LoadingController loadingController;
    [SerializeField]
    UIController uiController;

    private bool initialized;
    private static SceneManager _instance;
    public List<string> navigationStack = new List<string>();

    public enum Scenes {
      LoginScene, SplashScreen,Map
    }

    private void Awake() {
      DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() {
      UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadMode) {
      if (!initialized) {
        initialized = true;
      }
      loadingController.OpenClouds(null);
      UpdateBackBtnState(scene);
    }

    public void MoveTo(Scenes targetSceneName) {
      string sceneName = targetSceneName.ToString();
      Debug.Log(sceneName);
      if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != sceneName) {
        loadingController.CloseClouds((animated) => {
          UpdateScreenOrientation(targetSceneName);
          if (navigationStack != null && !navigationStack.Contains(sceneName)) {
            navigationStack.Add(sceneName);
          }
          UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        });
      } else {
        loadingController.OpenClouds(null, false);
      }
    }

    private void UpdateScreenOrientation(Scenes targetSceneName) {
      if (targetSceneName != Scenes.LoginScene && (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)) {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

#if UNITY_EDITOR
        var gameViewType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameView");
        var gameViewWindow = UnityEditor.EditorWindow.GetWindow(gameViewType);
        var sizeSelectionCallback = gameViewType.GetMethod("SizeSelectionCallback");
        sizeSelectionCallback.Invoke(gameViewWindow, new object[] { 32, null });
#endif
      }
    }

    public void OnBackClicked() {
      if (navigationStack != null && navigationStack.Count > 1) {
        MoveTo((Scenes)navigationStack.Count - 2);
        navigationStack.RemoveAt(navigationStack.Count - 1);
      } else {
        Debug.LogError("Stack is empty can't go back");
      }
    }

    private void UpdateBackBtnState(Scene scene) {
      if (scene.name == Scenes.LoginScene.ToString() || scene.name == Scenes.SplashScreen.ToString()) {
        uiController.SetBackBtnState(false);
      } else {
        uiController.SetBackBtnState(true);
      }
    }
  }
}
