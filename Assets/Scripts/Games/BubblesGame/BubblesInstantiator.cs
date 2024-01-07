using DG.Tweening;
using KendaGame.Helpers;
using KendaGame.Navigation;
using KendaGame.Tutorial;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KendaGame.Games {
  public class BubblesInstantiator : MonoBehaviour {

    private static BubblesInstantiator _instance;
    public static BubblesInstantiator Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<BubblesInstantiator>();
        }
        return _instance;
      }
    }

    [SerializeField] TextMeshPro letterGuide;
    [SerializeField] bool canInstantiate;
    [SerializeField] GameObject bubbleInstance;
    [SerializeField] Transform bubbleParent;
    [SerializeField] AudioClip endingSfx;
    [SerializeField] AudioSource gameMusic;
    [SerializeField] GameObject coins;
    [SerializeField] ParticleSystem coinsEffect;
    bool shouldShowTutorial;
    int correctCounter = 0;
    int correctAnswers;
    string[] incorrectLetters = null;
    private bool gameCompleted;
    private bool canClickBubble = true;
    List<BubbleMovement> bubbles = new List<BubbleMovement>();
    TutorialController tutorialController;
    Coroutine tutorialCoroutine;
    private int wrongAnswersNumber = 0;
    int wrongmax ;

    private void Start() {
      letterGuide.text = LettersHelper.GetActiveLetter();
      incorrectLetters = LettersHelper.GetInCorrectLetters();
      shouldShowTutorial = true;
      if (shouldShowTutorial) {
       // canClickBubble = false;
        BubbleMovement bubble = Instantiate(bubbleInstance, this.transform).GetComponent<BubbleMovement>();
        bubble.gameObject.SetActive(true);
        bubble.targetLetter = LettersHelper.GetActiveLetterIndex();
        bubble.letterText.text = LettersHelper.GetActiveLetter();
        bubbles.Add(bubble);
      }
      StartCoroutine(InstantiateBubbles());
    }

    private IEnumerator InstantiateBubbles() {
      while (canInstantiate && !shouldShowTutorial) {
        correctCounter++;
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));
        if (canInstantiate) {
          BubbleMovement bubble = Instantiate(bubbleInstance, bubbleParent).GetComponent<BubbleMovement>();
          bubble.gameObject.SetActive(true);
          bubble.transform.position = new Vector2(bubbleParent.transform.position.x, bubble.transform.position.y);
          bubbles.Add(bubble);
          if (correctCounter >= wrongmax) {
            correctCounter = 0;
            wrongmax = Random.Range(2, 5);
            bubble.targetLetter = LettersHelper.GetActiveLetterIndex();
            bubble.letterText.text = LettersHelper.GetActiveLetter();
          } else {
            if (LettersHelper.GetActiveLetterIndex()<24) {
              bubble.targetLetter = Random.Range(LettersHelper.GetActiveLetterIndex() + 2, incorrectLetters.Length - 1);
            } else {
              bubble.targetLetter = Random.Range(0, LettersHelper.GetActiveLetterIndex() - 1);
            }

            bubble.letterText.text = incorrectLetters[bubble.targetLetter];
          }
        }
      }
      if (shouldShowTutorial && bubbles.Count > 0) {
        tutorialCoroutine = StartCoroutine(TutorialCoroutine());
      }
    }
    private IEnumerator TutorialCoroutine() {
      yield return new WaitForSeconds(2f);
      bubbles[0].SetMovement(false);
      tutorialController = PopUpsManager.Instance.LoadTutorial(null);
      tutorialController.PlayTutorial(bubbles[0].transform, bubbles[0].transform, true);
      yield return new WaitForSeconds(3f);
      canClickBubble = true;
    }

    public void OnCorrect() {
      if (shouldShowTutorial) {
        shouldShowTutorial = false;
        if (tutorialController != null) {
          tutorialController.HideTutorial(() => {
            //StartCoroutine(InstantiateBubbles());
          });
        }
      }
      if (correctAnswers >= 5) {
        canInstantiate = false;
        gameMusic.Stop();
        gameCompleted = true;
        AudioController.Instance.PlayAudioAtPoint(endingSfx);
        StartCoroutine(OnGameCompleted(endingSfx.length , wrongAnswersNumber));
      }
    }

    public void InCorrect() {
      wrongAnswersNumber++;
    }

    public bool IsGameCompleted() {
      return gameCompleted;
    }

    public bool CanClickBubble() {
      return canClickBubble;
    }

    public void PlayFillEffect() {
      correctAnswers++;
      coins.transform.DOMoveY(coins.transform.position.y + 0.24f, 1);
      coinsEffect.Play();
      SceneManager.Instance.IncrementCoins(10);
      OnCorrect();
      if (tutorialCoroutine != null) {
        StopCoroutine(tutorialCoroutine);
        StartCoroutine(InstantiateBubbles());
      }
    }

    public void RemoveObjectFromList(BubbleMovement objectToRemove) {
      if (objectToRemove == null || bubbles == null)
        return;

      bubbles.Remove(objectToRemove);
      Destroy(objectToRemove);
    }

    private IEnumerator OnGameCompleted(float waitSeconds, int wrongAnswersNumber) {
      yield return new WaitForSeconds(waitSeconds);
      for (int x = 0; x < bubbles.Count; x++) {
        if (bubbles[x].targetLetter != 0) {
          bubbles[x].gameObject.SetActive(false);
        }
      }
      StorageHelper.WriteStorage(StorageHelper.MapLetterProgress + "_Game1_" + LettersHelper.GetActiveLetterIndex(), CalculateGameRating(wrongAnswersNumber).ToString());
      PopUpsManager.Instance.GetGameRatingPopUp().ShowPopUp(wrongAnswersNumber);
    }

    public int CalculateGameRating(int errorCount) {
      if (errorCount > 4) {
        return 2;
      } else {
        return 3;
      }
    }
  }
}