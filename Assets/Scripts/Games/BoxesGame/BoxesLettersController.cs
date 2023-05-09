using KendaGame.Navigation;
using ScorpionSteps.Utils;
using UnityEngine;

namespace KendaGame.Games {
  public class BoxesLettersController : MonoBehaviour {

    public GameObject[] letterPrefabs;
    public Transform LettersInitalPos;
    public SpriteRenderer levelWord;
    private LetterCharacterController[] instantiatedLetters;
    int currentLetterIndex = -1;
    int correctCount;

    private void Start() {
      InitializeLetters();
    }

    private void InitializeLetters() {
      instantiatedLetters = new LetterCharacterController[letterPrefabs.Length];
      for (int x = 0; x < letterPrefabs.Length; x++) {
        instantiatedLetters[x] = Instantiate(letterPrefabs[x], null, false).GetComponent<LetterCharacterController>();
        instantiatedLetters[x].SetInitalPos(LettersInitalPos.position);
        Invoke(nameof(StartCharactersWave), 2 + x);
      }
    }

    private void StartCharactersWave() {
      currentLetterIndex++;
      if (currentLetterIndex < instantiatedLetters.Length) {
        instantiatedLetters[currentLetterIndex].SetRunState(true);
      }
    }

    public void OnCorrectLetter() {
      levelWord.material.SetFloat("_FillAmount", levelWord.material.GetFloat("_FillAmount") + 0.05f);
      correctCount++;

      if (correctCount >= 3) {
        for (int x = 0; x < instantiatedLetters.Length; x++) {
          instantiatedLetters[x].SetRunState(false);
          instantiatedLetters[x].Jump();
        }
        Invoke(nameof(GameCompleted), 2f);
      }
    }

    private void GameCompleted() {
      PopUpsManager.Instance.LoadRatingLevel();
    }

  }
}
