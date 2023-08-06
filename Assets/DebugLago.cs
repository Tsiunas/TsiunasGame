using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLago : DebugScript
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
        StoreManager.ObtainFruit(TypesGameElement.Fruits.Tsiuna);
        GameManager.Instance.SetGameState(GameState.InGame);
    }
}
