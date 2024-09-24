using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class PlayerStaminaChangeCommand : AbstractCommand
{
    int _staminaChange;

    public PlayerStaminaChangeCommand(int staminaChange)
    {
        _staminaChange = staminaChange;
    }

    protected override void OnExecute()
    {
        var playerModel = this.GetModel<PlayerNumModel>();

        playerModel.PlayerStaminaChange(_staminaChange);
    }
}
