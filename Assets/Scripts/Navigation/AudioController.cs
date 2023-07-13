using UnityEngine;
using UnityEngine.Events;
using ScorpionSteps.Utils;

namespace KendaGame.Helpers {
  public class AudioController : MonoBehaviour {
    private AudioSource audioSource;
    public CommonSfx commonSfx;
    private bool sfxState = true;
    private bool musicState = true;

    private static AudioController _instance;
    public static AudioController Instance {
      get {
        if (!_instance)
          _instance = GameObject.FindObjectOfType<AudioController>();

        return _instance;
      }
    }
    private void OnEnable() {
      UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoad;
      MusicStateChanged+= (bool state)=>{ audioSource.mute = !audioSource.mute;};
    }
    private void OnDisable() {
      UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoad;
    }
    private void Awake() {
      audioSource = gameObject.GetComponent<AudioSource>();
    }
    private void OnSceneLoad(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadMode){
      if(Navigation.SceneManager.Instance.SceneClip[Navigation.SceneManager.Instance.GetActiveScene()]!= null){
        audioSource.clip = Navigation.SceneManager.Instance.SceneClip[Navigation.SceneManager.Instance.GetActiveScene()];
      }
      else{
        audioSource.clip = null;
      }
      PlayMusic();
    }
    public void StopMusic(){
      audioSource.Stop();
    }
    public void PlayMusic(){
      audioSource.Play();
    }
    public void FadeOutAudio(float speed = 1){
      AudioUtils.FadeOutAudio(audioSource, speed);
    }
    public void SetSfxState(bool state) {
      sfxState = state;
      SfxStateChanged?.Invoke(sfxState);
    }
    public void SetMusicState(bool state) {
      musicState = state;
      MusicStateChanged?.Invoke(sfxState);
    }
    public bool GetSfxState() {
      return sfxState;
    }
    public bool GetMusicState() {
      return musicState;
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