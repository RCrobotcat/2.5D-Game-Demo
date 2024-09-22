using Spine.Unity;
using Spine;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using QFramework;

public class PlayerController : MonoBehaviour
{
    [Header("Player Basic Settings")]
    public float PlayerSpeed;
    float horizontal, vertical;

    PlayerNumController playerDataCtrl;

    Rigidbody rb;
    SpineAnimationController spineAnimationController;

    [Header("Projectile Settings")]
    public GameObject ProjectileParticle;
    public GameObject FireEffect;
    public float ProjectileDelayTime;
    float currentProjectileDelayTime;
    public Transform RightShootingTransform;
    public Transform LeftShootingTransform;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spineAnimationController = GetComponent<SpineAnimationController>();

        playerDataCtrl = FindObjectOfType<PlayerNumController>();
    }

    void Update()
    {
        HandleMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DamageArea"))
            playerDataCtrl.SendCommand(new PlayerHealthChangeCommand(-5));
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(horizontal, 0, vertical);
        rb.velocity = move * PlayerSpeed;

        HandleProjectileShoot();
    }

    void HandleMovement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (horizontal < 0)
        {
            spineAnimationController.skeletonAnimation.skeleton.ScaleX = -1;
        }
        else if (horizontal > 0)
        {
            spineAnimationController.skeletonAnimation.skeleton.ScaleX = 1;
        }

        if (rb.velocity.magnitude > 0.1f)
        {
            spineAnimationController.skeletonAnimation.state.AddAnimation(1, spineAnimationController.walk, true, 0);
        }
        else
        {
            spineAnimationController.DeleteAnimation(1);
        }
    }

    void HandleProjectileShoot()
    {
        if (Input.GetMouseButton(0))
        {
            if (currentProjectileDelayTime > 0)
                currentProjectileDelayTime -= Time.deltaTime;
            else if (currentProjectileDelayTime <= 0)
            {
                GameObject projectile, shooting;

                Vector3 ForwardDir = spineAnimationController.skeletonAnimation.skeleton.ScaleX == 1 ?
                    Vector3.right : Vector3.left;
                Quaternion ForwardRot = spineAnimationController.skeletonAnimation.skeleton.ScaleX == 1 ?
                    Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);

                Vector3 shootPos = spineAnimationController.skeletonAnimation.skeleton.ScaleX == 1 ?
                    RightShootingTransform.position : LeftShootingTransform.position;

                shooting = Instantiate(FireEffect, shootPos + ForwardDir * 0.5f, Quaternion.identity);

                projectile = Instantiate(ProjectileParticle, shootPos, ForwardRot);
                projectile.GetComponent<Rigidbody>().velocity = ForwardDir * 20f;

                spineAnimationController.AddAnimation(spineAnimationController.shoot, false, 2);
                spineAnimationController.AddAnimation(spineAnimationController.aim, false, 3);

                currentProjectileDelayTime = ProjectileDelayTime;

                Destroy(shooting, 0.3f);
                Destroy(projectile, 1f);
            }
        }
    }
}
