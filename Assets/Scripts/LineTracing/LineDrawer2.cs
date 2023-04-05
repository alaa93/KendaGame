using UnityEngine;

public class LineDrawer2 : MonoBehaviour {
  [SerializeField] private RectTransform lineBounds;
  [SerializeField] private float lineWidth = 0.1f;
  [SerializeField] private Color lineColor = Color.white;
  [SerializeField] private LineRenderer lineRenderer;
  [SerializeField] private Camera mainCamera;

  private Vector2 startPos;
  private Vector2 endPos;
  private bool isDrawing = false;

  void Start() {
    lineRenderer.startWidth = lineWidth;
    lineRenderer.endWidth = lineWidth;
    lineRenderer.startColor = lineColor;
    lineRenderer.endColor = lineColor;
  }

  void Update() {
    if (Input.GetMouseButtonDown(0)) {
      if (RectTransformUtility.ScreenPointToLocalPointInRectangle(lineBounds, Input.mousePosition, mainCamera, out startPos)) {
        isDrawing = true;
        endPos = startPos;
      }
    } else if (Input.GetMouseButtonUp(0)) {
      isDrawing = false;
    }

    if (isDrawing) {
      if (RectTransformUtility.ScreenPointToLocalPointInRectangle(lineBounds, Input.mousePosition, mainCamera, out endPos)) {
        DrawLine();
      }
    }
  }

  void DrawLine() {
    Vector3 worldStartPos = mainCamera.ScreenToWorldPoint(startPos + Vector2.up * mainCamera.transform.position.z);
    Vector3 worldEndPos = mainCamera.ScreenToWorldPoint(endPos + Vector2.up * mainCamera.transform.position.z);

    lineRenderer.positionCount = 2;
    lineRenderer.SetPosition(0, worldStartPos);
    lineRenderer.SetPosition(1, worldEndPos);
  }
}
