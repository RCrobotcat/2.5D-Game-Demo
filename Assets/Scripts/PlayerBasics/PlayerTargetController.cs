using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using TreeEditor;
using UnityEditor.ShaderKeywordFilter;

public class PlayerTargetController : MonoBehaviour
{

    public SkeletonAnimation skeletonAnimation;
    PlayerController player;

    [SpineBone(dataField: "skeletonAnimation")]
    public string boneName;
    public Camera cam;

    Bone bone;

    void OnValidate()
    {
        if (skeletonAnimation == null) skeletonAnimation = GetComponent<SkeletonAnimation>();
        player = GetComponent<PlayerController>();
    }

    void Start()
    {
        bone = skeletonAnimation.Skeleton.FindBone(boneName);
    }

    Vector3 center = new(Screen.width / 2, Screen.height / 2, 0);
    void Update()
    {
        // Vector3 screenPos = cam.WorldToScreenPoint(transform.position);
        Vector3 mousePos = Input.mousePosition;
        
        if(player.isFlip)

        {
            Vector3 pos = center - mousePos;
            pos.y = pos.y * -1;
            bone.SetLocalPosition(pos);
        }
        else
        {
            Vector3 pos = mousePos - center;
            bone.SetLocalPosition(pos);
        }
    }
}