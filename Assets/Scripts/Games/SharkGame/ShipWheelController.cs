using System;
using System.Collections;
using System.Collections.Generic;
using KendaGame.Helpers;
using KendaGame.Navigation;
using ScorpionSteps.Utils;
using TMPro;
using UnityEngine;

namespace KendaGame.Games {
  public class ShipWheelController : MonoBehaviour {

    [SerializeField]
    TextMeshPro wheelLetter;
    [SerializeField]
    private float rotationSpeed = 50f; // The speed of rotation in degrees per second
    [SerializeField]
    private bool canRotate;
    [SerializeField]
    AudioClip gameWin;
    [SerializeField]
    ShipSharkController shipSharkController;
    bool gameCompleted;
    private float positionOffet = 3.3f;
    List<string> selectedLetters = new List<string>();
    int correctCounter;
    ShipWheelBubble[] wheelBubbles;
    private int wrongAnswersNumber = 0;

    private void Start() {
      wheelBubbles = GameObject.FindObjectsOfType<ShipWheelBubble>();
      AssignLetters();
      SetScreenPosition();
      SceneManager.Instance.SetCoinsObjectState(true);
      SceneManager.Instance.SetSettingsBtnState(true);
      wheelLetter.text = LettersHelper.GetActiveLetter();
    }

    private void SetScreenPosition() {
      //this.transform.parent.position = new Vector3(CameraUtils.GetLeftScreenEdge().x + positionOffet, this.transform.parent.position.y, 0f);
    }

    private void Update() {
      RotateWheel();
    }

    private void RotateWheel() {
      if (canRotate) {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
      }
    }

    public void AssignLetters() {
      canRotate = true;
      int correctLetterIndex = UnityEngine.Random.Range(0, wheelBubbles.Length);
      wheelBubbles[correctLetterIndex].SetLetter(LettersHelper.GetActiveLetter());
      for (int x = 0; x < wheelBubbles.Length; x++) {
        if (x != correctLetterIndex) {
          int inCorrectIndex = GetInCorrectLetterIndex();
          if(inCorrectIndex != -1) {
            wheelBubbles[x].SetLetter(LettersHelper.GetLetterByIndex(inCorrectIndex));
          } else {
            Debug.LogError("Couldn't Get Valid Random Letter Index");
          }
        }
        wheelBubbles[x].gameObject.SetActive(true);
      }
    }

    private int GetInCorrectLetterIndex() {
      int rand = -1;
      for (int x = 0; x < LettersHelper.GetLettersCount(); x++) {
        rand = UnityEngine.Random.Range(0, LettersHelper.GetLettersCount() - 1);
        if (LettersHelper.GetLetterByIndex(rand) == LettersHelper.GetActiveLetter()) {
          continue;
        } else {
          bool found = false;
          for (int y = 0; y < selectedLetters.Count; y++) {
            if (LettersHelper.GetLetterByIndex(rand) == selectedLetters[y]) {
              found = true;
            }
          }
          if (found) {
            continue;
          } else {
            break;
          }
        }
      }
      return rand;
    }

    public void OnBubbleHit(ShipWheelBubble wheelBubble, Action<bool, bool> onComplete) {
      canRotate = false;
      if (wheelBubble.IsCorrectLetter()) {
        correctCounter++;
        AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetCorrectSfx());
        wheelBubble.PlayHitEffect(true);
        SceneManager.Instance.IncrementCoins(10);
        if (correctCounter >= 3) {
          canRotate = false;
          this.gameCompleted = true;
          AudioController.Instance.PlayAudioAtPoint(gameWin);
          shipSharkController.PlayWinAnim();
          StartCoroutine(OnGameCompleted(gameWin.length + 0.2f));
          onComplete?.Invoke(true, true);
          return;
        } else {
          onComplete?.Invoke(false, true);
        }
      } else {
        AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetInCorrectSfx());
        wheelBubble.PlayHitEffect(false);
        onComplete?.Invoke(false, false);
        wrongAnswersNumber++;
      }
    }

    private IEnumerator OnGameCompleted(float delay) {
      yield return new WaitForSeconds(delay);
      if (PopUpsManager.Instance != null) {
        StorageHelper.WriteStorage(StorageHelper.MapLetterProgress + "_Game2_" + LettersHelper.GetActiveLetterIndex(), CalculateGameRating(wrongAnswersNumber).ToString());

        PopUpsManager.Instance.GetGameRatingPopUp().ShowPopUp(wrongAnswersNumber);
      }
    }

    public bool IsGameCompleted() {
      return gameCompleted;
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