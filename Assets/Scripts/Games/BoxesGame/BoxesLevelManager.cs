using KendaGame.Helpers;
using UnityEngine;

namespace KendaGame.Games {
  public class BoxesLevelManager : MonoBehaviour {

    [SerializeField]
    BoxesGameLevels[] gameLevels;

    private void Start() {
      for (int x = 0; x < gameLevels.Length; x++) {
        if (gameLevels[x].levelLetter == LettersHelper.GetActiveLetter()) {
          Instantiate(gameLevels[x].level, null, false);
          break;
        }
      }
    }

    [System.Serializable]
    private class BoxesGameLevels {
      public GameObject level;
      public string levelLetter = "أ";
    }
  }

}
