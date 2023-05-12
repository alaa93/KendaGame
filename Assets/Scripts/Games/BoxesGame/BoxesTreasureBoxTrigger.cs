using UnityEngine;
namespace KendaGame.Games {
  public class BoxesTreasureBoxTrigger : MonoBehaviour {
    public string letterId = "أ";
    public GameObject[] parts;
    int counter;

    public void ActivatePart() {
      if(counter < parts.Length) {
        parts[counter].SetActive(true);
        counter++;
      }
    }
  }
}

