using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CornSeed : Seed
{
    public override string GetName
    {
        get
        {
            return typeof(CornSeed).ToString();
        }
    }

    public override SeedType SeedType
    {
        get
        {
            return SeedType.CornSeed;
        }
    }

    public CornSeed(int sellPrice, int satietyIncrease, int durability, SeedType seedType) : base(sellPrice, satietyIncrease, durability, seedType)
    {
        this.Name = GetName;
        this.SellPrice = sellPrice;
        this.SatietyIncrease = satietyIncrease;
        this.Durability = durability;
        
    }
}
