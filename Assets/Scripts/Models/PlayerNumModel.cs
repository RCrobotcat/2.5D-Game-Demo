using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public interface IPlayerNumModel : IModel
{
    // Player Health
    BindableProperty<int> PlayerHealth { get; }

    void PlayerHealthChange(int changeVal);
}

public class PlayerNumModel : AbstractModel, IPlayerNumModel
{
    public BindableProperty<int> PlayerHealth { get; } = new BindableProperty<int>(100);

    public void PlayerHealthChange(int changeVal)
    {
        int currentHealth = PlayerHealth.Value + changeVal;
        PlayerHealth.Value = Mathf.Clamp(currentHealth, 0, 100);
    }

    protected override void OnInit() { }
}
