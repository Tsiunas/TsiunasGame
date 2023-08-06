using System.Collections;
using System.Collections.Generic;
using Tsiunas.SistemaDialogos;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugPNJActor : DebugScript {
    

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
        StoreManager.ObtainFruit(TypesGameElement.Fruits.Tsiuna);
    }

    protected override void DoOnK2()
    {
        throw new System.NotImplementedException();
    }

    protected override void DoOnSpace()
    {
        
    }

    internal override void ConfigurarInicio()
    {
        k1 = KeyCode.T;
        
    }

    internal override void ConfigurarOnEnable()
    {
        GameManager.Instance.SetGameState(GameState.InGame);
        StoreManager.ObtainFruit(TypesGameElement.Fruits.Tsiuna,5);
    }
}
