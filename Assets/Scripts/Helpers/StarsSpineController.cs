using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class StarsSpineController : MonoBehaviour {


  //Spine.EventData eventData;

  AudioSource audioPlayer;
  SkeletonGraphic skeletonGraphic;

  [SerializeField]
  bool NoCharactersAttached;

  [SerializeField]
  SkeletonGraphic Yaqoot, Kenda, Habar;

  [SerializeField]
  AudioClip OneStarSFX, TwoStarsSFX, ThreeStarsSFX, VectorySFX, TotalVectorySFX, LoseSFX;

  [SerializeField]
  GameObject StarsParticle, ConfettiParticle;

  [SerializeField]
  Transform star1Position, star2Position, star3Position, particlesLeftPosition, ConfettiRightPosition;


  // Start is called before the first frame update
  void Start() {
    audioPlayer = GetComponent<AudioSource>();
    skeletonGraphic = GetComponent<SkeletonGraphic>();
    skeletonGraphic.AnimationState.Event += HandleEvent;
  }

  void HandleEvent(TrackEntry trackEntry, Spine.Event e) {

    if (e.ToString() == "0") {
      audioPlayer.PlayOneShot(LoseSFX);
      SadAnimation();
    }

    if (e.ToString() == "1") {
      audioPlayer.PlayOneShot(OneStarSFX);
      Instantiate(StarsParticle, star1Position);
    }

    if (e.ToString() == "2") {
      audioPlayer.PlayOneShot(TwoStarsSFX);
      Instantiate(StarsParticle, star2Position);
    }

    if (e.ToString() == "3") {
      audioPlayer.PlayOneShot(ThreeStarsSFX);
      Instantiate(StarsParticle, star3Position);
      HappyAnimations();
    }

    if (e.ToString() == "TotalWin") {
      audioPlayer.PlayOneShot(TotalVectorySFX);
      Instantiate(ConfettiParticle, particlesLeftPosition);
      Instantiate(ConfettiParticle, ConfettiRightPosition);
    }

    if (e.ToString() == "Win") {
      audioPlayer.PlayOneShot(VectorySFX);
      Instantiate(ConfettiParticle, particlesLeftPosition);
      Instantiate(ConfettiParticle, ConfettiRightPosition);
    }

    if (e.ToString() == "Lose") {
      SadAnimation();
      audioPlayer.PlayOneShot(LoseSFX);
    }
  }


  void HappyAnimations() {
    if (!NoCharactersAttached) {
      Yaqoot.AnimationState.SetAnimation(0, "HappySmallJump", true);
      Kenda.AnimationState.SetAnimation(0, "HappySmallJump", true);
      Habar.AnimationState.SetAnimation(0, "_Idle_bouncy", true);
    }
  }


  void SadAnimation() {
    if (!NoCharactersAttached) {
      Yaqoot.AnimationState.SetAnimation(0, "Sad", true);
      Kenda.AnimationState.SetAnimation(0, "Sad", true);
      Habar.AnimationState.SetAnimation(0, "sad", true);
    }
  }
}
