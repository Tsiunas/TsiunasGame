using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;
using UnityEngine;

public class DebugEduardo : DebugScript
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
        HungerManager.Instance.DecreaseHungerLevel();
    }

    protected override void DoOnK2()
    {
        throw new System.NotImplementedException();
    }

    bool alerta = false;
    protected override void DoOnSpace()
    {
        TimeManager.Instance.SetCurrentTime(new GameTime(55, 0, 4, 55));
    }

    PNJMamaTule maamTule;
    internal override void ConfigurarInicio()
    {
        //StoreManager.ObtainTool(TypesGameElement.Tools.Watering_Can);
        //StoreManager.ObtainTool(TypesGameElement.Tools.Hoe);
        //StoreManager.ObtainSeed(TypesGameElement.Seeds.Tsiuna, 4);
        //TimeManager.Instance.SetDaysPerMinute(30);
        //GameManager.Instance.SetGameState(GameState.InGame);
        //maamTule = FindObjectOfType<PNJMamaTule>();
        //maamTule.enabled = true;
        //maamTule.GetComponent<AlertasMamaTule>().enabled = true;
        //k1 = KeyCode.B;
        //GameManager.Instance.SetGameState(GameState.InGame);
        //HungerManager.Instance.DecreaseHungerLevel(50);
        //StoreManager.ObtainSeed(TypesGameElement.Seeds.Corn, 4);
        //StoreManager.ObtainSeed(TypesGameElement.Seeds.Tsiuna, 4);

        //StoreManager.ObtainFruit(TypesGameElement.Fruits.Tsiuna, 4);
        //StoreManager.ObtainFruit(TypesGameElement.Fruits.Corn, 4);

        //HarmonyFlamesManager.Instance.IncreaseHarmonyFlamesLevel(9);
        //TimeManager.Instance.SetDaysPerMinute(10);

        //StoreManager.ObtainFruit(TypesGameElement.Fruits.Tsiuna, 2);
        //StoreManager.ObtainItem(TypeInventory.TOOL, 0, 1);
        //StoreManager.ObtainItem(TypeInventory.TOOL, 1, 1);
        //StoreManager.ObtainItem(TypeInventory.TOOL, 2, 1);
        //StoreManager.ObtainItem(TypeInventory.TOOL, 3, 1);
        //StoreManager.ObtainItem(TypeInventory.TOOL, 4, 1);
        //StoreManager.ObtainItem(TypeInventory.TOOL, 5, 1);

        //StoreManager.ObtainFood(TypesGameElement.Foods.Bread);
        //StoreManager.ObtainFood(TypesGameElement.Foods.Chontaduro);

        StoreManager.ObtainSeed(TypesGameElement.Seeds.Corn, 10);
        StoreManager.ObtainSeed(TypesGameElement.Seeds.Tsiuna, 10);
        StoreManager.ObtainFruit(TypesGameElement.Fruits.Tsiuna, 2);

        StoreManager.ObtainTool(TypesGameElement.Tools.Axe);
        StoreManager.ObtainTool(TypesGameElement.Tools.Hammer);
        StoreManager.ObtainTool(TypesGameElement.Tools.Hoe);
        GameManager.Instance.compraAzadon.comproEnTiendaDonJorge = true;
        //TimeManager.Instance.SetDaysPerMinute(10);
        GameManager.Instance.SetGameState(GameState.InGame);

        GameManager.Instance.SetNamePJ = "Eduardo";


    }



}
