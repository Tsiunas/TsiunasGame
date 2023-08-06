using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPersistencia : DebugScript
{
    protected override void DoOnButton()
    {
        throw new System.NotImplementedException();
    }

    protected override void DoOnEnter()
    {
        
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
        PersistenceManager.Instance.GuardarPerfil();
    }
}
