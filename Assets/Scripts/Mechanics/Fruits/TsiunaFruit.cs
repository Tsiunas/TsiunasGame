using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TsiunaFruit : Fruit {

    public override string GetName
    {
        get
        {
            return typeof(TsiunaFruit).ToString();
        }
    }

    public override FruitType fruitType
    {
        get
        {
            return FruitType.TsiunaFruit;
        }
    }

    public TsiunaFruit(int sellPrice, int satietyIncrease, int durability, FruitType fruitType) : base(sellPrice, satietyIncrease, durability, fruitType)
    {
        this.Name = GetName;
        this.SellPrice = sellPrice;
        this.SatietyIncrease = satietyIncrease;
        this.Durability = durability;
        
    }
}
