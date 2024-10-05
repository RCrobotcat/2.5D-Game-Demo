using UnityEngine;
using Spine.Unity;
using Spine;
using QFramework;

public class SpineAnimationController : MonoBehaviour
{
    [HideInInspector] public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset walk, shoot, slash, getHit, death;

    PlayerController player;

    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        if (skeletonAnimation == null)
        {
            return;
        }
        player = GetComponent<PlayerController>();
    }

    public void AddAnimation(AnimationReferenceAsset animation, bool loop, int track)
    {
        TrackEntry trackEntry = skeletonAnimation.AnimationState.SetAnimation(track, animation, loop);
        trackEntry.MixAttachmentThreshold = 1f;
        trackEntry.SetMixDuration(0f, 0f);
        if (animation == slash)
            player.playerNumController.SendCommand(new PlayerStaminaChangeCommand(-3f));
        // skeletonAnimation.state.AddEmptyAnimation(1, 0.5f, 0.1f);
    }

    public void AddEmptyAnim(int track)
    {
        skeletonAnimation.state.AddEmptyAnimation(track, 0.3f, 0.1f);
    }

    public void DeleteAnimation(int track)
    {
        skeletonAnimation.state.ClearTrack(track);
    }

    // void AnimationComplete(Spine.TrackEntry trackEntry) { }
}
