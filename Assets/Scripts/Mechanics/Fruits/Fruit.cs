using UnityEngine;
using System.Collections;
using System;
using Tsiunas.Mechanics;

public enum FruitType { CornFruit, TsiunaFruit, None }
[System.Serializable]
public abstract class Fruit : GameElement, ISell, IEat, IGive
{
    // Attributes
    #region Attributes
    
    private int sellPrice;
    
    private int satietyIncrease;
    
    public abstract FruitType fruitType { get; }
    #endregion

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

    public int SellPrice
    {
        get { return sellPrice; }
        set { sellPrice = value; }
    }

    public int SatietyIncrease
    {
        get { return satietyIncrease; }
        set { satietyIncrease = value; }
    }
    #endregion

    // Constructor
    #region Constructor
    protected Fruit(int sellPrice, int satietyIncrease, int durability, FruitType fruitType)
    {
        this.sellPrice = sellPrice;
        this.satietyIncrease = satietyIncrease;
        this._durability = durability;
        
    }
    #endregion

    // Methods
    #region Methods
    public GameElement Sell()
    {
        Debug.Log(GetName + " fue: vendido por " + sellPrice.ToString());
        DecreaseDurability();
        GameManager.Instance.IncreaseMoneyAmount(this.sellPrice);
        return this;
    }

    public void BeEaten()
    {
        DecreaseDurability();
        HungerManager.Instance.IncreaseHungerLevel(satietyIncrease);
    }

    public GameElement BeGiven()
    {
        Debug.Log(GetName + " fue: dado");
        DecreaseDurability();
        return this;
    }


    #endregion
}
