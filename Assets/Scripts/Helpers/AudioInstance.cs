using UnityEngine;

namespace KendaGame.Helpers {
  public class AudioInstance : MonoBehaviour {
    [SerializeField]
    AudioSource audioSource;

    private void OnEnable() {
      AudioController.MusicStateChanged += AudioController_MusicStateChanged;
    }

    private void OnDisable() {
      AudioController.MusicStateChanged -= AudioController_MusicStateChanged;
    }

    private void AudioController_MusicStateChanged(bool state) {
      if (audioSource != null) {
        Debug.Log("music state changed "+state);
        audioSource.mute = state;
      }
    }
  }
}
