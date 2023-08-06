using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;
using UnityEngine;

public class DebugFarm : DebugScript
{


    protected override void DoOnButton()
    {
        throw new System.NotImplementedException();
    }

    protected override void DoOnEnter()
    {
        maamTule.EncolarMensaje("Mensaje de prueba");
    }

    protected override void DoOnK1()
    {
        TimeManager.Instance.IncreaseOneDay();
    }

    protected override void DoOnK2()
    {
        throw new System.NotImplementedException();
    }

    bool alerta = false;
    protected override void DoOnSpace()
    {
        maamTule.EstablecerAlerta((alerta=!alerta));
    }

    PNJMamaTule maamTule;
    internal override void ConfigurarInicio()
    {
        /*
        StoreManager.ObtainTool(TypesGameElement.Tools.Watering_Can);
        StoreManager.ObtainTool(TypesGameElement.Tools.Hoe);
        StoreManager.ObtainSeed(TypesGameElement.Seeds.Tsiuna, 4);
        TimeManager.Instance.SetDaysPerMinute(30);
        GameManager.Instance.SetGameState(GameState.InGame);
        maamTule = FindObjectOfType<PNJMamaTule>();
        maamTule.enabled = true;
        maamTule.GetComponent<AlertasMamaTule>().enabled = true;
        */
        k1 = KeyCode.B;
    }

}
