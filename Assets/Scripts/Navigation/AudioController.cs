using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace KendaGame.Helpers {
	public class AudioController : MonoBehaviour {
		public CommonSfx commonSfx;
		private bool canPlaySfx = true;
		private bool canPlayMusic = true;
		private bool canPlayNarration = true;

		Queue<AudioClip> audioQueue = new Queue<AudioClip>();
		bool isPlaying = false;
		[SerializeField] AudioSource audioSource;
		Coroutine playCoroutine;

		private static AudioController _instance;
		public static AudioController Instance {
			get {
				if (!_instance)
					_instance = GameObject.FindObjectOfType<AudioController>();

				return _instance;
			}
		}

		private void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1) {
			//stop narration and empty the queue
			if (audioSource != null && audioSource.isPlaying) {
				audioSource.Stop();
			}
			if (playCoroutine != null) {
				StopCoroutine(playCoroutine);
			}
			audioQueue.Clear();
			isPlaying = false;
		}

		public void Awake() {
			canPlayNarration = bool.Parse(StorageHelper.ReadStorage(StorageHelper.ISNARRATIONMUTED, "true"));
			canPlaySfx = bool.Parse(StorageHelper.ReadStorage(StorageHelper.ISSFXMUTED, "true"));
			canPlayMusic = bool.Parse(StorageHelper.ReadStorage(StorageHelper.ISMUSICMUTED, "true"));
			SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
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
		public bool GetNarrationState() {
			return canPlayNarration;
		}
		public void PlayNarrationAtPoint(AudioClip audioClip) {
			if (GetNarrationState() && audioClip != null) {
				AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
			}
		}
		public void PlayNarrationAtPoint(AudioClip audioClip, float volume = 1f) {
			if (GetNarrationState() && audioClip != null) {
				AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, volume);
			}
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

		public void AddAudioToQueue(AudioClip audioClip) {
			if (audioClip != null) {
				audioQueue.Enqueue(audioClip);
			}
		}

		public void AddAudiosToQueue(List<AudioClip> audioClips) {
			foreach (var clip in audioClips) {
				if (clip != null) {
					audioQueue.Enqueue(clip);
				}
			}
		}

		public void PlayAudioFromQueue() {
			if (!isPlaying && audioQueue.Count > 0) {
				playCoroutine = StartCoroutine(PlayAudioQueue());
			}
		}

		private IEnumerator PlayAudioQueue() {
			if (audioSource != null) {
				isPlaying = true;

				while (audioQueue.Count > 0) {
					AudioClip clip = audioQueue.Peek();
					audioSource.clip = clip;
					audioSource.Play();

					yield return new WaitForSeconds(clip.length);

					audioQueue.Dequeue();
				}
				isPlaying = false;
			} else {
				Debug.Log("ERROR: Audio Controller - Audio source is null");
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