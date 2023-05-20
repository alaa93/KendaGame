using KendaGame.Helpers;
using KendaGame.Navigation;
using KendaGame.Tutorial;
using ScorpionSteps.Utils;
using UnityEngine;

namespace KendaGame.Games {
  public class RotateImageGameController : MonoBehaviour {
    [SerializeField]
    RotateImagePart[] imageParts;
    [SerializeField]
    AudioClip rotateSfx;
    [SerializeField]
    AudioSource musicLoop;

    int correctCount;
    public static RotateImageGameController Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<RotateImageGameController>();
        }
        return _instance;
      }
    }
    private static RotateImageGameController _instance;

    private void Start() {
      Invoke(nameof(InitLevel), 0.6f);
    }

    private void InitLevel() {
      bool showTutorial = PlayerPrefs.GetInt("ImagePuzzleGameTutorial", 0) == 0;
      for (int x = 0; x < imageParts.Length; x++) {
        if (showTutorial) {
          imageParts[x].SetRandomRotation(false);
        }
      }
      if (showTutorial) {
        TutorialController tutorialController = PopUpsManager.Instance.LoadTutorial(null);
        imageParts[0].SetTutorialPos(tutorialController);
      }
    }

    public AudioClip GetRotateSfx() {
      return rotateSfx;
    }

    public void OnCorrect() {
      correctCount++;
      AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetCorrectSfx());
      if (correctCount >= imageParts.Length) {
        Debug.Log("GAME COMPLETED");
        AudioUtils.FadeOutAudio(musicLoop, 0.4f);
        AudioClip endingSfx = AudioController.Instance.commonSfx.GetEndingSfx();
        AudioController.Instance.PlaySfxAtPoint(endingSfx);
        Invoke(nameof(OnGameCompleted), endingSfx.length);
      }
    }

    public void OnTutorialCompleted(TutorialController tutorialController) {
      tutorialController.HideTutorial(() => {
        for (int x = 1; x < imageParts.Length; x++) {
          imageParts[x].SetClickState(true);
        }
      });
    }

    private void OnGameCompleted() {
      PopUpsManager.Instance.LoadRatingLevel();
    }
  }
}
