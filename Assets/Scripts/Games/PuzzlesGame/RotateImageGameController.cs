using System.Threading.Tasks;
using KendaGame.Helpers;
using KendaGame.Navigation;
using KendaGame.Tutorial;
using ScorpionSteps.Utils;
using Spine.Unity;
using UnityEngine;

namespace KendaGame.Games {
  public class RotateImageGameController : MonoBehaviour {
    [SerializeField]
    RotateImagePart[] imageParts;
    [SerializeField]
    AudioClip rotateSfx;
    [SerializeField]
    AudioSource musicLoop;
    [SerializeField]
    GameObject background;
    [SerializeField]
    SkeletonGraphic endingAnim;
    [SerializeField]
    int endingAnimDuration = 2;
    TutorialController tutorialController;

    string puzzleGameTutorial = "ImagePuzzleGameTutorial";
    bool tutorialPlayed = false;

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
      bool showTutorial = PlayerPrefs.GetInt(puzzleGameTutorial, 0) == 0;
      for (int x = 0; x < imageParts.Length; x++) {
        if (showTutorial) {
          imageParts[x].SetRandomRotation(true);
        }
      }
      if (showTutorial) {
        tutorialController = PopUpsManager.Instance.LoadTutorial(null);
        imageParts[0].SetTutorialPos(tutorialController);
      }
    }

    public AudioClip GetRotateSfx() {
      return rotateSfx;
    }

    public void OnCorrect() {
      tutorialPlayed = true;
      correctCount++;
      AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetCorrectSfx());
      SceneManager.Instance.IncrementCoins(20);
      if (correctCount >= imageParts.Length) {
        OnGameCompleted();
      }
    }

    public void OnTutorialCompleted() {
      if (tutorialController != null) {
        tutorialController.HideTutorial(() => {
          //PlayerPrefs.SetInt(puzzleGameTutorial, 1);
          for (int x = 1; x < imageParts.Length; x++) {
            imageParts[x].SetClickState(true);
          }
        });
      }
    }

    private async void OnGameCompleted() {
      Debug.Log("GAME COMPLETED");
      AudioUtils.FadeOutAudio(musicLoop, 0.4f);
      AudioClip endingSfx = AudioController.Instance.commonSfx.GetEndingSfx();
      AudioController.Instance.PlaySfxAtPoint(endingSfx);
      await Task.Delay(500);
      background.SetActive(false);
      for (int x = 0; x < imageParts.Length; x++) {
        imageParts[x].gameObject.SetActive(false);
      }
      endingAnim.gameObject.SetActive(true);
      await Task.Delay(endingAnimDuration * 1000);
      PopUpsManager.Instance.GetGameRatingPopUp().ShowPopUp();
    }

    public bool IsTutorialPlayed() {
      return tutorialPlayed;
    }
    public void SetTutorialPlayed() {
      tutorialPlayed = true;
      for (int x = 0; x < imageParts.Length; x++) {
        imageParts[x].SetClickState(true);
      }
    }
  }
}
