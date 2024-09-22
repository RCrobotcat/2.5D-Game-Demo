using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class PlayerHealthChangeCommand : AbstractCommand
{
    int _healthChange;

    public PlayerHealthChangeCommand(int healthChange)
    {
        _healthChange = healthChange;
    }

    protected override void OnExecute()
    {
        var playerNumModel = this.GetModel<IPlayerNumModel>();

        playerNumModel.PlayerHealthChange(_healthChange);
    }
}
