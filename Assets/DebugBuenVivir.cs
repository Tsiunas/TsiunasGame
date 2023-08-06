using System.Collections;
using System.Collections.Generic;
using Tsiunas.Places;
using UnityEngine;

public class DebugBuenVivir : DebugScript
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
        throw new System.NotImplementedException();
    }

    internal override void ConfigurarInicio()
    {
        
    }

    internal override void ConfigurarOnEnable()
    {
        PlaceFlags.Instance.RaiseFlag(PlaceFlags.INICIAR_HITO_AGUA);
        PlaceFlags.Instance.RaiseFlag(PNJ_MARGARITA.KEY_MARGARITA);
        StoreManager.ObtainTool(TypesGameElement.Tools.Watering_Can);
        Debug.Log("Raised");
    }
}
