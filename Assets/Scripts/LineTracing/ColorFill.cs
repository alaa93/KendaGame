using UnityEngine;

public class ColorFill : MonoBehaviour {

  public Collider2D[] fillAreas; // The Collider2D array that defines the areas to be colored
  public Color[] colors; // An array of colors to randomly select from
  private Color currentColor; // The current color being used to color the fill areas

  void Start() {
    currentColor = Color.white; // Initialize the current color to white
  }

  void Update() {
    if (Input.GetMouseButtonDown(0)) {
      // Select a new random color on mouse down
      currentColor = colors[Random.Range(0, colors.Length)];
    }

    if (Input.GetMouseButton(0)) {
      // Color the fill areas while the mouse button is held down
      Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      foreach (Collider2D fillArea in fillAreas) {
        if (fillArea.OverlapPoint(mousePosition)) {
          Collider2D[] colliders = Physics2D.OverlapPointAll(mousePosition);
          foreach (Collider2D collider in colliders) {
            if (collider == fillArea) {
              collider.GetComponent<Renderer>().material.color = currentColor;
            }
          }
        }
      }
    }
  }
}
