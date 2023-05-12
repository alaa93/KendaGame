using DG.Tweening;
using UnityEngine;
namespace KendaGame.Games {
  public class DraggablePart : MonoBehaviour {
    public bool mouseDown;
    public int id = 0;
    public bool locked;
    Vector2 initialPos;

    private void Awake() {
      initialPos = this.transform.position;
    }
    private void OnMouseDown() {
      mouseDown = true;
    }

    private void OnMouseUp() {
      mouseDown = false;

      if (!mouseDown && !locked) {
        this.transform.DOMove(initialPos, 0.4f);
      }
    }

    private void Update() {
      if (mouseDown) {
        OnDrag();
      }
    }

    private void OnDrag() {
      Vector2 dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      this.transform.position = dragPos;
    }
  }
}