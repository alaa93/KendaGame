using System.Collections;
using KendaGame.Helpers;
using TMPro;
using UnityEngine;

namespace KendaGame.Games {
  public class ShipWheelBubble : MonoBehaviour {

    [SerializeField]
    TextMeshPro letter;
    [SerializeField]
    ParticleSystem particleSystemObject;

    public void SetLetter(string letter) {
      this.letter.text = letter;
      ResetLetter();
    }

    public bool IsCorrectLetter() {
      return letter.text == LettersHelper.GetActiveLetter();
    }

    public void PlayHitEffect(bool correct) {
      if (!correct) {
        StartCoroutine(FlashEffect());
      } else {
        this.GetComponent<SpriteRenderer>().enabled = false;
        particleSystemObject.Play();
        letter.gameObject.SetActive(false);
      }
    }

    public void ResetLetter() {
      this.GetComponent<SpriteRenderer>().enabled = true;
      this.GetComponent<SpriteRenderer>().color = Color.white;
      particleSystemObject.Stop();
      letter.gameObject.SetActive(true);
    }

    private IEnumerator FlashEffect() {
      this.GetComponent<SpriteRenderer>().color = Color.red;
      yield return new WaitForSeconds(0.1f);
      this.GetComponent<SpriteRenderer>().color = Color.white;
      yield return new WaitForSeconds(0.1f);
      this.GetComponent<SpriteRenderer>().color = Color.red;
      yield return new WaitForSeconds(0.1f);
      this.GetComponent<SpriteRenderer>().color = Color.white;
      yield return new WaitForSeconds(0.1f);
      this.GetComponent<SpriteRenderer>().color = Color.red;
      yield return new WaitForSeconds(0.1f);
      this.GetComponent<SpriteRenderer>().color = Color.white;
    }
  }
}
