using UnityEngine;
using UnityEngine.UI;

namespace KendaGame.Games {
  public class BoxesTreasureBoxTrigger : MonoBehaviour {
    public string letterId = "أ";
    public GameObject[] parts;
    int counter;
    float alpha = 0.25f;
    public void ActivatePart() {
      //alpha += 0.25f;
      //parts[0].GetComponent<Image>().color = new Color(255f,255f,255f, alpha);
      //if(counter < parts.Length) {
      //  parts[counter].SetActive(true);
      //  counter++;
      //}
    }
  }
}

