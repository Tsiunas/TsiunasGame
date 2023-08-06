using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFinalJuego : DebugScript
{
    protected override void DoOnButton()
    {
        throw new System.NotImplementedException();
    }

    protected override void DoOnEnter()
    {
        throw new System.NotImplementedException();
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
        //TimeManager.Instance.SetCurrentTime(new GameTime(0, 0, 30, 0));
    }

    internal override void ConfigurarInicio()
    {
        //TimeManager.Instance.SetCurrentTime(new GameTime(0, 0, 30, 0));
    }

    internal override void ConfigurarOnEnable()
    {
        GameManager.Instance.SetGameState(GameState.Finished);
        //TimeManager.Instance.SetCurrentTime(new GameTime(0, 0, 30, 0));
        HarmonyFlamesManager.Instance.IncreaseHarmonyFlamesLevel(9);
    }
}
