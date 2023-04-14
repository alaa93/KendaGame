using UnityEditor;
using UnityEngine;

public class KendaEditorScript : EditorWindow {

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
}

