using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
namespace KendaGame.Games {
  public class DraggablePart : MonoBehaviour {
    public bool mouseDown;
    public int id = 0;
    public bool locked;
    Vector2 initialPos;
    bool isReseting = false;

    private void Awake() {
      initialPos = this.transform.position;
    }
    private void OnMouseDown() {
      mouseDown = true;
    }

    private void OnMouseUp() {
      mouseDown = false;

      if (!mouseDown && !locked) {
        ResetPart();
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

    private async void ResetPart() {
      this.transform.DOMove(initialPos, 0.4f).SetDelay(0.1f);
      await Task.Delay(100);
      isReseting = true;
      await Task.Delay(400);
      isReseting = false;
    }

    public bool IsReseting() {
      return isReseting;
    }

  }
}