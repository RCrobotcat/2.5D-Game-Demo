using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using DG.Tweening;
using Spine.Unity;

public class Enemy1Controller : MonoBehaviour, IController
{
    [Header("Enemy Basics")]
    public string enemyID;
    public int EnemyHealth;

    Rigidbody rb;
    bool isDead;

    [Header("UI")]
    public Image UIHealthBar;
    Image HealthSlider;

    [Header("Movement")]
    public float EnemySpeed;
    Vector3 direction;

    [HideInInspector] public SkeletonAnimation skeletonAnim;
    public AnimationReferenceAsset walk, attack, getHit, dead;

    float horizontal, vertical;
    bool isFlip;

    PlayerController player;

    IEnemy1NumModel mModel;

    public GameObject bloodPrefab;
    public Transform bloodPos;
    GameObject blood;

    public float attackGaptime;
    float timer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        HealthSlider = UIHealthBar.transform.GetChild(0).GetComponent<Image>();

        skeletonAnim = GetComponent<SkeletonAnimation>();

        player = GameObject.Find("Player_Hero").GetComponent<PlayerController>();
    }

    void Start()
    {
        mModel = this.GetModel<IEnemy1NumModel>();

        mModel.EnemyHealthSeperate.Add(enemyID, new Enemy1Nums { eID = enemyID, health = EnemyHealth });

        this.RegisterEvent<UpdateEnemyNumsEvent>(e =>
        {
            UpdateHealthBar();
            Enemy1Dead();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        /*mModel.EnemyHealthSeperate.RegisterWithInitValue(health =>
        {
            UpdateHealthBar();
            Enemy1Dead();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);*/
    }

    void Update()
    {
        HandleMovement();

        direction = (player.transform.position - transform.position).normalized;

        if (blood != null)
            blood.transform.position = bloodPos.position;
    }

    void FixedUpdate()
    {
        if (!isDead)
            rb.velocity = direction * EnemySpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            this.SendCommand(new Enemy1HealthChangeCommand(enemyID, -5));
            blood = Instantiate(bloodPrefab, bloodPos.position, Quaternion.identity);
            skeletonAnim.state.AddAnimation(2, getHit, false, 0);
            cinemachineShake.Instance.shakingCamera(2f, 0.5f);
            Destroy(other.gameObject);
            Destroy(blood, 0.5f);
        }
        else
        {
            skeletonAnim.state.ClearTrack(2);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            skeletonAnim.state.AddAnimation(3, attack, true, 0);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            skeletonAnim.state.ClearTrack(3);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int damage = mModel.EnemyDamage.Value;
            player.GetComponent<PlayerController>().playerNumController.SendCommand(new PlayerHealthChangeCommand(damage));
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else if (collision.gameObject.CompareTag("Player") && timer <= 0)
        {
            int damage = mModel.EnemyDamage.Value;
            player.GetComponent<PlayerController>().playerNumController.SendCommand(new PlayerHealthChangeCommand(damage));
            timer = attackGaptime;
        }
    }

    void HandleMovement()
    {
        horizontal = rb.velocity.x;
        vertical = rb.velocity.z;

        if (horizontal < 0)
        {
            skeletonAnim.skeleton.ScaleX = -1;
            isFlip = true;
        }
        else if (horizontal > 0)
        {
            skeletonAnim.skeleton.ScaleX = 1;
            isFlip = false;
        }

        if (rb.velocity.magnitude > 0.1f)
        {
            skeletonAnim.state.AddAnimation(1, walk, true, 0);
        }
        else
        {
            skeletonAnim.state.ClearTrack(1);
        }
    }

    void UpdateHealthBar()
    {
        float SliderPercent = (float)mModel.EnemyHealthSeperate[enemyID].health / 50;
        HealthSlider.DOFillAmount(SliderPercent, 0.3f);
    }

    void Enemy1Dead()
    {
        if (mModel.EnemyHealthSeperate[enemyID].health <= 0)
        {
            isDead = true;
            rb.velocity = Vector3.zero;
            skeletonAnim.state.AddAnimation(4, dead, false, 0);
            Destroy(gameObject, 2.5f);
        }
        else return;
    }

    public IArchitecture GetArchitecture()
    {
        return EnemyApp.Interface;
    }
}
