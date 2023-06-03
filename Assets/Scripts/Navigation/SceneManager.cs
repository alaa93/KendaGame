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
    [SerializeField]
    Camera uiCamera;

    private bool initialized;
    private static SceneManager _instance;
    private List<string> navigationStack = new List<string>();
    private bool openManually;

    public enum Scenes {
      SplashScreen, LoginScene, HomeScreen, Map, BubblesGame, ColoringEngine, TracingBasic, SharkGame, TracingAdvanced, BoxesGame, VideoPlayer, PuzzleGame, StairsGame
    }

    public Scenes ActiveScene { get => activeScene; set => activeScene = value; }
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
        UpdateUIBtnsState(scene);
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
      if (GetActiveScene().ToString() != targetSceneName) {
        loadingController.CloseClouds((animated) => {
          UpdateScreenOrientation(targetScene);
          if (navigationStack != null && !navigationStack.Contains(targetSceneName)) {
            navigationStack.Add(targetSceneName);
          }
          ActiveScene = targetScene;
          uiCamera.transform.position = new Vector3(0f, 0f, -10f);
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
      if(PopUpsManager.Instance.IsGameRatingActive()) {
        PopUpsManager.Instance.GetGameRatingPopUp().gameObject.SetActive(false);
      }
      if (PopUpsManager.Instance.IsLetterRatingActive()) {
        PopUpsManager.Instance.GetLetterRatingPopUp().gameObject.SetActive(false);
      }
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

    public void IncrementCoins(int addedAmount) {
      uiController.IncrementCoins(addedAmount);
    }

    private void UpdateUIBtnsState(Scene scene) {
      if (scene.name == Scenes.LoginScene.ToString() || scene.name == Scenes.SplashScreen.ToString() || scene.name == Scenes.HomeScreen.ToString()) {
        uiController.SetBackBtnState(false);
      } else {
        uiController.SetBackBtnState(true);
      }

      if (scene.name == Scenes.LoginScene.ToString() || scene.name == Scenes.SplashScreen.ToString() || scene.name == Scenes.Map.ToString() || scene.name != Scenes.HomeScreen.ToString() || !scene.name.Contains("Game")) {
        uiController.SetCoinsObjectState(false);
      } else {
        uiController.SetCoinsObjectState(true);
      }

      if(scene.name == Scenes.HomeScreen.ToString()) {
        uiController.SetCoinsObjectState(true);
      }
    }

    public Scenes GetActiveScene() {
      string loadedScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.ToString();
      return (Scenes)Enum.Parse(typeof(Scenes), loadedScene);
    }

    public void OnMapLetterClicked(int mapLetterIndex) {
      LettersHelper.SetActiveLetter(mapLetterIndex);
      if (PlayerPrefs.GetInt("PlayIntro1", 0) > 0) {
        PlayerPrefs.SetInt("PlayIntro1", 1);
        //TODO implement inside letter progress through playerPrefs class
        if (LettersHelper.GetActiveLetterIndex() == 0 || LettersHelper.GetActiveLetterIndex() % 2 == 0) {
          Instance.MoveTo(Scenes.BubblesGame.ToString());
        } else {
          Instance.MoveTo(Scenes.PuzzleGame.ToString());
        }
      } else {
        Instance.MoveTo(Scenes.VideoPlayer.ToString(), true);
      }
    }

    public void OnVideoCompletedMoveTo() {
      if (LettersHelper.GetActiveLetterIndex() == 0 || LettersHelper.GetActiveLetterIndex() % 2 == 0) {// this is to ensure we always have different set of games for each letter
        Instance.MoveTo(Scenes.BubblesGame.ToString(), true);
      } else {
        // for now keep loading bubbles game but later this should be changed to load puzzle game or steps game
        Instance.MoveTo(Scenes.PuzzleGame.ToString(), true);
      }
    }

    public void OnNextScreenBtnClicked() {
      if (GetActiveScene() == Scenes.BubblesGame || GetActiveScene() == Scenes.PuzzleGame) {
        Instance.MoveTo(Scenes.ColoringEngine.ToString());
      } else if (GetActiveScene() == Scenes.ColoringEngine) {
        if (LettersHelper.GetActiveLetterIndex() % 2 == 0) {
          Instance.MoveTo(Scenes.SharkGame.ToString());
        } else {
          Instance.MoveTo(Scenes.StairsGame.ToString());
        }
      } else if (GetActiveScene() == Scenes.SharkGame || GetActiveScene() == Scenes.StairsGame) {
        Instance.MoveTo(Scenes.TracingBasic.ToString());
      } else if (GetActiveScene() == Scenes.TracingBasic) {
        Instance.MoveTo(Scenes.BoxesGame.ToString());
      } else if (GetActiveScene() == Scenes.BoxesGame) {
        Instance.MoveTo(Scenes.TracingAdvanced.ToString());
      } else if (GetActiveScene() == Scenes.TracingAdvanced) {
        Instance.MoveTo(Scenes.Map.ToString());
      }
    }

    public void ReloadScene() {
      UnityEngine.SceneManagement.SceneManager.LoadScene(GetActiveScene().ToString());
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
