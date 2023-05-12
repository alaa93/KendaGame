using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

namespace KendaGame.Tutorial {
  public class TutorialController : MonoBehaviour {

    [SerializeField]
    private Transform handIcon, maskObject, maskObject2;
    [SerializeField]
    private SpriteRenderer fadeBG;
    [SerializeField]
    [Range(0, 1)]
    private float fadeInAmount = 0.7f;
    [SerializeField]
    [Range(0, 1)]
    private float fadeSpeed = 0.4f;
    bool isTutorialPlaying;
    Transform followStartPos,followEndPos;
    bool shouldFollow;
    bool startHandAnim;

    private void FadeIn(Action onComplete) {
      fadeBG.DOFade(fadeInAmount, fadeSpeed);
      StartCoroutine(OnFadeIn(fadeSpeed, () => {
        onComplete?.Invoke();
      }));
    }

    private void FadeOut(Action onComplete) {
      fadeBG.DOFade(0f, fadeSpeed);
      onComplete?.Invoke();
    }

    private IEnumerator OnFadeIn(float delay, Action onComplete) {
      yield return new WaitForSeconds(delay);
      onComplete?.Invoke();

    }

    public void PlayTutorial(Transform startPos, Transform endPos) {
      FadeIn(() => {
        isTutorialPlaying = true;
        StartCoroutine(StartTutorial(startPos, endPos));
      });
    }

    private IEnumerator StartTutorial(Transform startPos, Transform endPos) {
      yield return new WaitForSeconds(0.5f);
      this.followEndPos = endPos;
      this.followStartPos = startPos;
      maskObject.transform.DOMove(startPos.position, 0.4f);
      maskObject2.transform.DOMove(endPos.position, 0.4f);
      handIcon.transform.DOMove(startPos.position, 0.3f);
      yield return new WaitForSeconds(0.4f);
      shouldFollow = true;
      yield return new WaitForSeconds(1f);
      handIcon.transform.DOMove(followEndPos.position, 1.5f).SetLoops(-1);
    }

    private void Update() {
      if (shouldFollow) {
        maskObject.transform.position = followStartPos.position;
        maskObject2.transform.position = followEndPos.position;
      }
    }

    public void HideTutorial(Action onComplete) {
      shouldFollow = false;
      isTutorialPlaying = false;
      StopAllCoroutines();
      FadeOut(() => {
        onComplete?.Invoke();
        this.gameObject.SetActive(false);
      });
    }

    public bool IsTutorialPlaying() {
      return isTutorialPlaying;
    }
  }
}