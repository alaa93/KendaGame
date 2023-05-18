using System;
using System.Collections.Generic;
using KendaGame.Helpers;
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
    public static int tracingGuide = 0;
    private bool initialized;
    private static SceneManager _instance;
    private List<string> navigationStack = new List<string>();
    private bool openManually;

    public enum Scenes {
      SplashScreen, LoginScene, HomeScreen, Map, BubblesGame, ColoringEngine, TracingBasic, SharkGame, TracingAdvanced, BoxesGame, VideoPlayer
    }

    private Scenes activeScene;

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
      if (!openManually) {
        loadingController.OpenClouds(null);
        UpdateBackBtnState(scene);
      }
    }

    public void OpenCloudsManually(Action<bool> onComplete = null, bool animated = true) {
      loadingController.OpenClouds(onComplete, animated);
    }

    public void CloseCloudsManually(Action<bool> onComplete = null, bool animated = true) {
      loadingController.CloseClouds(onComplete, animated);
    }

    public void MoveTo(string targetSceneName, bool openManually = false) {
      this.openManually = openManually;
      Debug.Log("MoveTo " + targetSceneName);
      Scenes targetScene = (Scenes)Enum.Parse(typeof(Scenes), targetSceneName);
      if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != targetSceneName) {
        loadingController.CloseClouds((animated) => {
          UpdateScreenOrientation(targetScene);
          if (navigationStack != null && !navigationStack.Contains(targetSceneName)) {
            navigationStack.Add(targetSceneName);
          }
          activeScene = targetScene;
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

    public void SetBackBtnState(bool state) {
      uiController.SetBackBtnState(state);
    }

    public void SetCoinsObjectState(bool state) {
      uiController.SetCoinsObjectState(state);
    }

    private void UpdateBackBtnState(Scene scene) {
      if (scene.name == Scenes.LoginScene.ToString() || scene.name == Scenes.SplashScreen.ToString() || scene.name == Scenes.HomeScreen.ToString()) {
        uiController.SetBackBtnState(false);
      } else {
        uiController.SetBackBtnState(true);
      }
    }

    public Scenes GetActiveScene() {
      return activeScene;
    }

    public void OnMapLetterClicked(string mapLetter) {
      if (PlayerPrefs.GetInt("PlayIntro1", 0) > 0) {
        if (LettersHelper.GetActiveLetterIndex() == 0 || LettersHelper.GetActiveLetterIndex() % 2 == 0) {
          Instance.MoveTo(Scenes.BubblesGame.ToString(), true);
        } else {
          // for now keep loading bubbles game but later this should be changed to load puzzle game or steps game
          Instance.MoveTo(Scenes.BubblesGame.ToString(), true);
        }
      } else {
        Instance.MoveTo(Scenes.VideoPlayer.ToString(), true);
      }
    }

    public void OnVideoCompletedMoveTo() {
      if (LettersHelper.GetActiveLetterIndex() == 0 || LettersHelper.GetActiveLetterIndex() % 2 == 0) {
        Instance.MoveTo(Scenes.BubblesGame.ToString(), true);
      } else {
        // for now keep loading bubbles game but later this should be changed to load puzzle game or steps game
        Instance.MoveTo(Scenes.BubblesGame.ToString(), true);
      }
    }

#if UNITY_EDITOR
    private void UpdateEditorOrientation(bool islandscape) {
      //var gameViewType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameView");
      //var gameViewWindow = UnityEditor.EditorWindow.GetWindow(gameViewType);
      //var sizeSelectionCallback = gameViewType.GetMethod("SizeSelectionCallback");
      //sizeSelectionCallback.Invoke(gameViewWindow, new object[] { islandscape ? 32 : 31, null });
    }
#endif
  }
}
