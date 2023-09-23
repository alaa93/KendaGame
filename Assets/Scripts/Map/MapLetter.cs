using UnityEngine;
using TMPro;
using KendaGame.Helpers;
using UnityEngine.UI;
using KendaGame.UI;
using Spine.Unity;
using DG.Tweening;
using static KendaGame.Models.TracingModels;
using static KendaGame.Models.UserModels;

namespace KendaGame.Navigation {
  [RequireComponent(typeof(UIButtonAnimator))]
  public class MapLetter : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI letterText;
    [SerializeField]
    UIButtonAnimator btn;
    [SerializeField]
    Image[] rating;
    [SerializeField]
    [Range(0, 28)]
    int id;
    [SerializeField]
    private SkeletonGraphic _lock;

    private void Awake() {
      letterText.text = LettersHelper.GetLetterByIndex(id);
      CheckLevel();
      LoadLetterRating();
      btn.AddClickListener(OnLetterClicked);
    }

    private void OnLetterClicked() {
      if (LettersHelper.CheckIfLetterOpen(LettersHelper.GetLetterByIndexClass(id)) == "Closed") {
        return;
      }
      Debug.Log("map letter index " + id);
      SceneManager.Instance.OnMapLetterClicked(id);
    }

    private void LoadLetterRating() {
      LetterProgress letterProgress = ProgressHelper.FetchProgress(id);
      int storedRating = (letterProgress.tracing_advanced_completed + letterProgress.tracing_basic_completed) / 2;
      if (storedRating != -1) {
        for (int x = 0; x < storedRating; x++) {
          rating[x].sprite = MapController.Instance.coloredStar;
        }
      }
    }
    private void CheckLevel() {
      if (LettersHelper.CheckIfLetterOpen(LettersHelper.GetLetterByIndexClass(id)) == "Closed") {
        return;
      }
      _lock.AnimationState.SetAnimation(0, "unlocked", false);
    }
  }
}