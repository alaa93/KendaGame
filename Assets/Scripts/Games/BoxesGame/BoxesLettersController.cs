using System.Collections;
using KendaGame.Helpers;
using KendaGame.Navigation;
using UnityEngine;
using DG.Tweening;
using KendaGame.Tutorial;
using Spine.Unity;
using System.Threading.Tasks;

namespace KendaGame.Games {
  public class BoxesLettersController : MonoBehaviour {

    private static BoxesLettersController _instance;
    public static BoxesLettersController Instance {
      get {
        if (_instance == null) {
          _instance = FindObjectOfType<BoxesLettersController>();
        }
        return _instance;
      }
    }

    public BoxesTreasureBoxTrigger boxTrigger;
    public GameObject allPartsObject;
    public SkeletonAnimation endingAnim;
    public GameObject[] letterAnimatedParts;
    public GameObject[] letterPrefabs;
    public Transform LettersInitalPos;
    public AudioSource musicLoop;
    public AudioClip showPartsSfx;

    private LetterCharacterController[] instantiatedLetters;
    int currentLetterIndex = -1;
    int correctCount;
    int correctSolvedParts;
    public int totalPartsToSolve;
    private int partsPerBox;
    private int partsToSolve;
    private TutorialController tutorialController;
    string cachedLetterId;
    string tutorialPrefsKey = "BoxesGameTutorial1234r";
    public bool gameCompleted;
    private int unlockedParts;
    private int unlockChunk = 1;

    public AudioClip firstNarration;
    public AudioClip partSolvedSfx;


    private void Start() {
      InitializeLetters();
      InitGameParts();
      AudioController.Instance.AddToQueueAndPlay(firstNarration);
    }

    private void InitGameParts() {
      unlockChunk = letterAnimatedParts.Length <= 4 ? 1 : 2;
      for (int x = 0; x < letterAnimatedParts.Length; x++) {
        letterAnimatedParts[x].SetActive(false);
      }
    }

    private void InitializeLetters() {
      instantiatedLetters = new LetterCharacterController[letterPrefabs.Length];

      for (int x = 0; x < letterPrefabs.Length; x++) {
        instantiatedLetters[x] = Instantiate(letterPrefabs[x], null, false).GetComponent<LetterCharacterController>();
        instantiatedLetters[x].SetInitalPos(LettersInitalPos.position);
        if (IsTutorialSeen()) {
          if (x == 0) {
            Invoke(nameof(StartCharactersWave), 0.5f);
          } else {
            Invoke(nameof(StartCharactersWave), x + Random.Range(0.5f, 2f));
          }
        } else {
          if(x == 0) {
            Invoke(nameof(StartCharactersWave), Random.Range(1, 2));
          }
        }
      }
    }

    private void StartCharactersWave() {
      currentLetterIndex++;
      if (currentLetterIndex < instantiatedLetters.Length) {
        instantiatedLetters[currentLetterIndex].SetRunState(true);
      }
    }

    public void OnCorrectLetter(BoxesTreasureBox box) {
      if(gameCompleted) {
        return;
      }
      correctCount++;
      AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetCorrectSfx());
      SceneManager.Instance.IncrementCoins(30);
      ShowGameParts(box);

      if (correctCount >= 3) {
        gameCompleted = true;
        for (int x = 0; x < instantiatedLetters.Length; x++) {
          instantiatedLetters[x].SetRunState(false);
          instantiatedLetters[x].Jump();
        } 
      }
      if (tutorialController != null && tutorialController.IsTutorialPlaying()) {
        tutorialController.HideTutorial(() => {
          //PlayerPrefs.SetInt(tutorialPrefsKey, 1);
          OnTutorialCompleted();
        });
      }
    }

