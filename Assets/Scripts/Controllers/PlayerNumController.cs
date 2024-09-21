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

    IPlayerNumModel mModel;

    void Awake()
    {
        HealthSlider = UIBar.GetChild(0).GetComponent<Image>();
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
            this.SendCommand<PlayerHealthChangeCommand>();
    }

    void UpdateHealthBar()
    {
        float SliderPercent = (float)mModel.PlayerHealth.Value / 100;

        HealthSlider.DOFillAmount(SliderPercent, 0.3f);
    }

    public IArchitecture GetArchitecture()
    {
        return PlayerApp.Interface;
    }
}
