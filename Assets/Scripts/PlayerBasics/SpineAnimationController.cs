using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Unity.VisualScripting;
using Spine;

public class SpineAnimationController : MonoBehaviour
{
    [HideInInspector] public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset walk, aim, shoot;

    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        if (skeletonAnimation == null)
        {
            return;
        }
    }

    public void AddAnimation(AnimationReferenceAsset animation, bool loop, int track)
    {
        TrackEntry trackEntry = skeletonAnimation.AnimationState.SetAnimation(track, animation, loop);
        trackEntry.MixAttachmentThreshold = 1f;
        trackEntry.SetMixDuration(0f, 0f);
        skeletonAnimation.state.AddEmptyAnimation(1, 0.5f, 0.1f);
    }

    public void DeleteAnimation(int track)
    {
        skeletonAnimation.state.ClearTrack(track);
    }

    // void AnimationComplete(Spine.TrackEntry trackEntry) { }
}
