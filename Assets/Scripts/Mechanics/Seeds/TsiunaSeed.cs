using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TsiunaSeed : Seed {

    public override string GetName
    {
        get
        {
            return typeof(TsiunaSeed).ToString();
        }
    }

    public override SeedType SeedType
    {
        get
        {
            return SeedType.TsiunaSeed;
        }
    }

    public TsiunaSeed(int sellPrice, int satietyIncrease, int durability, SeedType seedType) : base(sellPrice, satietyIncrease, durability, seedType)
    {
        this.Name = GetName;
        this.SellPrice = sellPrice;
        this.SatietyIncrease = satietyIncrease;
        this.Durability = durability;
        
    }

    public TsiunaSeed():this(SeedHelp.SalePrices.tsiuna, SeedHelp.SatietyIncrease.tsiuna, SeedHelp.Durability.tsiuna, SeedType.TsiunaSeed)
    {

    }
}
