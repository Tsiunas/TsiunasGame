using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAnimations : DebugScript {

    protected override void DoOnButton()
    {
        throw new System.NotImplementedException();
    }

    protected override void DoOnEnter()
    {
        
    }

    protected override void DoOnK1()
    {
        
    }

    protected override void DoOnK2()
    {
        throw new System.NotImplementedException();
    }

    bool alerta = false;
    protected override void DoOnSpace()
    {
        AnimationsManager.Instance.AnimarPNJ("PNJ_JUAN", 1);
        AnimationsManager.Instance.AnimarPNJ("PNJA_PUB_3", 2);
    }


    internal override void ConfigurarInicio()
    {
        
    }


}
