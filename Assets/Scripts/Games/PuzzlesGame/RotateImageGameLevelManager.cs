using KendaGame.Helpers;
using UnityEngine;

namespace KendaGame.Games {
  public class RotateImageGameLevelManager : MonoBehaviour {

    [SerializeField]
    Transform parent;
    [SerializeField]
    GameObject[] levels;
    [SerializeField]
    NarrationSounds narrationSounds;

    private void Awake() {
      Instantiate(levels[LettersHelper.GetActiveLetterIndex()], parent, false);
      AudioController.Instance.PlayNarrationAtPoint(narrationSounds.RotateImageStartClip);
    }
  }
  [System.Serializable]
  public class NarrationSounds {
    public AudioClip RotateImageStartClip;
  }
}