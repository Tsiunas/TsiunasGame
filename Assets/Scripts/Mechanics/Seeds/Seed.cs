
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tsiunas.Mechanics;

public enum SeedType { CornSeed, TsiunaSeed, None }
[System.Serializable]
public abstract class Seed : GameElement, IEat, ISell, IGive, ISow
{
	// Attributes
	#region Attributes
	
	private int sellPrice;
	
	private int satietyIncrease;
	
	public abstract SeedType SeedType { get; }
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
		get { return _durability; }
		set { _durability = value; }
	}

	public int SatietyIncrease
	{
		get { return satietyIncrease; }
		set { satietyIncrease = value; }
	}

	public int SellPrice
	{
		get { return sellPrice; }
		set { sellPrice = value; }
	}
	#endregion

	// Constructor
	#region Constructor
	protected Seed(int sellPrice, int satietyIncrease, int durability, SeedType seedType)
	{
		this.sellPrice = sellPrice;
		this.satietyIncrease = satietyIncrease;
		Durability = durability;
		
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
		Debug.Log(GetName + " fue: comido");
		TrackerSystem.Instance.SendTrackingData("user", "used", "item", GetName+"|user|éxito");
		TrackerSystem.Instance.SendTrackingData("user", "decreased", "item", GetName+"|user|éxito");
		DecreaseDurability();
		HungerManager.Instance.IncreaseHungerLevel(satietyIncrease);
	}

	public GameElement BeGiven()
	{
		Debug.Log(GetName + " fue: dado");		
		DecreaseDurability();
		return this;
	}

	public void BeSowed(GameObject _sproutParent)
	{
		Debug.Log(GetName + " fue: sembrado");        
		DecreaseDurability();
		// Realiza llamada a método de sembrar
		PlantManager.Instance.SowSeed(ReturnPlantTypeBySeedType(this.SeedType), _sproutParent);
		TrackerSystem.Instance.SendTrackingData("user", "dragged", "item", GetName+"|Farm|éxito");
		TrackerSystem.Instance.SendTrackingData("user", "decreased", "item", GetName+"|user|éxito");
	}

	/// <summary>
	/// Regresa el tipo de planta dado un tipo de semilla
	/// Planta tipo maíz es regresado cuando la Semilla es de tipo maíz
	/// Planta tipo Tsina es regresado cuando la Semilla es de tipo Tsiuna
	/// </summary>
	/// <returns>El tipo de Planta requerido </returns>
	/// <param name="_seedType">Tipo de semilla</param>
	Plant.PlantTypes ReturnPlantTypeBySeedType(SeedType _seedType) {
		Plant.PlantTypes plantType = Plant.PlantTypes.Maiz;
		switch (_seedType)
		{
			case SeedType.CornSeed:
				plantType = Plant.PlantTypes.Maiz;
				break;
			case SeedType.TsiunaSeed:
				plantType = Plant.PlantTypes.Tsiuna;
				break;
		}
		return plantType;
	}
	#endregion
}
