using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public interface IEnemy1NumModel : IModel
{
    BindableProperty<int> EnemyHealth { get; }

    BindableProperty<int> EnemyDamage { get; }

    void EnemyHealthChange(int changeVal);
}

public class Enemy1NumModel : AbstractModel, IEnemy1NumModel
{
    public BindableProperty<int> EnemyHealth { get; } = new BindableProperty<int>(50);

    public BindableProperty<int> EnemyDamage { get; } = new BindableProperty<int>(10);

    public void EnemyHealthChange(int changeVal)
    {
        int currentHealth = EnemyHealth.Value + changeVal;
        EnemyHealth.Value = Mathf.Clamp(currentHealth, 0, 50);
    }

    protected override void OnInit() { }
}
