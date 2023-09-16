using KendaGame.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace KendaGame.Games {
  public class BoxesLevelManager : MonoBehaviour {

    [SerializeField]
    BoxesGameLevels[] gameLevels;
    [SerializeField]
    Image levelBackGround;
    [SerializeField]
    Image levelFrontGround;
    [SerializeField]
    Sprite[] islandBackGrounds;
    [SerializeField]
    Sprite[] islandFrontGrounds;
    private void Start() {
      levelBackGround.sprite=islandBackGrounds[LettersHelper.GetIslandId(LettersHelper.GetActiveLetterIndex())];
      levelFrontGround.sprite = islandFrontGrounds[LettersHelper.GetIslandId(LettersHelper.GetActiveLetterIndex())];
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
