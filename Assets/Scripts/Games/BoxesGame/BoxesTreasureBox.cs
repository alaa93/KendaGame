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
      if (draggedLetter != null) {
        draggedLetter.gameObject.SetActive(false);
      }
      skeletonAnimation.AnimationState.SetAnimation(0, "close", false);
    }

    public void OnTriggered(GameObject letter) {
      draggedLetter = letter;
      PlayOpenAnim();
      Invoke(nameof(PlayCloseAnim), 0.3f);
    }
  }
}