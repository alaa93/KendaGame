using UnityEngine;
using TMPro;
using KendaGame.Helpers;
using UnityEngine.UI;
using KendaGame.UI;
using Spine.Unity;
using DG.Tweening;

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
    private int LastLetterCompleted;

    private void Awake() {
    
      letterText.text = LettersHelper.GetLetterByIndex(id);
      CheckLevel();
      LoadLetterRating();
      btn.AddClickListener(OnLetterClicked);
      LastLetterCompleted = int.Parse(StorageHelper.ReadStorage(StorageHelper.LastLetterCompleted,"0"));
    }

      private void OnLetterClicked() {
      if (id > LastLetterCompleted) {
        return;
      }
      Debug.Log("map letter index "+id);
      SceneManager.Instance.OnMapLetterClicked(id);
    }

    private void LoadLetterRating() {
      int.TryParse(StorageHelper.ReadStorage(StorageHelper.MapLetterProgress + "_" + id), out int storedRating);
      if (storedRating != -1) {
        for (int x = 0; x < storedRating; x++) {
          rating[x].sprite = MapController.Instance.coloredStar;
        }
      }
    }
        private void CheckLevel()
        {

            LastLetterCompleted = int.Parse(StorageHelper.ReadStorage(StorageHelper.LastLetterCompleted, "0"));

            if (id <= LastLetterCompleted)
            {
                _lock.AnimationState.SetAnimation(0, "unlocked", false);
            }

        }
    }
}