using UnityEngine;
using UnityEngine.Events;

namespace KendaGame.Helpers {
  public class AudioController : MonoBehaviour {
    public CommonSfx commonSfx;
    private bool canPlaySfx = true;
    private bool canPlayMusic = true;

    private static AudioController _instance;
    public static AudioController Instance {
      get {
        if (!_instance)
          _instance = GameObject.FindObjectOfType<AudioController>();

        return _instance;
      }
    }

    public void Awake() {

      canPlaySfx = bool.Parse(StorageHelper.ReadStorage(StorageHelper.ISSFXMUTED, "true"));
      canPlayMusic = bool.Parse(StorageHelper.ReadStorage(StorageHelper.ISMUSICMUTED, "true"));

    }

    public void SetSfxState(bool state) {
      canPlaySfx = state;
      StorageHelper.WriteStorage(StorageHelper.ISSFXMUTED, $"{canPlaySfx}");
      SfxStateChanged?.Invoke(canPlaySfx);
    }

    public void SetMusicState(bool state) {
      canPlayMusic = state;
      StorageHelper.WriteStorage(StorageHelper.ISMUSICMUTED, $"{canPlayMusic}");
      MusicStateChanged?.Invoke(canPlayMusic);
    }

    public bool GetSfxState() {
      return canPlaySfx;
    }

    public bool GetMusicState() {
      return canPlayMusic;
    }

    public void PlaySfxAtPoint(AudioClip audioClip) {
      if (GetSfxState()) {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
      }
    }

    public void PlaySfxAtPoint(AudioClip audioClip, float volume = 1f) {
      if (GetSfxState()) {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, volume);
      }
    }

    public void PlayAudioAtPoint(AudioClip audioClip) {
      if (GetMusicState()) {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
      }
    }

    public void PlayAudioAtPoint(AudioClip audioClip, float volume = 1f) {
      if (GetMusicState()) {
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