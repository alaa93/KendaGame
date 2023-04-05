using UnityEngine;

public class LineTracer : MonoBehaviour {

  public LineRenderer lineRendererPrefab; // The prefab for the line renderer object
  public BoxCollider2D boxCollider; // The BoxCollider2D that defines the area where lines can be drawn
  private LineRenderer currentLine; // The current line being drawn
  private Vector3 mousePosition; // The current position of the mouse

  void Update() {
    if (Input.GetMouseButtonDown(0)) { // If the left mouse button is pressed down
      currentLine = Instantiate(lineRendererPrefab); // Instantiate a new line renderer object
      currentLine.positionCount = 0; // Clear the line renderer's current positions
    } else if (Input.GetMouseButton(0)) { // If the left mouse button is held down
      mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the current position of the mouse in world space
      mousePosition.z = 0f; // Ignore the Z axis
      //if (!boxCollider.bounds.Contains(mousePosition)) { // Check if the mouse position is inside the box collider
      //  currentLine.positionCount++; // Increase the position count of the line renderer
      //  currentLine.SetPosition(currentLine.positionCount - 1, mousePosition); // Add the current mouse position to the line renderer
      //}
      if (IsInside(boxCollider, mousePosition)) { // Check if the mouse position is inside the box collider
        currentLine.positionCount++; // Increase the position count of the line renderer
        currentLine.SetPosition(currentLine.positionCount - 1, mousePosition); // Add the current mouse position to the line renderer
      }
    }
  }

  public static bool IsInside(Collider2D c, Vector2 point) {
    Vector2 closest = c.ClosestPoint(point);
    // Because closest=point if point is inside - not clear from docs I feel
    return closest == point;
  }
}
