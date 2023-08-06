using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fish : Food {

    public override string GetName
    {
        get
        {
            return typeof(Fish).ToString();
        }
    }

    public Fish(int price, int durability, int satietyIncrease, FoodType foodType) : base(price, durability, satietyIncrease, foodType)
    {
        this.Name = GetName;
        this.PurchasePrice = price;
        this.Durability = durability;
        this.SatietyIncrease = satietyIncrease;
        this.foodType = foodType;
    }
}
