using UnityEngine;

namespace KendaGame.Games {
  public class BoxesLetterReseter : MonoBehaviour {

    public bool bottomReseter;
    public int direction = 1;

    private void OnCollisionEnter2D(Collision2D collision) {
      LetterCharacterController letter = collision.gameObject.GetComponent<LetterCharacterController>();
      if (letter != null) {
        if (bottomReseter) {
          letter.ResetPosition();
        }
        letter.ReverseDirection(direction);
      }
    }

  }
}
