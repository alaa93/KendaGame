using System;
using System.Collections;
using System.Collections.Generic;
using KendaGame.Helpers;
using KendaGame.Navigation;
using UnityEngine;

namespace KendaGame.Games {
  public class ShipWheelController : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed = 50f; // The speed of rotation in degrees per second
    [SerializeField]
    private bool canRotate;
    [SerializeField]
    AudioClip gameWin;

    bool gameCompleted;

    List<string> selectedLetters = new List<string>();
    int correctCounter;
    ShipWheelBubble[] wheelBubbles;

    private void Start() {
      wheelBubbles = GameObject.FindObjectsOfType<ShipWheelBubble>();
      AssignLetters();
    }

    private void Update() {
      RotateWheel();
    }

    private void RotateWheel() {
      if (canRotate) {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
      }
    }

    private void AssignLetters() {
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

    public void OnBubbleHit(ShipWheelBubble wheelBubble, Action<bool> gameCompleted) {
      if (wheelBubble.IsCorrectLetter()) {
        // correct letter
        AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetCorrectSfx());
        wheelBubble.gameObject.SetActive(false);
        correctCounter++;
        if (correctCounter >= 3) {
          canRotate = false;
          this.gameCompleted = true;
          AudioController.Instance.PlayAudioAtPoint(gameWin);
          StartCoroutine(OnGameCompleted(gameWin.length + 0.2f));
          gameCompleted?.Invoke(true);
          return;
        }
        gameCompleted?.Invoke(false);
      } else {
        // in correct letter
        AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetInCorrectSfx());
        gameCompleted?.Invoke(false);
      }
      AssignLetters();
    }

    private IEnumerator OnGameCompleted(float delay) {
      yield return new WaitForSeconds(delay);
      if (PopUpsManager.Instance != null) {
        PopUpsManager.Instance.LoadRatingLevel();
      }
    }

    public bool IsGameCompleted() {
      return gameCompleted;
    }
  }
}