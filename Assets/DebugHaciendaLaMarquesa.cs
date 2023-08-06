using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHaciendaLaMarquesa : DebugScript
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
        
    }

    internal override void ConfigurarOnEnable()
    {

        PlaceFlags.Instance.RaiseFlag(PlaceFlags.INICIAR_SEGUNDO_HITO);
        Debug.Log("OnEnable");
        
    }
    

}
