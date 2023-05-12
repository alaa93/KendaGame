using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KendaGame.Games {
  public class BoxesLetterReseter : MonoBehaviour {

    public int direction = 1;

    private void OnCollisionEnter2D(Collision2D collision) {
      LetterCharacterController letter = collision.gameObject.GetComponent<LetterCharacterController>();
      if (letter != null) {
        letter.ReverseDirection(direction);
      }
    }
  }
}
