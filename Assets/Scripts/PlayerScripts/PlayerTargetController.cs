using System.Collections;
using System.Collections.Generic;
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

    void OnValidate()
    {
        if (skeletonAnimation == null) skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    void Start()
    {
        bone = skeletonAnimation.Skeleton.FindBone(boneName);
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = cam.ScreenToWorldPoint(mousePosition);
        Vector3 skeletonSpacePoint = skeletonAnimation.transform.InverseTransformPoint(worldMousePosition);
        skeletonSpacePoint.x *= transform.localScale.x;
        skeletonSpacePoint.y *= transform.localScale.y;
        skeletonSpacePoint.z *= transform.localScale.z;
        bone.SetLocalPosition(skeletonSpacePoint);
    }
}