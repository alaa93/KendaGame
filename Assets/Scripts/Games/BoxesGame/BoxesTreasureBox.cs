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

    private void Awake() {
      skeletonAnimation = GetComponent<SkeletonAnimation>();
      InvokeRepeating(nameof(PlayShakeAnim),1f,5f);
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
      CancelInvoke(nameof(PlayShakeAnim));
      PlayOpenAnim();
      Invoke(nameof(PlayCloseAnim), 0.3f);
    }

    private void PlayShakeAnim() {
      if(letterId == LettersHelper.GetActiveLetter()) {
        skeletonAnimation.AnimationState.SetAnimation(0, "shake", false);
      }
    }
  }
}