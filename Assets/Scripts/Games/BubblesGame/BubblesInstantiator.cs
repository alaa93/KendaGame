using KendaGame.Helpers;
using KendaGame.Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KendaGame.Games {
  public class BubblesInstantiator : MonoBehaviour {
    [SerializeField]
    bool canInstantiate;
    [SerializeField]
    GameObject bubbleInstance;
    [SerializeField]
    Transform bubbleParent;
    [SerializeField]
    AudioClip endingSfx;
    [SerializeField]
    AudioSource gameMusic;
    List<BubbleMovement> bubbles = new List<BubbleMovement>();
    int counter = 0;
    int correctCounter = 0;
    int correctAnswers;
    string[] arabicAlphabet = new string[] {
    "أ", "ب", "ت", "ث", "ج", "ح", "خ", "د", "ذ", "ر", "ز", "س", "ش", "ص", "ض", "ط", "ظ", "ع", "غ", "ف", "ق", "ك", "ل", "م", "ن", "ه", "و", "ي"
};
    private void Start() {
      StartCoroutine(InstantiateBubbles());
    }

    private IEnumerator InstantiateBubbles() {
      while (canInstantiate && counter < 100) {
        counter++;
        correctCounter++;
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        if (canInstantiate) {
          BubbleMovement bubble = Instantiate(bubbleInstance, bubbleParent).GetComponent<BubbleMovement>();
          bubble.gameObject.SetActive(true);
          bubbles.Add(bubble);
          if (correctCounter >= 3) {
            correctCounter = 0;
            bubble.targetLetter = 0;
            bubble.letterText.text = LettersHelper.GetActiveLetter();
          } else {
            bubble.targetLetter = Random.Range(0, arabicAlphabet.Length - 1);
            bubble.letterText.text = arabicAlphabet[bubble.targetLetter];
          }
        }
      }
    }

    public void OnCorrect() {
      correctAnswers++;

      if (correctAnswers >= 3) {
        canInstantiate = false;
        gameMusic.Stop();
        AudioController.Instance.PlayAudioAtPoint(endingSfx);
        for (int x = 0; x < bubbles.Count; x++) {
          if (bubbles[x].targetLetter != 0) {
            bubbles[x].gameObject.SetActive(false);
          }
        }
        StartCoroutine(OnGameCompleted(endingSfx.length + 0.5f));
      }
    }

    private IEnumerator OnGameCompleted(float waitSeconds) {
      yield return new WaitForSeconds(waitSeconds);
      PopUpsManager.Instance.LoadRatingLevel().ShowPopUp();
    }
  }
}