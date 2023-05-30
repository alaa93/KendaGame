using System.Collections;
using System.Collections.Generic;
using KendaGame.Helpers;
using Spine.Unity;
using UnityEngine;

namespace KendaGame.Games {
  public class BoxesTreasureBox : MonoBehaviour {
    public string letterId = "أ";
    SkeletonAnimation skeletonAnimation;
    GameObject draggedLetter;
    bool solved;
    private void Awake() {
      skeletonAnimation = GetComponent<SkeletonAnimation>();
      InvokeRepeating(nameof(PlayShakeAnim), Random.Range(1f, 2f), Random.Range(4f, 6f));
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
      CancelInvoke(nameof(PlayShakeAnim));
      PlayOpenAnim();
      Invoke(nameof(PlayCloseAnim), 0.3f);
    }

    public void OnSolved() {
      solved = true;
      CancelInvoke(nameof(PlayShakeAnim));
      skeletonAnimation.AnimationState.SetAnimation(0, "close", false);
    }

    private void PlayShakeAnim() {
      if (letterId == LettersHelper.GetActiveLetter() && !solved) {
        skeletonAnimation.AnimationState.SetAnimation(0, "shake", false);
      }
    }
  }
}