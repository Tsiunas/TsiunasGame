using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPub: DebugScript
{
    public Tsiunas.SistemaDialogos.PNJActor juan;

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
        juan.NivelAmistad = NivelesAmistad.AMISTAD_ENTABLADA;
    }
   
}
