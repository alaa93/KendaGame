using UnityEngine;
using TMPro;
using KendaGame.Helpers;
using UnityEngine.UI;
using KendaGame.UI;

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

    private void Awake() {
      letterText.text = LettersHelper.GetLetterByIndex(id);
      LoadLetterRating();
      btn.AddClickListener(OnLetterClicked);
    }

    private void OnLetterClicked() {
      if (id >= 8) {
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

  }
}