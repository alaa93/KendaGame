using KendaGame.Helpers;
using TMPro;
using UnityEngine;

namespace KendaGame.Games {
  public class ShipWheelBubble : MonoBehaviour {

    [SerializeField]
    TextMeshPro letter;

    private void OnTriggerEnter2D(Collider2D collision) {
      
    }

    public void SetLetter(string letter) {
      this.letter.text = letter;
    }

    public bool IsCorrectLetter() {
      return letter.text == LettersHelper.GetActiveLetter();
    }
  }
}
