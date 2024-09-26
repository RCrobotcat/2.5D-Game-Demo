using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class EnemyApp : Architecture<EnemyApp>
{
    protected override void Init()
    {
        this.RegisterModel<IEnemy1NumModel>(new Enemy1NumModel());
        this.RegisterSystem<IEnemy1NumSystem>(new Enemy1NumSystem());
    }
}
