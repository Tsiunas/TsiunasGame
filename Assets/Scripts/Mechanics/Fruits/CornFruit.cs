using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CornFruit : Fruit, IPurchase {

    private int purchasePrice;

    public override string GetName
    {
        get
        {
            return typeof(CornFruit).ToString();
        }
    }

    public int PurchasePrice
    {
        get { return purchasePrice; }
        set { purchasePrice = value; }
    }

    public override FruitType fruitType
    {
        get
        {
            return FruitType.CornFruit;
        }
    }

    public CornFruit(int sellPrice, int purchasePrice, int satietyIncrease, int durability, FruitType fruitType) : base(sellPrice, satietyIncrease, durability, fruitType)
    {
        this.Name = GetName;
        this.SellPrice = sellPrice;
        this.PurchasePrice = purchasePrice;
        this.SatietyIncrease = satietyIncrease;
        this.Durability = durability;
        
    }

    public GameElement Purchase()
    {
        return this;
    }


}
