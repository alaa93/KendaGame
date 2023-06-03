using System.Collections;
using Spine.Unity;
using UnityEngine;

namespace KendaGame.Games {
  public class BoxesTreasureBox : MonoBehaviour {
    SkeletonAnimation skeletonAnimation;
    GameObject draggedLetter;
    bool solved;
    private void Awake() {
      skeletonAnimation = GetComponent<SkeletonAnimation>();
      //InvokeRepeating(nameof(PlayShakeAnim), Random.Range(1f, 2f), Random.Range(4f, 6f));
      float rand = Random.Range(2f, 3f);
      Invoke(nameof(PlayShakeAnim), rand);
      StartCoroutine(PlayAnim(rand));
    }

    public void PlayOpenAnim() {
      skeletonAnimation.AnimationState.SetAnimation(0, "Open", false);
    }

    public void PlayCloseAnim() {
      if (draggedLetter != null) {
        draggedLetter.gameObject.SetActive(false);
      }
      skeletonAnimation.AnimationState.SetAnimation(0, "close", false);
    }

    public void OnTriggered(GameObject letter) {
      draggedLetter = letter;
      solved = true;
      CancelInvoke();
      StopAllCoroutines();
      PlayOpenAnim();
      Invoke(nameof(PlayCloseAnim), 0.3f);
    }

    public void OnSolved() {
      solved = true;
      CancelInvoke();
      StopAllCoroutines();
      skeletonAnimation.AnimationState.SetAnimation(0, "close", false);
    }

    private void PlayShakeAnim() {
      if (!solved) {
        skeletonAnimation.AnimationState.SetAnimation(0, "shake", false);
      } else {
        skeletonAnimation.AnimationState.SetAnimation(0, "close", false);
      }
    }

    private IEnumerator PlayAnim(float startDelay) {
      yield return new WaitForSeconds(startDelay);
      while (!solved) {
        PlayShakeAnim();
        yield return new WaitForSeconds(Random.Range(3f, 5f));
      }
      skeletonAnimation.AnimationState.SetAnimation(0, "close", false);
    }

  }
}