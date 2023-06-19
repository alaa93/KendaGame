using System.Threading.Tasks;
using DG.Tweening;
using KendaGame.Helpers;
using KendaGame.Navigation;
using Spine.Unity;
using UnityEngine;

namespace KendaGame.Games {
  public class StairsSubmarine : MonoBehaviour {
    [SerializeField]
    Transform doorPoint;
    [SerializeField]
    SkeletonAnimation skeletonAnimation;
    [SerializeField]
    AudioClip openSfx, winSfx;

    public bool canEnter = false;

    private void OnMouseDown() {
      if (canEnter) {
        PlayEnterAnim();
      }
    }

    private async void PlayEnterAnim() {
      StairsLevelManager.Instance.CameraStopFollowing();
      StairsLevelManager.Instance.GetActiveCharacterRef().PlaySwimmingAnim();
      skeletonAnimation.AnimationState.SetAnimation(3, "Open", false);
      await Task.Delay(1000);
      StairsLevelManager.Instance.GetActiveCharacter().transform.DOMove(doorPoint.position, 1.5f);
      await Task.Delay(1500);
      StairsLevelManager.Instance.GetActiveCharacter().transform.DOMoveY(doorPoint.position.y + 1.5f, 1f);
      await Task.Delay(1000);
      skeletonAnimation.AnimationState.SetAnimation(3, "close", false);
      await Task.Delay(1000);
      StairsLevelManager.Instance.GetActiveCharacter().gameObject.SetActive(false);
      this.transform.DOMoveX(-30f, 6f);
      AudioController.Instance.PlaySfxAtPoint(winSfx);
      await Task.Delay(2500);
      PopUpsManager.Instance.GetGameRatingPopUp().ShowPopUp();
    }

    public void ReachedLastStair() {
      canEnter = true;
    }
  }
}
