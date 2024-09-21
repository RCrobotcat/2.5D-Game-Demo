using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class PlayerApp : Architecture<PlayerApp>
{
    protected override void Init()
    {
        this.RegisterModel<IPlayerNumModel>(new PlayerNumModel());
        this.RegisterSystem<IPlayerNumSystem>(new PlayerNumSystem());
    }
}
