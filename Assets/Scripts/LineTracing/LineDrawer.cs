using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour {
  [SerializeField] private LineRenderer lineRendererPrefab;
  [SerializeField] private float minDistance = 0.1f;
  [SerializeField] private float maxDistance = 0.5f;
  [SerializeField] private RectTransform drawAreaRectTransform;

  private List<Vector3> linePoints = new List<Vector3>();
  private LineRenderer currentLineRenderer;
  private GameObject currentLineObject;
  private float currentDistance;

  public RectTransform DrawAreaRectTransform {
    get => drawAreaRectTransform;
    set => drawAreaRectTransform = value;
  }


  private void Start() {
    Debug.Log("Draw area position: " + drawAreaRectTransform.position);
    Debug.Log("Draw area size: " + drawAreaRectTransform.sizeDelta);

  }
  private void Update() {
    if (Input.GetMouseButtonDown(0)) {
      Vector3 mousePosition = Input.mousePosition;
      Vector2 localPosition;
      if (RectTransformUtility.ScreenPointToLocalPointInRectangle(drawAreaRectTransform, mousePosition, null, out localPosition)) {
        if (drawAreaRectTransform.rect.Contains(localPosition)) {
          currentLineObject = new GameObject("Line");
          currentLineRenderer = currentLineObject.AddComponent<LineRenderer>();
          currentLineRenderer.positionCount = 0;
          currentLineRenderer.material = lineRendererPrefab.sharedMaterial;
          currentLineRenderer.startWidth = lineRendererPrefab.startWidth;
          currentLineRenderer.endWidth = lineRendererPrefab.endWidth;
          currentLineRenderer.startColor = lineRendererPrefab.startColor;
          currentLineRenderer.endColor = lineRendererPrefab.endColor;

          linePoints.Add(mousePosition);
          currentLineRenderer.positionCount = 1;
          currentLineRenderer.SetPosition(0, mousePosition);

          currentDistance = 0f;
        }
      }
    } else if (Input.GetMouseButton(0)) {
      Vector3 mousePosition = Input.mousePosition;
      Vector2 localPosition;
      if (RectTransformUtility.ScreenPointToLocalPointInRectangle(drawAreaRectTransform, mousePosition, null, out localPosition)) {
        if (drawAreaRectTransform.rect.Contains(localPosition)) {
          float distance = Vector3.Distance(mousePosition, linePoints[linePoints.Count - 1]);
          if (distance > currentDistance) {
            linePoints.Add(mousePosition);
            currentLineRenderer.positionCount = linePoints.Count;
            currentLineRenderer.SetPosition(linePoints.Count - 1, mousePosition);

            currentDistance = Random.Range(minDistance, maxDistance);
          }
        }
      }
    } else if (Input.GetMouseButtonUp(0)) {
      linePoints.Clear();
      currentLineObject = null;
      currentLineRenderer = null;
    }
  }

}