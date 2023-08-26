using UnityEngine;
namespace KendaGame.Helpers {
  public class LevelSelector : MonoBehaviour {
    public GameObject[] islandPrefabs;

    private void Start() {
      int activeLetter = LettersHelper.GetActiveLetterIndex();
      int islandId = LettersHelper.GetIslandId(activeLetter);

      if (islandId >= 0 && islandId < islandPrefabs.Length) {
        Instantiate(islandPrefabs[islandId]);
      }
    }
  }
}