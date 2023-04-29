using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using KendaGame.Navigation;

namespace KendaGame.EditorScripts {
  public class KendaEditorScript : EditorWindow {

    [MenuItem("KendaGame/Debug/Play From SplashScreen")]
    public static void PlayFromSplashScreen() {

      SaveAndLoadScene(SceneManager.Scenes.SplashScreen.ToString());
      EnterPlayMode();
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