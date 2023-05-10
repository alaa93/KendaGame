using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KendaGame.Games {
  public class BoxesTreasureBoxTrigger : MonoBehaviour {
    public string letterId = "أ";

    public void OnTriggered(GameObject gameObject) {
      Debug.Log("OnTriggered");
      this.GetComponentInParent<BoxesTreasureBox>().OnTriggered(gameObject);
    }
  }
}

