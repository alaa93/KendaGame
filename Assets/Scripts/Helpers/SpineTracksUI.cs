using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public class SpineTracksUI : MonoBehaviour {
  SkeletonGraphic skeleton;

  [SerializeField]
  List<TracksAnimations> animations = new List<TracksAnimations>();

  void Start() {
    skeleton = GetComponent<SkeletonGraphic>();

    for (int i = 0; i < animations.Count; i++) {
      skeleton.AnimationState.SetAnimation(i, animations[i].animationProperty, true);
    }
  }
  [System.Serializable]
  public class TracksAnimations {
    [SpineAnimation] public string animationProperty;
  }
}
