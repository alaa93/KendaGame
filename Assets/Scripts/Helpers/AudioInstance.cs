using UnityEngine;

namespace KendaGame.Helpers {
  public class AudioInstance : MonoBehaviour {
    [SerializeField]
    AudioSource audioSource;

    private bool isMuted;

    private void OnEnable() {
      AudioController.MusicStateChanged += AudioController_MusicStateChanged;
    }

    private void OnDisable() {
      AudioController.MusicStateChanged -= AudioController_MusicStateChanged;
    }
    private void Start() {
      isMuted =!AudioController.Instance.GetMusicState();
      CheckMusicState(isMuted);
    }
    private void CheckMusicState(bool state){
      if(audioSource!= null){
        audioSource.mute = state;
      }
    }
    private void AudioController_MusicStateChanged(bool state) {
      if(isMuted){
        audioSource.mute = false;
        isMuted = false;
      }
      else{
        audioSource.mute = true;
        isMuted = true;
      }
    }
  }
}
