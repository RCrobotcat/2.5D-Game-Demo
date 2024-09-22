using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using QFramework;

public class PlayerNumController : MonoBehaviour, IController
{
    public Transform UIBar;
    Image HealthSlider;
    Text HealthText;

    IPlayerNumModel mModel;

    void Awake()
    {
        HealthSlider = UIBar.GetChild(0).GetComponent<Image>();
        HealthText = UIBar.GetChild(1).GetComponent<Text>();
    }

    void Start()
    {
        mModel = this.GetModel<IPlayerNumModel>();

        mModel.PlayerHealth.RegisterWithInitValue(health =>
        {
            UpdateHealthBar();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            this.SendCommand(new PlayerHealthChangeCommand(-10));
    }

    void UpdateHealthBar()
    {
        float SliderPercent = (float)mModel.PlayerHealth.Value / 100;

        HealthSlider.DOFillAmount(SliderPercent, 0.3f);

        HealthText.text = mModel.PlayerHealth.Value.ToString() + "/100";
    }

    public IArchitecture GetArchitecture()
    {
        return PlayerApp.Interface;
    }
}
