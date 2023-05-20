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
    UIButtonAnimator nextBtn, restartBtn;
    public readonly float FullStarsRating = 0.8f;
    public readonly float OneStarRating = 0.5f;

    private void Awake() {
      nextBtn.AddClickListener(animate: true, callBackFn: () => {
        SceneManager.Instance.OnTracingCompleted();
        Destroy(this.gameObject);
      });
      restartBtn.AddClickListener(animate: true, callBackFn: () => {
        SceneManager.Instance.ReloadScene();
        Destroy(this.gameObject);
      });
    }

    private void OnEnable() {
      if (activeLetter != null) {
        activeLetter.text = LettersHelper.GetActiveLetter();
      }
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