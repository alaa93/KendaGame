using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public class spineTracks : MonoBehaviour
{
    SkeletonAnimation skeleton;
    public bool is_AddAnimation;

    [SerializeField]
    List<TracksAnimations> animations = new List<TracksAnimations>();

    void Awake()
    {
        skeleton = GetComponent<SkeletonAnimation>();

        for (int i = 0; i < animations.Count; i++)
        {
            if (is_AddAnimation)
                 skeleton.AnimationState.AddAnimation(animations[i].track, animations[i].animationProperty, animations[i].is_loop,0);
            else
                skeleton.AnimationState.SetAnimation(i, animations[i].animationProperty, animations[i].is_loop);

        }
    }
    [System.Serializable]
    public class TracksAnimations
    {
        [SpineAnimation] public string animationProperty;
        public bool is_loop;
        public int track;
    }
}
