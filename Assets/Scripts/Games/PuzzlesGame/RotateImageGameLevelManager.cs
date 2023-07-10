using KendaGame.Helpers;
using UnityEngine;

namespace KendaGame.Games {
  public class RotateImageGameLevelManager : MonoBehaviour {

    [SerializeField]
    Transform parent;
    [SerializeField]
    GameObject[] levels;

    private void Awake() {
      Instantiate(levels[LettersHelper.GetActiveLetterIndex()], parent, false);
    }
  }
}