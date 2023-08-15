using UnityEngine;
using DG.Tweening;
using KendaGame.Helpers;
using Spine.Unity;
using ScorpionSteps.Utils;

namespace KendaGame.Games
{
	public class ShipSharkController : MonoBehaviour
	{
		[SerializeField]
		float releaseSpeed = 1.3f;
		[SerializeField]
		Transform endPosition;
		[SerializeField]
		float endPosOffset = 2f;
		[SerializeField]
		Ease releaseEase = Ease.Linear;
		[SerializeField]
		GameObject particles;
		[SerializeField]
		ShipWheelController shipWheelController;
		[SerializeField]
		SkeletonAnimation skeletonAnimation;
		[SerializeField]
		float scalRate = 0.5f;
		[SerializeField]
		NarrationSounds narrationSounds;
		float scaleCount = 1;
		float positionOffet = 1.6f;
		private bool released;
		private Vector3 initialPosition;

		private void Awake()
		{
			this.transform.position = new Vector3(CameraUtils.GetRightScreenEdge().x - positionOffet, this.transform.position.y, 0f);
			initialPosition = this.transform.position;
		}
		private void OnMouseDown()
		{
			if (!released)
			{
				ReleaseShark();
				particles.SetActive(true);
			}
		}
		private void Start()
		{
			AudioController.Instance.PlayNarrationAtPoint(narrationSounds.sharkGameStartClip);
		}
		private void ReleaseShark()
		{
			released = true;
			this.transform.DOMoveX((endPosition.position.x + endPosOffset), releaseSpeed).SetEase(releaseEase);
			skeletonAnimation.AnimationState.SetAnimation(0, "swim", true);
		}

		public void ResetShark()
		{
			particles.SetActive(false);

			DOTween.Pause(this.transform);
			DOTween.Kill(this.transform);
			released = false;
			this.transform.position = initialPosition;
			this.GetComponent<Collider2D>().enabled = true;

			skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
			shipWheelController.AssignLetters();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (!shipWheelController.IsGameCompleted())
			{
				ShipWheelBubble wheelBubble = collision.gameObject.GetComponent<ShipWheelBubble>();
				if (wheelBubble != null && released)
				{
					this.GetComponent<Collider2D>().enabled = false;
					if (wheelBubble.IsCorrectLetter())
					{
						this.transform.DOScale(transform.localScale * (1 + (scalRate * scaleCount)), 0.4f);
						scaleCount++;
						scalRate -= 0.05f;
					}
					shipWheelController.OnBubbleHit(wheelBubble, (gameCompleted, correctAnswer) =>
					{
						if (!shipWheelController.IsGameCompleted())
						{
							skeletonAnimation.AnimationState.SetAnimation(0, correctAnswer ? "swim" : "hitWrong", true);
							Invoke(nameof(ResetShark), 1f);
						}
					});
				}
			}
		}

		public void PlayWinAnim()
		{
			skeletonAnimation.AnimationState.SetAnimation(0, "win", false);
		}
		[System.Serializable]
		public class NarrationSounds
		{
			public AudioClip sharkGameStartClip;
		}
    }
}