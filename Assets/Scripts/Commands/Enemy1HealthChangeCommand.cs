using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class Enemy1HealthChangeCommand : AbstractCommand
{
    int _healthChange;

    public Enemy1HealthChangeCommand(int healthChange)
    {
        _healthChange = healthChange;
    }

    protected override void OnExecute()
    {
        var Enemy1NumModel = this.GetModel<IEnemy1NumModel>();

        Enemy1NumModel.EnemyHealthChange(_healthChange);
    }
}
