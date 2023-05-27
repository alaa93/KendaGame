using System.Threading.Tasks;
using DG.Tweening;
using KendaGame.Helpers;
using Spine.Unity;
using UnityEngine;

namespace KendaGame.Games {
  public class StairsCharacter : MonoBehaviour {
    [SerializeField]
    SkeletonAnimation skeletonAnimation;
    [SerializeField]
    Rigidbody2D rigidbody2DRef;
    [SerializeField]
    float offsetX;
    [SerializeField]
    float offsetY;
    [SerializeField]
    AudioClip jumpSfx;

    public void AnimatedJump(StairsRock targetRock) {
      float difference = Mathf.Abs(targetRock.GetId() - StairsLevelManager.Instance.GetActiveRock().GetId());
      if (difference > 2) { /// invalid Jump ignore
        OnInvalidJump();
      } else {
        rigidbody2DRef.isKinematic = true;
        skeletonAnimation.AnimationState.SetAnimation(0, "Jump", true);
        AudioController.Instance.PlaySfxAtPoint(jumpSfx);
        rigidbody2DRef.constraints = RigidbodyConstraints2D.FreezeAll;
        this.transform.DOMove(new Vector3(targetRock.transform.position.x + offsetX, targetRock.transform.position.y + (difference > 1 ? offsetY + 0.3f : offsetY), 0f), difference).OnComplete(() => {
          rigidbody2DRef.isKinematic = false;
          rigidbody2DRef.constraints = RigidbodyConstraints2D.None;
          skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
        });
      }
    }

    private async void OnInvalidJump() {
      AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetInCorrectSfx());
      skeletonAnimation.AnimationState.SetAnimation(0, "Fighting", true);
      await Task.Delay(1000);
      skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
    }

    public async void OnInCorrectRockLanded(StairsRock rollBackRock, StairsRock inCorrectRock) {
      skeletonAnimation.AnimationState.SetAnimation(0, "Fighting", true);
      await Task.Delay(1000);
      skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
      AnimatedJump(rollBackRock);
      AudioController.Instance.PlaySfxAtPoint(AudioController.Instance.commonSfx.GetInCorrectSfx());
      inCorrectRock.GetComponent<Rigidbody2D>().isKinematic = false;
    }


    public void PlaySwimmingAnim() {
      AudioController.Instance.PlaySfxAtPoint(jumpSfx);
      skeletonAnimation.AnimationState.SetAnimation(0, "Jump", true);
    }

    public void PlayDanceAnim() {
      skeletonAnimation.AnimationState.SetAnimation(0, "Dancing", true);
    }
  }
}