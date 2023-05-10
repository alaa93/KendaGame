using System.Collections;
using KendaGame.Helpers;
using KendaGame.Navigation;
using UnityEngine;
using DG.Tweening;
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

    public Transform CorrectBoxPos;
    public GameObject allPartsObject;
    public GameObject [] letterStaticParts;
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
      for (int x = 0; x < instantiatedLetters.Length; x++) {
        instantiatedLetters[x].gameObject.SetActive(false);
      }
      allPartsObject.SetActive(true);
      AudioController.Instance.PlaySfxAtPoint(showPartsSfx);
      CorrectBoxPos.GetComponent<BoxesTreasureBox>().OnTriggered(null);
      for (int x = 0; x < letterStaticParts.Length; x++) {
        letterStaticParts[x].gameObject.SetActive(true);
        letterStaticParts[x].GetComponent<SpriteRenderer>().DOFade(0, 0f);
        letterStaticParts[x].GetComponent<SpriteRenderer>().DOFade(1, 0.6f);
      }

      for (int x = 0; x < letterAnimatedParts.Length; x++) {
        letterAnimatedParts[x].gameObject.SetActive(true);
        Vector3 scale = letterAnimatedParts[x].transform.localScale;
        Vector3 pos = letterAnimatedParts[x].transform.position;
        letterAnimatedParts[x].transform.position = CorrectBoxPos.position;
        letterAnimatedParts[x].transform.localScale = Vector3.zero;
        letterAnimatedParts[x].transform.DOScale(scale, 0.5f);
        letterAnimatedParts[x].transform.DOMove(pos, 0.7f);
      }
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
  }
}
