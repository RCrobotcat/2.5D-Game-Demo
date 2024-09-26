using UnityEngine;
using QFramework;
using Spine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Player Basic Settings")]
    public float PlayerSpeed;
    float horizontal, vertical;
    [HideInInspector] public bool isFlip;
    [HideInInspector] public PlayerNumController playerNumController;
    public Transform lookAtPoint;

    PlayerNumController playerDataCtrl;

    [HideInInspector] public Rigidbody rb;
    SpineAnimationController spineAnimationController;
    PlayerTargetController playerTargetCtrl;

    [Header("Slashing")]
    Transform dir, forwardDir;
    [HideInInspector] public bool isSlashing;

    [Header("Projectile Settings")]
    public GameObject ProjectileParticle;
    public GameObject FireEffect;
    public float ProjectileDelayTime;
    float currentProjectileDelayTime;
    /*public Transform RightShootingTransform;
    public Transform LeftShootingTransform;*/
    public Transform FireStart;
    public float ProjectileSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spineAnimationController = GetComponent<SpineAnimationController>();

        playerDataCtrl = FindObjectOfType<PlayerNumController>();
        playerTargetCtrl = GetComponent<PlayerTargetController>();

        dir = FireStart.GetChild(1);
        forwardDir = FireStart.GetChild(2);

        playerNumController = GameObject.Find("PlayerNumCanvas").GetComponent<PlayerNumController>();
    }

    void Update()
    {
        HandleMovement();
        HandleSlash();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DamageArea"))
            playerDataCtrl.SendCommand(new PlayerHealthChangeCommand(-5));
    }

    void FixedUpdate()
    {
        if (playerNumController.isRunning)
            rb.velocity = new Vector3(horizontal, 0, vertical) * PlayerSpeed * 2f;
        else
            rb.velocity = new Vector3(horizontal, 0, vertical) * PlayerSpeed;

        HandleProjectileShoot();
    }

    void HandleMovement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (horizontal < 0)
        {
            spineAnimationController.skeletonAnimation.skeleton.ScaleX = -1;
            isFlip = true;
        }
        else if (horizontal > 0)
        {
            spineAnimationController.skeletonAnimation.skeleton.ScaleX = 1;
            isFlip = false;
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
        if (Input.GetMouseButton(0) && !isSlashing)
        {
            if (currentProjectileDelayTime > 0)
                currentProjectileDelayTime -= Time.deltaTime;
            else if (currentProjectileDelayTime <= 0)
            {
                GameObject projectile;
                // GameObject shooting;

                /* Vector3 ForwardDir = spineAnimationController.skeletonAnimation.skeleton.ScaleX == 1 ?
                     Vector3.right : Vector3.left;
                 Quaternion ForwardRot = spineAnimationController.skeletonAnimation.skeleton.ScaleX == 1 ?
                     Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);

                 Vector3 shootPos = spineAnimationController.skeletonAnimation.skeleton.ScaleX == 1 ?
                     RightShootingTransform.position : LeftShootingTransform.position;

                 shooting = Instantiate(FireEffect, shootPos + ForwardDir * 0.5f, Quaternion.identity);

                 projectile = Instantiate(ProjectileParticle, shootPos, ForwardRot);
                 projectile.GetComponent<Rigidbody>().velocity = ForwardDir * 20f;*/

                FireStart.GetChild(0).gameObject.SetActive(true);

                projectile = Instantiate(ProjectileParticle, FireStart.position, FireStart.rotation);

                Vector3 forward = dir.position - FireStart.position;

                // if the player is aiming down
                if (FireStart.transform.position.y < 0.5f)
                    forward = forwardDir.position - lookAtPoint.position;

                projectile.GetComponent<Rigidbody>().velocity = forward * ProjectileSpeed;

                spineAnimationController.AddAnimation(spineAnimationController.shoot, false, 2);

                currentProjectileDelayTime = ProjectileDelayTime;

                // Destroy(shooting, 0.3f);
                Destroy(projectile, 1f);
            }
        }
        else
        {
            FireStart.GetChild(0).gameObject.SetActive(false);
        }
    }

    void HandleSlash()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isSlashing && playerNumController.GetModel<IPlayerNumModel>().PlayerStamina.Value >= 3f)
            {
                isSlashing = true;
                spineAnimationController.AddAnimation(spineAnimationController.slash, false, 3);
                spineAnimationController.skeletonAnimation.state.Complete += (entry) =>
                {
                    spineAnimationController.AddEmptyAnim(3);
                };
            }
        }
        if (Input.GetKeyUp(KeyCode.F))
            StartCoroutine(CancelSlash());
    }

    IEnumerator CancelSlash()
    {
        yield return new WaitForSeconds(0.1f);
        if (isSlashing)
            isSlashing = false;
    }
}
