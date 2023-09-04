using UnityEngine;
namespace KendaGame.Helpers {
  public class LevelSelector : MonoBehaviour {
    [SerializeField] GameObject[] islandPrefabs;
    [SerializeField] NarrationSounds narrationSounds;
    private void Start() {
      LettersHelper.SetActiveLetter(18);
      int activeLetter = LettersHelper.GetActiveLetterIndex();
      int islandId = LettersHelper.GetIslandId(activeLetter);
      	  AudioController.Instance.PlayNarrationAtPoint(narrationSounds.GameStartClip[islandId]);

      if (islandId >= 0 && islandId < islandPrefabs.Length) {
        Instantiate(islandPrefabs[islandId]);
      }
    }
  }
  [System.Serializable]
  public class NarrationSounds {
    public AudioClip [] GameStartClip;
  }
}