using UnityEngine;

namespace KendaGame.Games {
  public class StairsRock : MonoBehaviour {
    private void OnMouseDown() {
      StairsLevelManager.Instance.OnRockSelected(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      StairsCharacter stairsCharacter = collision.gameObject.GetComponent<StairsCharacter>();

      if(stairsCharacter != null) {
        stairsCharacter.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        stairsCharacter.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
      }
    }
  }
}