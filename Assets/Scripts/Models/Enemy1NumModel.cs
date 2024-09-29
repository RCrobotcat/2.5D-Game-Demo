using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class Enemy1Nums
{
    public string eID { get; set; }
    public int health { get; set; }
}

public interface IEnemy1NumModel : IModel
{
    // BindableProperty<int> EnemyHealth { get; }

    Dictionary<string, Enemy1Nums> EnemyHealthSeperate { get; set; }

    BindableProperty<int> EnemyDamage { get; }

    void EnemyHealthChange(string enemyId, int changeVal);
}

public class Enemy1NumModel : AbstractModel, IEnemy1NumModel
{
    // public BindableProperty<int> EnemyHealth { get; } = new BindableProperty<int>(200);

    public BindableProperty<int> EnemyDamage { get; } = new BindableProperty<int>(-5);

    public Dictionary<string, Enemy1Nums> EnemyHealthSeperate { get; set; } = new Dictionary<string, Enemy1Nums>();

    public void EnemyHealthChange(string enemyId, int changeVal)
    {
        int currentHealth = EnemyHealthSeperate[enemyId].health + changeVal;
        EnemyHealthSeperate[enemyId].health = Mathf.Clamp(currentHealth, 0, 50);
        // Debug.Log(enemyId + " " + EnemyHealthSeperate[enemyId].health);
    }

    protected override void OnInit()
    { }
}
