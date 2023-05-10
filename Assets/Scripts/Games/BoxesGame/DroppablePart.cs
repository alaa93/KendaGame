using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KendaGame.Games {
  public class DroppablePart : MonoBehaviour {
    public int id = 0;
    bool locked;

    private void OnTriggerStay2D(Collider2D collision) {
      DraggablePart draggable = collision.GetComponent<DraggablePart>();

      if(draggable && draggable.id == this.id && !locked && !draggable.mouseDown) {
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<Collider2D>().enabled = false;
        locked = true;
        draggable.locked = true;
        draggable.gameObject.SetActive(false);
        BoxesLettersController.Instance.OnGamePartSolved();
      }
    }
  }
}