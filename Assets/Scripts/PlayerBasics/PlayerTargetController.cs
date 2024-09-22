using UnityEngine;
using Spine;
using Spine.Unity;

public class PlayerTargetController : MonoBehaviour
{

    public SkeletonAnimation skeletonAnimation;

    [SpineBone(dataField: "skeletonAnimation")]
    public string boneName;
    public Camera cam;

    Bone bone;

    Vector3 center;

    PlayerController player;

    void OnValidate()
    {
        if (skeletonAnimation == null) skeletonAnimation = GetComponent<SkeletonAnimation>();
        player = GetComponent<PlayerController>();
    }

    void Start()
    {
        bone = skeletonAnimation.Skeleton.FindBone(boneName);
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        center = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        Vector3 pos;
        if (player.isFlip)
        {
            pos = center - mousePos;
            pos.y *= -1;
        }
        else pos = mousePos - center;

        bone.SetLocalPosition(pos);
    }
}