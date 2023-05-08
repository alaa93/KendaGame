using UnityEngine;

namespace KendaGame.Games {
  public class BoxesLettersController : MonoBehaviour {
    public GameObject[] letterPrefabs;
    public Transform LettersInitalPos;
    private LetterCharacterController[] instantiatedLetters;
    int currentLetterIndex = -1;
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

  }
}
