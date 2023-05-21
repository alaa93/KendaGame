using UnityEngine;
using TMPro;
using KendaGame.Helpers;
using UnityEngine.UI;

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
      SceneManager.Instance.OnMapLetterClicked(id);
    }

    private void LoadLetterRating() {
      //for (int x = 0; x < rating.Length; x++) {
      //  rating[x].sprite = //eather colored star or empty star based on rating from 3
      //}
    }
  }
}