using KendaGame.Helpers;
using KendaGame.Navigation;
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
    [SerializeField]
    StairsGameCamera gameCamera;
    [SerializeField]
    StairsSubmarine stairsSubmarine;

    GameObject activeRock;
    int activeRockIndex = 0;
    StairsRock lastTouchedRock;

    private static StairsLevelManager _instance;
    public static StairsLevelManager Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<StairsLevelManager>();
        }
        return _instance;
      }
    }

    private void Awake() {
      if (gameCamera == null) {
        if (Camera.main != null) {
          if (Camera.main.gameObject.GetComponent<StairsGameCamera>() == null) {
            gameCamera = Camera.main.gameObject.AddComponent<StairsGameCamera>();
          } else {
            gameCamera = Camera.main.gameObject.GetComponent<StairsGameCamera>();
          }
          gameCamera.transform.position = new Vector3(2.5f, 2.6f, -10f);
        } else {
          Debug.LogError("something is wrong can't find main camera");
        }
      }
    }

    private void Start() {
      LoadLevel();
    }

    private void LoadLevel() {
      GameObject level = Instantiate(levels[LettersHelper.GetActiveLetterIndex()], null);
      stairsCharacter = level.GetComponent<StairsCharacter>();
      AssignLetters();
      gameCamera.StartFollowing(level.transform);
    }

    public void CameraStopFollowing() {
      gameCamera.StopFollowing();
    }

    public void AssignLetters() {
      levelGuide.text = LettersHelper.GetActiveLetter();
      string[] inCorrectLetters = LettersHelper.GetInCorrectLetters();
      inCorrectLetters = CollectionsUtils.ShuffleArray(inCorrectLetters);
      levelRocks[0].GetComponent<StairsRock>().InitRock(0, LettersHelper.GetActiveLetter());

      for (int x = 1; x < levelRocks.Length; x++) {
        if (x < inCorrectLetters.Length) {
          if (x % 2 != 0) {
            levelRocks[x].GetComponent<StairsRock>().InitRock(x, inCorrectLetters[x]);
          } else {
            levelRocks[x].GetComponent<StairsRock>().InitRock(x, LettersHelper.GetActiveLetter());
          }
        }
      }
    }

    public void OnRockTouched(StairsRock targetRock) {
      activeRock = targetRock.gameObject;
      activeRockIndex = targetRock.GetId();
      if (targetRock.GetLetter() != LettersHelper.GetActiveLetter()) {
        stairsCharacter.OnInCorrectRockLanded(lastTouchedRock, targetRock);
      } else {
        //targetRock.GetId() > lastTouchedRock.GetId() this to insure we don't play correct sfx unless its the next rock,
        //otherwise player is jumping to previous rock and we shouldn't play correct sfx
        if (targetRock.GetId() > 0 && (lastTouchedRock == null || targetRock.GetId() > lastTouchedRock.GetId())) {
          AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetCorrectSfx());
          SceneManager.Instance.IncrementCoins(20);
        }

        lastTouchedRock = targetRock;
        if (targetRock.GetId() == levelRocks.Length - 1) {
          stairsCharacter.PlayDanceAnim();
          gameCamera.SetEndingOffset();
          stairsSubmarine.ReachedLastStair();
        }
      }
    }

    public GameObject GetNextRock() {
      if (activeRockIndex + 1 < levelRocks.Length) {
        activeRockIndex++;
        return levelRocks[activeRockIndex];
      }
      return levelRocks[0];
    }

    public StairsRock GetActiveRock() {
      return activeRock != null ? activeRock.GetComponent<StairsRock>() : levelRocks[0].GetComponent<StairsRock>();
    }

    public int GetActiveRockIndex() {
      return activeRock != null ? activeRockIndex : 0;
    }


    public void OnRockSelected(StairsRock targetRock) {
      stairsCharacter.AnimatedJump(targetRock);
    }

    public GameObject GetActiveCharacter() {
      return stairsCharacter.gameObject;
    }

    public StairsCharacter GetActiveCharacterRef() {
      return stairsCharacter;
    }
  }
}