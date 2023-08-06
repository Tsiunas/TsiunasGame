using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using UnityEngine;

public class DebugMuerte : DebugScript
{
    protected override void DoOnButton()
    {
        throw new System.NotImplementedException();
    }

    protected override void DoOnEnter()
    {
        StoreManager.ObtainFruit(TypesGameElement.Fruits.Tsiuna, 10);
        StoreManager.ObtainSeed(TypesGameElement.Seeds.Tsiuna, 5);
        StoreManager.ObtainSeed(TypesGameElement.Seeds.Corn, 5);
        HarmonyFlamesManager.Instance.HarmonyFlames = 5;
    }

    protected override void DoOnK1()
    {
        HungerManager.Instance.IncreaseHungerLevel(10);
    }

    protected override void DoOnK2()
    {
        HungerManager.Instance.DecreaseHungerLevel(10);
    }

    protected override void DoOnSpace()
    {

        HarmonyFlamesManager.Instance.DecreaseIntenistyHarmonyFlamesLevel(10);
    }

    internal override void ConfigurarInicio()
    {
        k2 = KeyCode.H;
        k1 = KeyCode.J;
        base.ConfigurarInicio();
    }

    internal override void ConfigurarOnEnable()
    {

        GameManager.Instance.SetGameState(GameState.InGame);
       

        
    }
}
