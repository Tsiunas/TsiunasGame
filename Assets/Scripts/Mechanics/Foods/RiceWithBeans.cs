using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RiceWithBeans : Food {

    public override string GetName
    {
        get
        {
            return typeof(RiceWithBeans).ToString();
        }
    }

    public RiceWithBeans(int price, int durability, int satietyIncrease, FoodType foodType) : base(price, durability, satietyIncrease, foodType)
    {
        this.Name = GetName;
        this.PurchasePrice = price;
        this.Durability = durability;
        this.SatietyIncrease = satietyIncrease;
        this.foodType = foodType;
    }
}
