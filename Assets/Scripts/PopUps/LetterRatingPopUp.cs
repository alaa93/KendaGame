using KendaGame.Helpers;
using KendaGame.Navigation;
using Spine.Unity;
using System.Collections;
using TMPro;
using UnityEngine;

namespace KendaGame.UI
{
	public class LetterRatingPopUp : MonoBehaviour
	{
		[SerializeField]
		TextMeshProUGUI activeLetter;
		[SerializeField]
		SkeletonGraphic starsAnimation;
		[SerializeField]
		UIButtonAnimator nextBtn, restartBtn;
		[SerializeField]
		NarrationSounds narrationSounds;
		public static float FullStarsRating = 0.8f;
		public static float OneStarRating = 0.5f;

		private void Awake()
		{
			nextBtn.AddClickListener(animate: true, callBackFn: () =>
			{
				SceneManager.Instance.OnNextScreenBtnClicked();
				gameObject.SetActive(false);
			});
			restartBtn.AddClickListener(animate: true, callBackFn: () =>
			{
				SceneManager.Instance.ReloadScene();
				gameObject.SetActive(false);
			});
		}

		private void OnEnable()
		{
			if (activeLetter != null)
			{
				activeLetter.text = LettersHelper.GetActiveLetter();
			}
		}

		public void PlayStarsAnimation(int rating)
		{
			gameObject.SetActive(true);
			starsAnimation.gameObject.SetActive(false);
			switch (rating)
			{
				case 0:
				case -1:
				case 1:
					starsAnimation.AnimationState.SetAnimation(0, "1Stars_lose", false);
					break;
				case 2:
					starsAnimation.AnimationState.SetAnimation(0, "2Stars", false);
					break;
				case 3:
					starsAnimation.AnimationState.SetAnimation(0, "3Stars", false);
					break;
			}
			starsAnimation.gameObject.SetActive(true);
		}
		private IEnumerator RatingNarration(int rating)
		{
			yield return new WaitForSeconds(3.9f);
			switch (rating)
			{
				case 0:
				case -1:
				case 1:
					nextBtn.SetClickState(false);
					break;
				case 2:
					AudioClip twoStarClip = narrationSounds.GetTwoStarClip();
					AudioController.Instance.PlayNarrationAtPoint(twoStarClip);
					yield return new WaitForSeconds(twoStarClip.length + 1f);
					break;
				case 3:
					AudioClip threeStarClip = narrationSounds.GetThreeStarClip();
					AudioController.Instance.PlayNarrationAtPoint(threeStarClip);
					yield return new WaitForSeconds(threeStarClip.length + 1f);
					break;
			}
			if (rating < 2)
			{
				AudioController.Instance.PlayNarrationAtPoint(narrationSounds.GetRetryLevelClip());
			}
			else
			{
				AudioController.Instance.PlayNarrationAtPoint(narrationSounds.GetNextLevelClip());
			}
		}
		[System.Serializable]
		public class NarrationSounds
		{
			public AudioClip[] threeStarClips;
			public AudioClip[] twoStarClips;
			public AudioClip[] goNextLevelClips;
			public AudioClip[] retryLevelClips;

			public AudioClip GetThreeStarClip()
			{
				return threeStarClips[Random.Range(0, threeStarClips.Length)];
			}
			public AudioClip GetTwoStarClip()
			{
				return twoStarClips[Random.Range(0, twoStarClips.Length)];
			}
			public AudioClip GetNextLevelClip()
			{
				return goNextLevelClips[Random.Range(0, goNextLevelClips.Length)];
			}
			public AudioClip GetRetryLevelClip()
			{
				return retryLevelClips[Random.Range(0, retryLevelClips.Length)];
			}
		}

	}
}