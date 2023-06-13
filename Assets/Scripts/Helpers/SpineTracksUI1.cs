using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public class SpineTracksUI1 : MonoBehaviour {
  SkeletonGraphic skeleton;

  [SerializeField]
  List<TracksAnimations> animations = new List<TracksAnimations>();

  void Start() {
    skeleton = GetComponent<SkeletonGraphic>();
        anima();


    }
    [System.Serializable]
  public class TracksAnimations {
    [SpineAnimation] public string animationProperty;
        public bool is_loop;
        public int track;

    }


    void anima()
    {
        for (int i = 0; i < animations.Count; i++)
        {
            skeleton.AnimationState.AddAnimation(animations[i].track, animations[i].animationProperty, animations[i].is_loop,0);
        if (i==animations.Count)
        {
            anima  ();
        }
        }
    }
}
