using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using QFramework;
using Unity.VisualScripting;

public class PlayerNumController : MonoBehaviour, IController
{
    public Transform UIHealthBar;
    Image HealthSlider;
    Text HealthText;

    public Transform UIStaminaBar;
    Image StaminaSlider;
    Text StaminaText;

    PlayerController player;

    [HideInInspector] public bool isRunning;
    public float RunStaminaCost;
    public float ChargeStaminaValue;

    IPlayerNumModel mModel;

    void Awake()
    {
        HealthSlider = UIHealthBar.GetChild(0).GetComponent<Image>();
        HealthText = UIHealthBar.GetChild(1).GetComponent<Text>();

        StaminaSlider = UIStaminaBar.GetChild(0).GetComponent<Image>();
        StaminaText = UIStaminaBar.GetChild(1).GetComponent<Text>();

        player = GameObject.Find("Player_Hero").GetComponent<PlayerController>();
    }

    void Start()
    {
        mModel = this.GetModel<IPlayerNumModel>();

        mModel.PlayerHealth.RegisterWithInitValue(health =>
        {
            UpdateHealthBar();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        mModel.PlayerStamina.RegisterWithInitValue(stamina =>
        {
            UpdateStaminaBar();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }


    void Update()
    {
        // For Testing
        if (Input.GetKeyDown(KeyCode.K))
            this.SendCommand(new PlayerHealthChangeCommand(-10));

        if (Input.GetKeyDown(KeyCode.LeftShift) && mModel.PlayerStamina.Value >= 0)
            isRunning = true;
        else if (Input.GetKeyUp(KeyCode.LeftShift) || mModel.PlayerStamina.Value == 0)
            isRunning = false;

        if (isRunning && player.rb.velocity.magnitude > 0.1f)
        {
            float cost = RunStaminaCost * Time.deltaTime * -1f;
            mModel.PlayerStaminaChange(cost);
        }
        else
        {
            float add = RunStaminaCost * Time.deltaTime;
            mModel.PlayerStaminaChange(add);
        }
    }

    void UpdateHealthBar()
    {
        float SliderPercent = (float)mModel.PlayerHealth.Value / 100;
        HealthSlider.DOFillAmount(SliderPercent, 0.3f);
        HealthText.text = mModel.PlayerHealth.Value.ToString() + "/100";
    }

    void UpdateStaminaBar()
    {
        float SliderPercent = (float)mModel.PlayerStamina.Value / 25;
        StaminaSlider.DOFillAmount(SliderPercent, 0.3f);
        int val = (int)mModel.PlayerStamina.Value;
        StaminaText.text = val.ToString() + "/25";
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
    }

    public IArchitecture GetArchitecture()
    {
        return PlayerApp.Interface;
    }
}
