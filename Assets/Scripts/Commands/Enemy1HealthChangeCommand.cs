using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class Enemy1HealthChangeCommand : AbstractCommand
{
    int _healthChange;
    string _enemyId;

    public Enemy1HealthChangeCommand(string enemyId, int healthChange)
    {
        _healthChange = healthChange;
        _enemyId = enemyId;
    }

    protected override void OnExecute()
    {
        var Enemy1NumModel = this.GetModel<IEnemy1NumModel>();
        Enemy1NumModel.EnemyHealthChange(_enemyId, _healthChange);

        this.SendEvent<UpdateEnemyNumsEvent>();
    }
}
