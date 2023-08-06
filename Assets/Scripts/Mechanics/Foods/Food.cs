using System;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using UnityEngine;

public enum FoodType { None, Bread, RiceWithBeans, Soup, Coffe, Milk, Water, Chontaduro, Fish }
[System.Serializable]
public abstract class Food : GameElement, IPurchase, IEat {
    
    private int price;
    
    private int satietyIncrease;
    
    public FoodType foodType;

    // Properties
    #region Properties
    public abstract string GetName { get; }

    public string Name
    {
        set { if (_name == null) this._name = value; else throw new InvalidOperationException(GetType().ToString() + " has already been established"); }
    }

    public int Durability
    {
        get { return this._durability; }
        set { _durability = value; }
    }

    public int PurchasePrice
    {
        get { return price; }
        set { price = value; }
    }

    public int SatietyIncrease
    {
        get { return satietyIncrease; }
        set { satietyIncrease = value; }
    }
    #endregion

    #region Constructor
    protected Food(int price, int durability, int satietyIncrease, FoodType foodType)
    {
        this.price = price;
        this._durability = durability;
        this.satietyIncrease = satietyIncrease;
        this.foodType = foodType;
    }

    public GameElement Purchase()
    {
        return this;
    }

    public void BeEaten()
    {
        DecreaseDurability();
        HungerManager.Instance.IncreaseHungerLevel(satietyIncrease);
    }
    #endregion
}