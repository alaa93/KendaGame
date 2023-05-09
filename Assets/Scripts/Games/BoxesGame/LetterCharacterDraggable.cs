using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KendaGame.Games {
  public class LetterCharacterDraggable : MonoBehaviour {
    LetterCharacterController letterController;
    private bool mouseDown;


    private void Awake() {
      letterController = this.GetComponent<LetterCharacterController>();
    }
    private void OnMouseDown() {
      mouseDown = true;
      letterController.SetRunState(false);
      letterController.SetHoldingAnimation(true);
    }

    private void OnMouseUp() {
      mouseDown = false;
      letterController.SetRunState(true);
      letterController.SetHoldingAnimation(false);
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