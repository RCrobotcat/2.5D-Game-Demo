using UnityEngine;
using Spine;
using Spine.Unity;

public class PlayerTargetController : MonoBehaviour
{

    public SkeletonAnimation skeletonAnimation;

    [SpineBone(dataField: "skeletonAnimation")]
    public string boneName;
    public Camera cam;

    [HideInInspector] public Bone bone;

    Vector3 center;
    [HideInInspector] public Vector3 targetPos;

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

        if (player.isFlip)
        {
            targetPos = center - mousePos;
            targetPos.y *= -1;
        }
        else targetPos = mousePos - center;

        bone.SetLocalPosition(targetPos);
    }
}