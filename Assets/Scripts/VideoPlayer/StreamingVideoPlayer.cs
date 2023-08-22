using KendaGame.Helpers;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace KendaGame.UI
{
	public class StreamingVideoPlayer : MonoBehaviour
	{

		[SerializeField]
		private AudioSource backGroundMusic;
		[SerializeField]
		ScrollRect map;
		[SerializeField]
		NarrationSounds narrationSounds;

		private VideoPlayer videoPlayer;

		public static StreamingVideoPlayer instance;
		private void Awake()
		{
			videoPlayer = GameObject.Find("Main Camera").GetComponent<VideoPlayer>();
			videoPlayer.loopPointReached += (videoplayer)=>{ EndReached(videoPlayer); };
		}
		void Start()
		{
			int lastLetterCompletted = int.Parse(StorageHelper.ReadStorage(StorageHelper.LastLetterCompleted, "0"));
			if ((lastLetterCompletted == 0 && StorageHelper.ReadStorage(StorageHelper.CanWatchIntro, "true") == "true") 
				|| (lastLetterCompletted == 8 && StorageHelper.ReadStorage(StorageHelper.CanWatchOutro, "true") == "true"))
			{
				StartVideo();
			}
			else
			{
				AudioController.Instance.PlayNarrationAtPoint(narrationSounds.mapLoadedClip);
			}
		}

		void StartVideo()
		{
			backGroundMusic.Pause();
			map.enabled = false;
			videoPlayer.Play();
		}

		public void EndReached(VideoPlayer vp)
		{
			if (HomeScreenController.Instance.videoName == IslandVideos.FirstLandIntro)
			{
				StorageHelper.WriteStorage(StorageHelper.CanWatchIntro, "false");
			}
			else
			{
				StorageHelper.WriteStorage(StorageHelper.CanWatchOutro, "false");
			}
			Destroy(vp);
			AudioController.Instance.PlayNarrationAtPoint(narrationSounds.afterIntroClip);
			backGroundMusic.Play();
			map.enabled = true;
		}
		[Serializable]
		public class NarrationSounds
		{
			public AudioClip afterIntroClip;
			public AudioClip mapLoadedClip;
		}
	}
}
