using KendaGame.Helpers;
using KendaGame.Navigation;
using KendaGame.UI;
using System;
using System.Collections;
using UnityEngine;

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

		private void Awake()
		{
			playBtn.AddClickListener(OnPlayBtnClicked);
			settingsBtn.AddClickListener(OnSettingsBtnClicked);
			menuBtn.AddClickListener(OnMenuButtonClicked);
			storeBtn.AddClickListener(OnStoreButtonClicked);
			if (PlayerPrefs.GetInt("GenderSelected", 0) == 0)
			{
				PopUpsManager.Instance.GetGenderPopUp().ShowPopUp();
			}
		}

		private void OnPlayBtnClicked()
		{
			StartCoroutine(PlayBtnCoroutine());
		}
		private IEnumerator PlayBtnCoroutine()
		{
			AudioController.Instance.PlayNarrationAtPoint(narrationSounds.PlayBtnClip);
			yield return new WaitForSeconds(narrationSounds.PlayBtnClip.length + 0.8f);
			SceneManager.Instance.MoveTo(SceneManager.Scenes.Map.ToString());
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