    public void OnInCorrectLetter() {
      AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetInCorrectSfx());
    }

    private IEnumerator GameCompleted(float delay) {
      PlayerPrefs.SetInt("Boxes_Game_Completed", 1);
      yield return new WaitForSeconds(delay);
      endingAnim.gameObject.SetActive(true);
      allPartsObject.SetActive(false);

      AudioClip endingSfx = AudioController.Instance.commonSfx.GetEndingSfx();
      AudioController.Instance.PlaySfxAtPoint(endingSfx,0.1f);
      AudioController.Instance.AddToQueueAndPlay(partSolvedSfx);
      if (LettersHelper.GetActiveLetterIndex() == 0) {
        endingAnim.AnimationState.AddAnimation(0, "idle", false,0);
       // yield return new WaitForSeconds(0.5f);
        endingAnim.AnimationState.AddAnimation(0, "Letters", false, 0);
        endingAnim.AnimationState.SetAnimation(0, "swimming", false);
                endingAnim.AnimationState.AddAnimation(0, "idle", false, 0);

                yield return new WaitForSeconds(6f);
      } else {
        yield return new WaitForSeconds(7f);
      }
      PopUpsManager.Instance.GetGameRatingPopUp().ShowPopUp();
    }

    private async void ShowGameParts(BoxesTreasureBox box) {
      allPartsObject.SetActive(true);
      AudioController.Instance.PlaySfxAtPoint(showPartsSfx);
      box.OnTriggered(null);
      await Task.Delay(2300);

      int counter = 0;
      for (int x = unlockedParts; x < letterAnimatedParts.Length; x++) {
        if (unlockedParts >= 4) {
          int temp = x;
          AnimateLetter(temp, box.transform.position);
        } else {
          if (counter < unlockChunk) {
            int temp = x;
            AnimateLetter(temp, box.transform.position);
            counter++;
          }
        }
      }
      unlockedParts += unlockChunk;
      unlockChunk = unlockedParts > 0 ? 2 : 1;

      if (partsToSolve >= letterAnimatedParts.Length) {
        for (int x = 0; x < instantiatedLetters.Length; x++) {
          instantiatedLetters[x].gameObject.SetActive(false);
        }
        for (int x = 0; x < letterAnimatedParts.Length; x++) {
          letterAnimatedParts[x].GetComponent<Collider2D>().enabled = true;
        }
      }
      partsToSolve += unlockedParts;
    }



    private void AnimateLetter(int index, Vector2 startPos) {
      letterAnimatedParts[index].GetComponent<Collider2D>().enabled = false;
      letterAnimatedParts[index].gameObject.SetActive(true);
      Vector3 scale = letterAnimatedParts[index].transform.localScale;
      Vector3 pos = letterAnimatedParts[index].transform.position;
      letterAnimatedParts[index].transform.position = startPos;
      letterAnimatedParts[index].transform.localScale = Vector3.zero;
      letterAnimatedParts[index].transform.DOScale(scale, 0.5f);
      letterAnimatedParts[index].transform.DOMove(pos, 0.7f);
    }

    public void OnGamePartSolved() {
      correctSolvedParts++;
      AudioClip sfx = AudioController.Instance.commonSfx.GetCorrectSfx();
      AudioController.Instance.PlaySfxAtPoint(sfx);
      if (correctSolvedParts >= totalPartsToSolve) {
        musicLoop.DOFade(0, 0.6f);
        StartCoroutine(GameCompleted(Mathf.Clamp(sfx.length, 0f, 1.5f)));
      }
    }

    public void SetLetterMoveState(bool state) {
      for (int x = 0; x < instantiatedLetters.Length; x++) {
        if (cachedLetterId != instantiatedLetters[x].GetLetterId()) {
          instantiatedLetters[x].SetFreezeState(state);
          instantiatedLetters[x].SetAnimation(state);
          instantiatedLetters[x].SetRunState(state);
        } else {
          instantiatedLetters[x].SetRunState(state);
          instantiatedLetters[x].SetDancingAnimation();
        }
      }
    }

    private void OnTutorialCompleted() {
      for (int x = 1; x < instantiatedLetters.Length; x++) {
        Invoke(nameof(ResumeWaveAfterTutorial), x + Random.Range(0.5f, 2f));
      }
    }

    private void ResumeWaveAfterTutorial() {
      currentLetterIndex++;
      if (currentLetterIndex < instantiatedLetters.Length) {
        instantiatedLetters[currentLetterIndex].SetFreezeState(false);
        instantiatedLetters[currentLetterIndex].SetAnimation(true);
        instantiatedLetters[currentLetterIndex].SetRunState(true);
      }
    }

    public void CheckPlayTutorial(Transform startPos, string letterId) {
      if (PlayerPrefs.GetInt(tutorialPrefsKey, 0) == 0 && letterId == LettersHelper.GetActiveLetter() && tutorialController == null) {
        PlayTutorial(startPos, letterId);
      }
    }

    private bool IsTutorialSeen() {
      return PlayerPrefs.GetInt(tutorialPrefsKey, 0) > 0;
    }

    private void PlayTutorial(Transform startPos, string letterId) {
      cachedLetterId = letterId;
      SetLetterMoveState(false);
      if(tutorialController == null) {
        tutorialController = PopUpsManager.Instance.LoadTutorial(null);
        if(tutorialController != null) {
          tutorialController.PlayTutorial(startPos, boxTrigger.transform);
        }
      }
    }

  }
}
