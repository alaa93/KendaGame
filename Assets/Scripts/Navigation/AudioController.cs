using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace KendaGame.Helpers {
  public class AudioController : MonoBehaviour {
    public CommonSfx commonSfx;
    private bool canPlaySfx = true;
    private bool canPlayMusic = true;

    private Queue<AudioClip> audioQueue = new Queue<AudioClip>();
    private bool isPlaying = false;


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
      SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1) {
      //TODO stop narration and empty the queue
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

    public void PlayNarrationAtPoint (AudioClip audioClip) {
      if (audioClip!=null) {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
      }
    }

    public void AddToQueueAndPlay(AudioClip clip) {
      if (clip == null) return;
      audioQueue.Enqueue(clip);
      if (!isPlaying && audioQueue.Count == 1) {
        StartCoroutine(PlayAudioQueue());
      }
    }
    private IEnumerator PlayAudioQueue() {
      isPlaying = true;

      while (audioQueue.Count > 0) {
        AudioClip clip = audioQueue.Peek();
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        //TODO replace play clip at point with actual audio source to be able to stop it at any state of the app
        yield return new WaitForSeconds(clip.length);

        audioQueue.Dequeue();
      }
      isPlaying = false;
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