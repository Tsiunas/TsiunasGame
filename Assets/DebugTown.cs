using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTown : DebugScript {


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
        throw new System.NotImplementedException();
    }

    internal override void ConfigurarInicio()
    {
       

    }

    internal override void ConfigurarOnEnable()
    {
        base.ConfigurarOnEnable();
        GameManager.Instance.SetGameState(GameState.InGame);
    }



    #region Enums
    #endregion

    #region Atributos y propiedades

    #endregion

    #region Eventos
    #endregion

    #region Métodos
    #endregion

    #region Mensajes Unity


    // Update is called once per frame
    void Update () {
		
	}
	#endregion
	
	
}
