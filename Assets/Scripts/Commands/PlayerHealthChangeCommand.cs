using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class PlayerHealthChangeCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        var playerNumModel = this.GetModel<IPlayerNumModel>();

        playerNumModel.PlayerHealth.Value -= 10;
    }
}
