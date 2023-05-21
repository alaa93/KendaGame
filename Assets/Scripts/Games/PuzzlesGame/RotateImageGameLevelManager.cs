using KendaGame.Helpers;
using UnityEngine;

namespace KendaGame.Games {
  public class RotateImageGameLevelManager : MonoBehaviour {

    [SerializeField]
    GameObject [] levels;

    private void Awake() {
      GameObject level = Instantiate(levels[LettersHelper.GetActiveLetterIndex()], GetComponentInParent<Canvas>().transform, false);
    }
  }
}