using KendaGame.Helpers;
using ScorpionSteps.Utils;
using TMPro;
using UnityEngine;

namespace KendaGame.Games {
  public class StairsLevelManager : MonoBehaviour {

    [SerializeField]
    GameObject[] levels;
    [SerializeField]
    TextMeshProUGUI levelGuide;
    [SerializeField]
    GameObject[] levelRocks;
    [SerializeField]
    StairsCharacter stairsCharacter;

    GameObject activeRock;
    int activeRockIndex = 0;

    private static StairsLevelManager _instance;
    public static StairsLevelManager Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<StairsLevelManager>();
        }
        return _instance;
      }
    }

    private void Start() {
      LoadLevel();
    }

    private void LoadLevel() {
      Instantiate(levels[LettersHelper.GetActiveLetterIndex()], null);
      AssignLetters();
    }

    public void AssignLetters() {
      levelGuide.text = LettersHelper.GetActiveLetter();
      string[] inCorrectLetters = LettersHelper.GetInCorrectLetters();
      inCorrectLetters = CollectionsUtils.ShuffleArray(inCorrectLetters);
      for (int x = 0; x < levelRocks.Length; x++) {
        if (x < inCorrectLetters.Length) {
          levelRocks[x].GetComponentInChildren<TextMeshPro>().text = inCorrectLetters[x];
        }
      }
    }

    public void OnRockTouched(GameObject rock) {
      for (int x = 0; x < levelRocks.Length; x++) {
        if(levelRocks[x] == rock) {
          activeRock = rock;
          activeRockIndex = x;
          break;
        }
      }
    }

    public GameObject GetNextRock() {
      if (activeRockIndex + 1 < levelRocks.Length) {
        return levelRocks[activeRockIndex + 1];
      }
      return levelRocks[0];
    }

    public GameObject GetActiveRock() {
      return activeRock != null ? activeRock : levelRocks[0];
    }


    public void OnRockSelected(GameObject targetRock) {
      stairsCharacter.PerformJump();
    }
  }
}