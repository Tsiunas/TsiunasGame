using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Water : Food {

    public override string GetName
    {
        get
        {
            return typeof(Water).ToString();
        }
    }

    public Water(int price, int durability, int satietyIncrease, FoodType foodType) : base(price, durability, satietyIncrease, foodType)
    {
        this.Name = GetName;
        this.PurchasePrice = price;
        this.Durability = durability;
        this.SatietyIncrease = satietyIncrease;
        this.foodType = foodType;
    }
}
