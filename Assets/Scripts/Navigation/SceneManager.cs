using System;
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
    private List<string> navigationStack = new List<string>();

    public enum Scenes {
      SplashScreen, LoginScene, HomeScreen, Map
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

    public void MoveTo(string targetSceneName) {
      Debug.Log("MoveTo " + targetSceneName);
      Scenes targetScene = (Scenes)Enum.Parse(typeof(Scenes), targetSceneName);
      if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != targetSceneName) {
        loadingController.CloseClouds((animated) => {
          UpdateScreenOrientation(targetScene);
          if (navigationStack != null && !navigationStack.Contains(targetSceneName)) {
            navigationStack.Add(targetSceneName);
          }
          UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(targetSceneName);
        });
      } else {
        loadingController.OpenClouds(null, false);
      }
    }

    private void UpdateScreenOrientation(Scenes targetSceneName) {
      if (targetSceneName != Scenes.LoginScene && (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)) {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
#if UNITY_EDITOR
        UpdateEditorOrientation(true);
#endif
      } else if (targetSceneName == Scenes.LoginScene || targetSceneName == Scenes.SplashScreen) {
        Screen.orientation = ScreenOrientation.Portrait;
#if UNITY_EDITOR
        UpdateEditorOrientation(false);
#endif
      }
    }

    public void OnBackClicked() {
      if (navigationStack != null && navigationStack.Count > 1) {
        MoveTo(navigationStack[navigationStack.Count - 2]);
        navigationStack.RemoveAt(navigationStack.Count - 1);
      } else {
        Debug.LogError("Stack is empty can't go back");
      }
    }

    private void UpdateBackBtnState(Scene scene) {
      if (scene.name == Scenes.LoginScene.ToString() || scene.name == Scenes.SplashScreen.ToString() || scene.name == Scenes.HomeScreen.ToString()) {
        uiController.SetBackBtnState(false);
      } else {
        uiController.SetBackBtnState(true);
      }
    }

#if UNITY_EDITOR
    private void UpdateEditorOrientation(bool islandscape) {
      var gameViewType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameView");
      var gameViewWindow = UnityEditor.EditorWindow.GetWindow(gameViewType);
      var sizeSelectionCallback = gameViewType.GetMethod("SizeSelectionCallback");
      sizeSelectionCallback.Invoke(gameViewWindow, new object[] { islandscape ? 32 : 31, null });
    }
#endif
  }
}
