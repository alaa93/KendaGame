using System.Collections;
using KendaGame.Helpers;
using KendaGame.Navigation;
using UnityEngine;
using DG.Tweening;
using KendaGame.Tutorial;
using Spine.Unity;

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
    public Transform CorrectBoxPos;
    public GameObject allPartsObject;
    public GameObject Box;
    public GameObject octopusAnimated;

        public GameObject[] letterStaticParts;
    public GameObject[] letterAnimatedParts;
    public GameObject[] letterPrefabs;
    public Transform LettersInitalPos;
    public SpriteRenderer levelWord;
    public AudioSource musicLoop;
    public AudioClip showPartsSfx;

    private LetterCharacterController[] instantiatedLetters;
    int currentLetterIndex = -1;
    int correctCount;
    int correctSolvedParts;
    public int totalPartsToSolve;
    private TutorialController tutorialController;
    string cachedLetterId;
    string tutorialPrefsKey = "BoxesGameTutorial123455";
    private void Start() {
      InitializeLetters();
    }

    private void InitializeLetters() {
      instantiatedLetters = new LetterCharacterController[letterPrefabs.Length];

      for (int x = 0; x < letterPrefabs.Length; x++) {
        instantiatedLetters[x] = Instantiate(letterPrefabs[x], null, false).GetComponent<LetterCharacterController>();
        instantiatedLetters[x].SetInitalPos(LettersInitalPos.position);
        if (IsTutorialSeen()) {
          Invoke(nameof(StartCharactersWave), Random.Range(1, 2));
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

    private void ContinueWaveLoad() {
      for (int x = 1; x < instantiatedLetters.Length; x++) {
        Invoke(nameof(StartCharactersWave), Random.Range(1, 2));
      }
    }

    public void OnCorrectLetter() {
      levelWord.material.SetFloat("_FillAmount", levelWord.material.GetFloat("_FillAmount") + 0.3f);
      correctCount++;
      AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetCorrectSfx());
      if (correctCount >= 3) {
        for (int x = 0; x < instantiatedLetters.Length; x++) {
          instantiatedLetters[x].SetRunState(false);
          instantiatedLetters[x].Jump();
        }
        Invoke(nameof(ShowGameParts), 1f);
      }
      if (tutorialController != null && tutorialController.IsTutorialPlaying()) {
        tutorialController.HideTutorial(() => {
          PlayerPrefs.SetInt(tutorialPrefsKey, 1);
          OnTutorialCompleted();
          ContinueWaveLoad();
        });
      }
    }

    public void OnInCorrectLetter() {
      AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetInCorrectSfx());
    }

    private IEnumerator GameCompleted(float delay) {
      yield return new WaitForSeconds(delay);
      AudioClip sfx = AudioController.Instance.commonSfx.GetEndingSfx();
      AudioController.Instance.PlaySfxAtPoint(sfx);
      yield return new WaitForSeconds(sfx.length);
      PopUpsManager.Instance.LoadRatingLevel();
    }

    private void ShowGameParts() {
      boxTrigger.gameObject.SetActive(false);
      for (int x = 0; x < instantiatedLetters.Length; x++) {
        instantiatedLetters[x].gameObject.SetActive(false);
      }
      allPartsObject.SetActive(true);
         //   Box.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "faded", false);

            AudioController.Instance.PlaySfxAtPoint(showPartsSfx);
      CorrectBoxPos.GetComponent<BoxesTreasureBox>().OnTriggered(null);
      for (int x = 0; x < letterStaticParts.Length; x++) {
        letterStaticParts[x].gameObject.SetActive(true);
        letterStaticParts[x].GetComponent<SpriteRenderer>().DOFade(0, 0f);
        letterStaticParts[x].GetComponent<SpriteRenderer>().DOFade(1, 0.6f);
      }

      for (int x = 0; x < letterAnimatedParts.Length; x++) {
        int temp = x;
        OnLetterAnimationCompleted(temp);
      }
    }

    private void OnLetterAnimationCompleted(int index) {
      letterAnimatedParts[index].GetComponent<Collider2D>().enabled = false;
      letterAnimatedParts[index].gameObject.SetActive(true);
      Vector3 scale = letterAnimatedParts[index].transform.localScale;
      Vector3 pos = letterAnimatedParts[index].transform.position;
      letterAnimatedParts[index].transform.position = CorrectBoxPos.position;
      letterAnimatedParts[index].transform.localScale = Vector3.zero;
      letterAnimatedParts[index].transform.DOScale(scale, 0.5f);
      letterAnimatedParts[index].transform.DOMove(pos, 0.7f).OnComplete(() => {
        letterAnimatedParts[index].GetComponent<Collider2D>().enabled = true;
      });
    }

    public void OnGamePartSolved() {
          


            correctSolvedParts++;
      AudioClip sfx = AudioController.Instance.commonSfx.GetCorrectSfx();
      AudioController.Instance.PlaySfxAtPoint(sfx);
      if (correctSolvedParts >= totalPartsToSolve) {
                if (octopusAnimated != null)
                {
                    octopusAnimated.SetActive(true);
                    allPartsObject.SetActive(false);
                }
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
      for (int x = 0; x < instantiatedLetters.Length; x++) {
        instantiatedLetters[x].SetFreezeState(false);
        instantiatedLetters[x].SetAnimation(true);
        instantiatedLetters[x].SetRunState(true);
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
