using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTiempo : DebugScript
{
    protected override void DoOnButton()
    {
        throw new System.NotImplementedException();
    }

    protected override void DoOnEnter()
    {
        HarmonyFlamesManager.Instance.IncreaseHarmonyFlamesLevel(1);
    }

    protected override void DoOnK1()
    {
        throw new System.NotImplementedException();
    }

    protected override void DoOnK2()
    {
        throw new System.NotImplementedException();
    }

    protected override void DoOnSpace()
    {
        GameManager.Instance.SetGameState(GameState.InGame);
        TimeManager.Instance.SetCurrentTime(new GameTime(0,0,29,0));
        Debug.Log("Tiempo cambiado a 29 días!");

    }
}
