using UnityEngine;

namespace KendaGame.Helpers {
  public class SfxInstance : MonoBehaviour {
    [SerializeField]
    AudioSource audioSource;

    private void OnEnable() {
      AudioController.SfxStateChanged += AudioController_SfxStateChanged;
    }

    private void OnDisable() {
      AudioController.SfxStateChanged -= AudioController_SfxStateChanged;
    }

    private void AudioController_SfxStateChanged(bool state) {
      if (audioSource != null) {
        audioSource.mute = !state;
      }
    }
  }
}
