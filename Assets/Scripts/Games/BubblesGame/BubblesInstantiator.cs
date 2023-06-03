using DG.Tweening;
using KendaGame.Helpers;
using KendaGame.Navigation;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KendaGame.Games {
  public class BubblesInstantiator : MonoBehaviour {

    [SerializeField]
    TextMeshPro letterGuide;
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
    [SerializeField]
    GameObject coins;
    [SerializeField]
    ParticleSystem coinsEffect;

    private static BubblesInstantiator _instance;
    public static BubblesInstantiator Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<BubblesInstantiator>();
        }
        return _instance;
      }
    }

    List<BubbleMovement> bubbles = new List<BubbleMovement>();

    int counter = 0;
    int correctCounter = 0;
    int correctAnswers;
    string[] incorrectLetters = null;
    private void Start() {
      letterGuide.text = LettersHelper.GetActiveLetter();
      incorrectLetters = LettersHelper.GetInCorrectLetters();
      StartCoroutine(InstantiateBubbles());
    }

    private IEnumerator InstantiateBubbles() {
      while (canInstantiate && counter < 100) {
        counter++;
        correctCounter++;
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));
        if (canInstantiate) {
          BubbleMovement bubble = Instantiate(bubbleInstance, bubbleParent).GetComponent<BubbleMovement>();
          bubble.gameObject.SetActive(true);
          bubbles.Add(bubble);
          if (correctCounter >= 3) {
            correctCounter = 0;
            bubble.targetLetter = LettersHelper.GetActiveLetterIndex();
            bubble.letterText.text = LettersHelper.GetActiveLetter();
          } else {
            bubble.targetLetter = Random.Range(0, incorrectLetters.Length - 1);
            bubble.letterText.text = incorrectLetters[bubble.targetLetter];
          }
        }
      }
    }

    public void OnCorrect() {
      if (correctAnswers >= 5) {
        canInstantiate = false;
        gameMusic.Stop();
        AudioController.Instance.PlayAudioAtPoint(endingSfx);
        StartCoroutine(OnGameCompleted(endingSfx.length + 0.5f));
      }
    }

    public void PlayFillEffect() {
      correctAnswers++;
      coins.transform.DOMoveY(coins.transform.position.y + 0.24f, 1);
      coinsEffect.Play();
      SceneManager.Instance.IncrementCoins(10);
      OnCorrect();
    }

    private IEnumerator OnGameCompleted(float waitSeconds) {
      yield return new WaitForSeconds(waitSeconds);
      for (int x = 0; x < bubbles.Count; x++) {
        if (bubbles[x].targetLetter != 0) {
          bubbles[x].gameObject.SetActive(false);
        }
      }
      PopUpsManager.Instance.GetGameRatingPopUp().ShowPopUp();
    }
  }
}