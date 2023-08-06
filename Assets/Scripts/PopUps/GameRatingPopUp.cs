using KendaGame.Helpers;
using KendaGame.Navigation;
using Spine.Unity;
using UnityEngine;

namespace KendaGame.UI {
  public class GameRatingPopUp : MonoBehaviour {

    [SerializeField]
    SkeletonGraphic starsAnimation;
    [SerializeField]
    UIButtonAnimator nextBtn, restartBtn;
    public readonly float FullStarsRating = 0.8f;
    public readonly float OneStarRating = 0.5f;

    private void Awake() {
      nextBtn.AddClickListener(animate: true, callBackFn: () => {
        SceneManager.Instance.OnNextScreenBtnClicked();
        gameObject.SetActive(false);
      });
      restartBtn.AddClickListener(animate: true, callBackFn: () => {
        SceneManager.Instance.ReloadScene();
        gameObject.SetActive(false);
      });
    }

    public void ShowPopUp(int rating) {
        
      gameObject.SetActive(true);
      if (rating < 4) PlayStarsAnimation(3);
      else PlayStarsAnimation(2);
    }
    public void ShowPopUp() {
      gameObject.SetActive(true);
      PlayStarsAnimation(3);
    }

    public void PlayStarsAnimation(int rating) {
      starsAnimation.gameObject.SetActive(false);
      switch (rating) {
        case 0:
        case -1:
        case 1:
          starsAnimation.AnimationState.SetAnimation(0, "1Stars_lose", false);
          break;
        case 2:
          starsAnimation.AnimationState.SetAnimation(0, "2Stars", false);
          break;
        case 3:
          starsAnimation.AnimationState.SetAnimation(0, "3Stars", false);
          break;
      }
      starsAnimation.gameObject.SetActive(true);
    }

    public void SetAnimation(int rating) {
      starsAnimation.gameObject.SetActive(false);
      if (rating == 3) {
        starsAnimation.AnimationState.SetAnimation(0, "3Stars", false);
      } else if (rating == 2) {
        starsAnimation.AnimationState.SetAnimation(0, "2Stars", false);
      } else {
        starsAnimation.AnimationState.SetAnimation(0, "1Stars_lose", false);
      }
      starsAnimation.gameObject.SetActive(true);
    }
  }
}
