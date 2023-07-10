using System.Collections;
using System.Threading.Tasks;
using Spine.Unity;
using UnityEngine;
using DG.Tweening;

namespace KendaGame.Games {
  public class BoxesTreasureBox : MonoBehaviour {
    SkeletonAnimation skeletonAnimation;
    GameObject draggedLetter;
    bool solved;
    private void Awake() {
      skeletonAnimation = GetComponent<SkeletonAnimation>();
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
      Invoke(nameof(HideBoxLetter), 2f);
    }

    public void OnSolved(GameObject letter) {
      solved = true;
      CancelInvoke();
      StopAllCoroutines();
      OpenAndJump(letter);
    }

    private async void OpenAndJump(GameObject letter) {
      PlayOpenAnim();
      await Task.Delay(400);
      letter.transform.DOScale(0f, 0.2f);
      letter.transform.DOMove(new Vector2(transform.position.x, transform.position.y + 0.5f), 0.2f);
      await Task.Delay(300);
      skeletonAnimation.AnimationState.SetAnimation(0, "JumpCloseFade", false);
    }

    private void PlayShakeAnim() {
      if (!solved) {
        skeletonAnimation.AnimationState.SetAnimation(0, "shake", false);
      } else {
        skeletonAnimation.AnimationState.SetAnimation(0, "close", false);
      }
    }

    public void HideBoxLetter() {
      FadeBoxLetter();
    }

    private void FadeBoxLetter() {
      SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
      if (spriteRenderer != null) {
        spriteRenderer.enabled = false;
        Debug.Log("sprite renderer found and disabled");
      } else {
        Debug.Log("sprite renderer is null");
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