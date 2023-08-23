using KendaGame.Helpers;
using UnityEngine;

public class IslandSelector : MonoBehaviour {
  public GameObject[] islandPrefabs;

  private void Start() {
    int activeLetter = LettersHelper.GetActiveLetterIndex();
    int islandId = LettersHelper.GetIslandId(activeLetter);

    if (islandId >= 0 && islandId < islandPrefabs.Length) {
      Instantiate(islandPrefabs[islandId]);
    }
  }
}
