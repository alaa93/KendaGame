using KendaGame.Helpers;
using KendaGame.Navigation;
using KendaGame.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace KendaGame.UI
{
	public class HomeScreenController : MonoBehaviour
	{

		[SerializeField]
		UIButtonAnimator playBtn;
		[SerializeField]
		UIButtonAnimator settingsBtn;
		[SerializeField]
		UIButtonAnimator menuBtn;
		[SerializeField]
		UIButtonAnimator storeBtn;
		[SerializeField]
		NarrationSounds narrationSounds;

		public VideoPlayer videoPlayer;
		public string videoName;

		private bool canPlayPlayBtnNarration;
		public static HomeScreenController Instance;

		private void Awake()
		{
			LoadMapVideo();
			canPlayPlayBtnNarration = true;
			playBtn.AddClickListener(OnPlayBtnClicked);
			settingsBtn.AddClickListener(OnSettingsBtnClicked);
			menuBtn.AddClickListener(OnMenuButtonClicked);
			storeBtn.AddClickListener(OnStoreButtonClicked);
			if (PlayerPrefs.GetInt("GenderSelected", 0) == 0)
			{
				PopUpsManager.Instance.GetGenderPopUp().ShowPopUp();
			}
		}
		private void LoadMapVideo()
		{
			int lastLetterCompletted = int.Parse(StorageHelper.ReadStorage(StorageHelper.LastLetterCompleted, "0"));
			if (lastLetterCompletted == 0 && StorageHelper.ReadStorage(StorageHelper.CanWatchIntro, "true") == "true")
			{
				videoName = IslandVideos.FirstLandIntro;
			}
			else if (lastLetterCompletted == 8 && StorageHelper.ReadStorage(StorageHelper.CanWatchOutro, "true") == "true")
			{
				videoName = IslandVideos.FirstLandOutro;
			}
			Debug.Log(GameObject.Find("Main Camera"));
			VideoPlayer videoPlayer = GameObject.Find("Main Camera").AddComponent<VideoPlayer>();
			Debug.Log(videoPlayer);
			videoPlayer.playOnAwake = false;
			videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
			videoPlayer.url = videoName;
			videoPlayer.isLooping = true;

		}
		private void OnPlayBtnClicked()
		{
			StartCoroutine(PlayBtnCoroutine());
		}
		private IEnumerator PlayBtnCoroutine()
		{
			if (canPlayPlayBtnNarration) {
				canPlayPlayBtnNarration = false;
				AudioController.Instance.PlayNarrationAtPoint(narrationSounds.PlayBtnClip);
				yield return new WaitForSeconds(narrationSounds.PlayBtnClip.length + 0.8f);
				canPlayPlayBtnNarration = true;
				SceneManager.Instance.MoveTo(SceneManager.Scenes.Map.ToString());
			}
		}
		private void OnSettingsBtnClicked()
		{
			PopUpsManager.Instance.GetSettingsPopUp().ShowPopUp();
		}

		private void OnMenuButtonClicked()
		{
			PopUpsManager.Instance.GetDashBoardPopUp().ShowPopUp();
		}
		private void OnStoreButtonClicked()
		{
			PopUpsManager.Instance.GetStorePopUp().ShowPopUp();
		}
		[Serializable]
		public class NarrationSounds
		{
			public AudioClip PlayBtnClip;
		}
	}
}
