using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public interface IPlayerNumModel : IModel
{
    // Player Health
    BindableProperty<int> PlayerHealth { get; }
    BindableProperty<float> PlayerStamina { get; }

    void PlayerHealthChange(int changeVal);

    void PlayerStaminaChange(float changeVal);
}

public class PlayerNumModel : AbstractModel, IPlayerNumModel
{
    public BindableProperty<int> PlayerHealth { get; } = new BindableProperty<int>(100);

    public BindableProperty<float> PlayerStamina { get; } = new BindableProperty<float>(25);

    public void PlayerHealthChange(int changeVal)
    {
        int currentHealth = PlayerHealth.Value + changeVal;
        PlayerHealth.Value = Mathf.Clamp(currentHealth, 0, 100);
    }

    public void PlayerStaminaChange(float changeVal)
    {
        float currentStamina = PlayerStamina.Value + changeVal;
        PlayerStamina.Value = Mathf.Clamp(currentStamina, 0, 25);
    }

    protected override void OnInit() { }
}
