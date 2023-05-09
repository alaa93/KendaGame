using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

namespace KendaGame.Games {
  public class BoxesTreasureBox : MonoBehaviour {
    public string letterId = "أ";
    SkeletonAnimation skeletonAnimation;
    GameObject draggedLetter;

    private void Awake() {
      skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    public void PlayOpenAnim() {
      skeletonAnimation.AnimationState.SetAnimation(0, "Open", false);
    }

    public void PlayCloseAnim() {
      draggedLetter.gameObject.SetActive(false);
      skeletonAnimation.AnimationState.SetAnimation(0, "close", false);
    }

    public void OnTriggered(GameObject letter) {
      draggedLetter = letter;
      PlayOpenAnim();
      Invoke(nameof(PlayCloseAnim), 0.3f);
      //skeletonAnimation.AnimationState.Event += AnimationState_Event;
    }

    private void AnimationState_Event(Spine.TrackEntry trackEntry, Spine.Event e) {
      ResetAnim();
    }

    private void ResetAnim() {
      Invoke(nameof(PlayCloseAnim), 1f);
    }
  }
}