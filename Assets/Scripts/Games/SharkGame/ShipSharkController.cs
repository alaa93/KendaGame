using UnityEngine;
using DG.Tweening;
using KendaGame.Helpers;
using Spine.Unity;

namespace KendaGame.Games {
  public class ShipSharkController : MonoBehaviour {
    [SerializeField]
    float releaseSpeed = 1.3f;
    [SerializeField]
    Transform endPosition;
    [SerializeField]
    float endPosOffset = 2f;
    [SerializeField]
    Ease releaseEase = Ease.Linear;
    [SerializeField]
    ShipWheelController shipWheelController;
    [SerializeField]
    SkeletonAnimation skeletonAnimation;
    
    private bool released;
    private Vector3 initialPosition;

    private void Awake() {
      initialPosition = this.transform.position;
    }
    private void OnMouseDown() {
      if (!released) {
        ReleaseShark();
      }
    }

    private void ReleaseShark() {
      released = true;
      this.transform.DOMoveX((endPosition.position.x + endPosOffset), releaseSpeed).SetEase(releaseEase);
      skeletonAnimation.AnimationState.SetAnimation(0, "swim", true);
    }

    public void ResetShark() {
      DOTween.Pause(this.transform);
      DOTween.Kill(this.transform);
      released = false;
      this.transform.position = initialPosition;
      this.GetComponent<Collider2D>().enabled = true;
      skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
      shipWheelController.AssignLetters();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
      ShipWheelBubble wheelBubble = collision.gameObject.GetComponent<ShipWheelBubble>();
      if (wheelBubble != null) {
        this.GetComponent<Collider2D>().enabled = false;
        shipWheelController.OnBubbleHit(wheelBubble, (gameCompleted, correctAnswer) => {
          if (!gameCompleted) {
            skeletonAnimation.AnimationState.SetAnimation(0, correctAnswer ? "swim" : "hitWrong", true);
            Invoke(nameof(ResetShark), 1f);
          }
        });
      }
    }
  }
}