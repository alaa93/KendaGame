using UnityEngine;
using UnityEngine.Events;

namespace KendaGame.Helpers {
  public class AudioController : MonoBehaviour {

    public CommonSfx commonSfx;
    private bool sfxOnState = false;  
    private bool musicOnState = false;

    private static AudioController _instance;
    public static AudioController Instance {
      get {
        if (!_instance)
          _instance = GameObject.FindObjectOfType<AudioController>();

        return _instance;
      }
    }

    private void Awake() {
      sfxOnState = (StorageHelper.ReadStorage(StorageHelper.SfxOnState, "1") == "0"?false:true);
      musicOnState = (StorageHelper.ReadStorage(StorageHelper.MusicOnState, "1") == "0"? false:true);
      SetSfxOnState(sfxOnState);
      SetMusicOnState(musicOnState);
    }

    public void SetSfxOnState(bool state) {
      sfxOnState = state;
      StorageHelper.WriteStorage(StorageHelper.SfxOnState, (sfxOnState?"1":"0"));
      SfxStateChanged?.Invoke(sfxOnState);
    }

    public void SetMusicOnState(bool state) {
      musicOnState = state;
      StorageHelper.WriteStorage(StorageHelper.MusicOnState, (musicOnState?"1":"0"));
      MusicStateChanged?.Invoke(musicOnState);
    }

    public bool GetSfxOnState() {
      return sfxOnState;
    }

    public bool GetMusicOnState() {
      return musicOnState;
    }

    public void PlaySfxAtPoint(AudioClip audioClip) {
      if (GetSfxOnState()) {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
      }
    }

    public void PlaySfxAtPoint(AudioClip audioClip, float volume = 1f) {
      if (GetSfxOnState()) {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, volume);
      }
    }

    public void PlayAudioAtPoint(AudioClip audioClip) {
      if (GetMusicOnState()) {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
      }
    }

    public void PlayAudioAtPoint(AudioClip audioClip, float volume = 1f) {
      if (GetMusicOnState()) {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, volume);
      }
    }

    public static event UnityAction<bool> SfxStateChanged;
    public static event UnityAction<bool> MusicStateChanged;

    [System.Serializable]
    public class CommonSfx {
      public AudioClip[] clickableSfx;
      public AudioClip[] CorrectSfx;
      public AudioClip[] IncorrectSfx;
      public AudioClip[] EndingSfx;

      public AudioClip GetClickableSfx() {
        return clickableSfx[Random.Range(0, clickableSfx.Length)];
      }
      public AudioClip GetCorrectSfx() {
        return CorrectSfx[Random.Range(0, CorrectSfx.Length)];
      }

      public AudioClip GetInCorrectSfx() {
        return IncorrectSfx[Random.Range(0, IncorrectSfx.Length)];
      }
      public AudioClip GetEndingSfx() {
        return EndingSfx[Random.Range(0, EndingSfx.Length)];
      }
    }
  }
}