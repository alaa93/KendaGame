using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using KendaGame.Navigation;
using KendaGame.Helpers;

namespace KendaGame.EditorScripts {
  public class KendaEditorScript : EditorWindow {

    private string inputValue = "";

    [MenuItem("KendaGame/Debug/Play From SplashScreen")]
    public static void PlayFromSplashScreen() {

      SaveAndLoadScene(SceneManager.Scenes.SplashScreen.ToString());
      EnterPlayMode();
    }

    [MenuItem("KendaGame/Debug/Set Active Letter")]
    public static void SetActiveLetter() {
      KendaEditorScript window = GetWindow<KendaEditorScript>("Set Active Letter");
      window.minSize = new Vector2(200f, 100f);
    }

    [MenuItem("KendaGame/Debug/Buy 10000 Coins")]
    public static void BuyCoins_10000() {
      StorageHelper.WriteStorage(StorageHelper.CoinsAmount, "110");
      Debug.Log("10,000 Coins Added");
    }

    [MenuItem("Window/Set Game View Resolution")]
    public static void ShowWindow() {
      EditorWindow.GetWindow<KendaEditorScript>("Set Game View Resolution");
    }

    private int width = 2340;
    private int height = 1080;

    private void OnGUI() {
      width = EditorGUILayout.IntField("Width", width);
      height = EditorGUILayout.IntField("Height", height);

      if (GUILayout.Button("Set Resolution")) {
        SetResolution(width, height);
      }

      ActiveLetterWindow();
    }

    private void ActiveLetterWindow() {
      GUILayout.Label("Enter a value:", EditorStyles.boldLabel);
      inputValue = EditorGUILayout.TextField("Input Value:", inputValue);

      if (GUILayout.Button("Submit")) {
        LettersHelper.SetActiveLetter(inputValue);
        Debug.Log("Active letter changed to "+inputValue + " Through Editor Script");
      }
    }

    private void SetResolution(int width, int height) {
      var gameViewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
      var gameViewWindow = EditorWindow.GetWindow(gameViewType);
      var sizeSelectionCallback = gameViewType.GetMethod("SizeSelectionCallback");
      sizeSelectionCallback.Invoke(gameViewWindow, new object[] { 32, null });
    }

    private static void SaveAndLoadScene(string sceneName, bool editorScene = false) {
      EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
      if (editorScene) {
        EditorSceneManager.OpenScene("Assets/_Scenes/" + sceneName + ".unity");
      } else {
        EditorSceneManager.OpenScene("Assets/_Scenes/" + sceneName + ".unity");
      }
    }

    private static void EnterPlayMode() {
      EditorApplication.isPlaying = true;
    }
  }
}