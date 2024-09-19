using Spine.Unity;
using Spine;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Basic Settings")]
    public float PlayerSpeed;
    float horizontal, vertical;

    Rigidbody rb;
    SpineAnimationController spineAnimationController;

    [Header("Projectile Settings")]
    public GameObject ProjectileParticle;
    public GameObject FireEffect;
    public float ProjectileDelayTime;
    float currentProjectileDelayTime;
    public Transform shootingTransform;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spineAnimationController = GetComponent<SpineAnimationController>();
    }

    void Update()
    {
        HandleMovement();
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
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (horizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
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

                Vector3 ForwardDir = transform.localScale == new Vector3(1, 1, 1) ? Vector3.right : Vector3.left;

                shooting = Instantiate(FireEffect, shootingTransform.position + ForwardDir * 0.5f, Quaternion.identity);

                // Set the rotation for the projectile based on character orientation
                Quaternion ForwardRot = transform.localScale == new Vector3(1, 1, 1) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);

                projectile = Instantiate(ProjectileParticle, shootingTransform.position, ForwardRot);
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
