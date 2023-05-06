using KendaGame.Helpers;
using KendaGame.Navigation;
using Spine.Unity;
using TMPro;
using UnityEngine;

namespace KendaGame.UI {
  public class LetterRatingPopUp : MonoBehaviour {
    [SerializeField]
    TextMeshProUGUI activeLetter;
    [SerializeField]
    SkeletonGraphic starsAnimation;
    [SerializeField]
    UIButtonAnimator nextBtn, RestartBtn;
    Spine.AnimationState animationState;
    

    private void Awake() {
      SkeletonDataAsset skeletonDataAsset = starsAnimation.skeletonDataAsset;
      animationState = new Spine.AnimationState(skeletonDataAsset.GetAnimationStateData());
      animationState.SetAnimation(0, "1Stars_lose", false);
      starsAnimation.skeletonDataAsset = skeletonDataAsset;

      nextBtn.AddClickListener(animate: true, callBackFn: () => {
        SceneManager.Instance.MoveTo(SceneManager.Scenes.SharkGame.ToString());
      });
    }

    private void OnEnable() {
      activeLetter.text = LettersHelper.GetActiveLetter();
      //PlayStarAnimation(LettersHelper.GetLetterRating(LettersHelper.GetActiveLetter()));
     
    }

    private void PlayStarAnimation(int rating) {
      switch (rating) {
        case 0:
        case -1:
          //starsAnimation.startingAnimation = "0Star";
          animationState.SetAnimation(0, "0Star", false);
          //skeletonAnimation.state.SetAnimation();
          break;
        case 1:
          //starsAnimation.startingAnimation = "1Stars_lose";
          animationState.SetAnimation(0, "1Stars_lose", false);
          break;
        case 2:
          //starsAnimation.startingAnimation = "2Stars";
          animationState.SetAnimation(0, "2Stars", false);
          break;
        case 3:
          //starsAnimation.startingAnimation = "3Stars";
          animationState.SetAnimation(0, "3Stars", false);
          break;
      }
    }
  }
}