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
    SkeletonGraphic stars3Animation;
    [SerializeField]
    UIButtonAnimator nextBtn, restartBtn;
    Spine.AnimationState animationState;

    private void Awake() {
      nextBtn.AddClickListener(animate: true, callBackFn: () => {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == SceneManager.Scenes.SharkGame.ToString()) {
          SceneManager.Instance.MoveTo(SceneManager.Scenes.TracingAdvanced.ToString());
        } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == SceneManager.Scenes.ColoringEngine.ToString()) {
          SceneManager.Instance.MoveTo(SceneManager.Scenes.SharkGame.ToString());
        } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == SceneManager.Scenes.TracingAdvanced.ToString() && PlayerPrefs.GetInt("Boxes_Game_Completed", 0) == 0) {
          SceneManager.Instance.MoveTo(SceneManager.Scenes.BoxesGame.ToString());
        } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == SceneManager.Scenes.TracingAdvanced.ToString() && PlayerPrefs.GetInt("Boxes_Game_Completed", 0) > 0) {
          SceneManager.Instance.MoveTo(SceneManager.Scenes.HomeScreen.ToString());
        } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == SceneManager.Scenes.BoxesGame.ToString()) {
          SceneManager.Instance.MoveTo(SceneManager.Scenes.TracingAdvanced.ToString());
        }
        Destroy(this.gameObject);
      });
      restartBtn.AddClickListener(animate: true, callBackFn: () => {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        Destroy(this.gameObject);
      });
    }

    private void OnEnable() {
      if (activeLetter != null) {
        activeLetter.text = LettersHelper.GetActiveLetter();
      }
    }

    private void PlayStarAnimation(int rating) {
      switch (rating) {
        case 0:
        case -1:
          animationState.SetAnimation(0, "0Star", false);
          break;
        case 1:
          animationState.SetAnimation(0, "1Stars_lose", false);
          break;
        case 2:
          animationState.SetAnimation(0, "2Stars", false);
          break;
        case 3:
          animationState.SetAnimation(0, "3Stars", false);
          break;
      }
    }

    public void SetAnimation(int rating) {
      stars3Animation.gameObject.SetActive(false);
      if (rating == 3) {
        stars3Animation.AnimationState.SetAnimation(0, "3Stars", false);
      } else if (rating == 2) {
        stars3Animation.AnimationState.SetAnimation(0, "2Stars", false);
      } else {
        stars3Animation.AnimationState.SetAnimation(0, "1Stars_lose", false);
      }
      stars3Animation.gameObject.SetActive(true);
    }
  }
}