using System;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using UnityEngine;

public static class StoreManager {
	
	/// <summary>
	/// Se utiliza para comprar una herramiento siempre y cuando tenga dinero suficiente
	/// </summary>
	/// <returns><c>true</c>, si la herramienta fue comprada, <c>false</c> si la herramienta no fue comprada.</returns>
	/// <param name="_toolType">Tipo de herramienta a comprar.</param>
	/// <param name="_isGoldTool">Si se establece como <c>true</c> la herramienta a comprar es de oro.</param>
	public static bool BuyTool(TypesGameElement.Tools _toolType, bool _isGoldTool = false)
	{       
		// Crea una herramienta basado en su tipo
		Tool toolToBuy = GameElementFactory.Instance.CreateTool(_toolType, _isGoldTool);

		int finalPrice = _isGoldTool ? (toolToBuy.PurchasePrice * 2) : toolToBuy.PurchasePrice;
		// Se comprueba si tengo dinero suficiente para comprar la herramienta
		if (finalPrice <= GameManager.Instance.Money)
		{
			// añado la herramienta a la lista de herramientas
			GameManager.Instance.tools.Add(toolToBuy);
			TrackerSystem.Instance.SendTrackingData("user", "bought", "tool", toolToBuy._name+"|user|éxito");
			// Resto el valor de la herramienta del dinero actual
			GameManager.Instance.DecreaseMoneyAmount(finalPrice);
			TrackerSystem.Instance.SendTrackingData("user", "decreased", "currency", finalPrice+"|user|éxito");
			return true;
		}

		toolToBuy = null;
		return false;
	}

	internal static void LoseAllFruits()
	{
		GameManager.Instance.fruits.Clear();
	}

	internal static void LoseAllSeeds()
	{
		GameManager.Instance.seeds.Clear();
	}

	public static bool BuyTool(TypesGameElement.Tools _toolType, int priceWithDiscount, bool _isGoldTool = false)
	{
		// Crea una herramienta basado en su tipo
		Tool toolToBuy = GameElementFactory.Instance.CreateTool(_toolType, _isGoldTool);
		// Se comprueba si tengo dinero suficiente para comprar la herramienta
		if (priceWithDiscount <= GameManager.Instance.Money)
		{
			// añado la herramienta a la lista de herramientas
			GameManager.Instance.tools.Add(toolToBuy);
				TrackerSystem.Instance.SendTrackingData("user", "bought", "tool", toolToBuy._name+"|user|éxito");
			// Resto el valor de la herramienta del dinero actual
			GameManager.Instance.DecreaseMoneyAmount(priceWithDiscount);
			TrackerSystem.Instance.SendTrackingData("user", "decreased", "currency", priceWithDiscount+"|user|éxito");
			return true;
		}

		toolToBuy = null;
		return false;
	}

	/// <summary>
	/// Se utiliza para comprar un fruto de maíz siempre y cuando tenga dinero suficiente
	/// </summary>
	/// <returns><c>true</c>, si el fruto fue comprado, <c>false</c> si el fruto no fue comprado.</returns>
	public static bool BuyFruitCorn()
	{
		// Crea una un fruto de maíz
		CornFruit fruitToBuy = (CornFruit)GameElementFactory.Instance.CreateFruit(TypesGameElement.Fruits.Corn);       

		// Se comprueba si tengo dinero suficiente para comprar el fruto de maíz
		if (fruitToBuy.PurchasePrice <= GameManager.Instance.Money)
		{
			// añado el fruto de maíz a la lista de frutos
			GameManager.Instance.fruits.Add((Fruit)fruitToBuy);
				TrackerSystem.Instance.SendTrackingData("user", "bought", "item", fruitToBuy._name+"|user|éxito");
			// Resto el valor del fruto de maíz del dinero actual
			GameManager.Instance.DecreaseMoneyAmount(fruitToBuy.PurchasePrice);
			TrackerSystem.Instance.SendTrackingData("user", "decreased", "currency", fruitToBuy.PurchasePrice+"|user|éxito");
			TrackerSystem.Instance.SendTrackingData("user", "increased", "item", fruitToBuy._name+"|user|éxito");
			return true;
		}
		fruitToBuy = null;
		return false;
	}

	/// <summary>
	/// Se utiliza para comprar una comida, siempre y cuando tenga dinero suficiente
	/// </summary>
	/// <returns><c>true</c>, si la comida fue comprada, <c>false</c> si la comida no fue comprada.</returns>
	/// <param name="_typeFood">Type food.</param>
	public static bool BuyFood(TypesGameElement.Foods _typeFood) {

		// IDEM: otros métodos de compra de elementos de juego
		Food foodToBuy = GameElementFactory.Instance.CreateFood(_typeFood);
		if (foodToBuy.PurchasePrice <= GameManager.Instance.Money) {
			GameManager.Instance.foods.Add(foodToBuy);
			TrackerSystem.Instance.SendTrackingData("user", "bought", "item", _typeFood+"|user|éxito");
			GameManager.Instance.DecreaseMoneyAmount(foodToBuy.PurchasePrice);
			TrackerSystem.Instance.SendTrackingData("user", "decreased", "currency", foodToBuy.PurchasePrice+"|user|éxito");

			return true;
		}
		foodToBuy = null;
		return false;
	}
   
	public static void BuyTsiunaFruit(int amountToObtain = 1)
	{
		for (int i = 0; i < amountToObtain; i++)
			GameManager.Instance.fruits.Add(GameElementFactory.Instance.CreateFruit(TypesGameElement.Fruits.Tsiuna));
	}

	public static void ObtainFruit(TypesGameElement.Fruits _typeFruit, int amountToObtain = 1)
	{
		for (int i = 0; i < amountToObtain; i++)
			GameManager.Instance.fruits.Add(GameElementFactory.Instance.CreateFruit(_typeFruit));
			TrackerSystem.Instance.SendTrackingData("user", "earned", "item", "fruta_"+_typeFruit+"|user|éxito");
	}

	public static void ObtainSeed(TypesGameElement.Seeds _typeSeed, int amountToObtain = 1)
	{
		for (int i = 0; i < amountToObtain; i++)
			GameManager.Instance.seeds.Add(GameElementFactory.Instance.CreateSeed(_typeSeed));
			TrackerSystem.Instance.SendTrackingData("user", "earned", "item", "semilla_"+_typeSeed+"|user|éxito");
	}
	public static Tool ObtainTool(TypesGameElement.Tools _typeTool, int amountToObtain = 1, bool isGold = false)
	{
		Tool tool = null;
		for (int i = 0; i < amountToObtain; i++)
		{
			tool = GameElementFactory.Instance.CreateTool(_typeTool, isGold);
			GameManager.Instance.tools.Add(tool);	
			TrackerSystem.Instance.SendTrackingData("user", "earned", "tool", tool._name+"|user|éxito");		
		}
		return tool;
	}

	public static void ObtainFood(TypesGameElement.Foods _typeFood, int amountToObtain = 1)
	{
		for (int i = 0; i < amountToObtain; i++)
			GameManager.Instance.foods.Add(GameElementFactory.Instance.CreateFood(_typeFood));		
			TrackerSystem.Instance.SendTrackingData("user", "earned", "item", _typeFood+"|user|éxito");			
	}

	/// <summary>
	/// Se utiliza para vender una semilla
	/// </summary>
	/// <param name="seed">Semilla a vender</param>
	public static void SellSeed(Seed seed) {
		GameManager instance = GameManager.Instance;
		instance.seeds.Remove(seed);
		TrackerSystem.Instance.SendTrackingData("user", "sold", "item", seed._name+"|user|éxito");
		TrackerSystem.Instance.SendTrackingData("user", "decreased", "item", seed._name+"|user|éxito");
	}

	/// <summary>
	/// Se utiliza para vender un fruto
	/// </summary>
	/// <param name="fruit">Fruto a vender</param>
	public static void SellFruit(Fruit fruit) {
		GameManager instance = GameManager.Instance;
		instance.fruits.Remove(fruit);
		TrackerSystem.Instance.SendTrackingData("user", "sold", "item", fruit._name+"|user|éxito");
		TrackerSystem.Instance.SendTrackingData("user", "decreased", "item", fruit._name+"|user|éxito");
	}

	internal static void ObtainItem(TypeInventory tipoItem, int idItem, int cant = 1, bool isGold = false)
	{
		switch (tipoItem)
		{
			case TypeInventory.TOOL:
				ObtainTool((TypesGameElement.Tools)idItem, cant, isGold);
				break;
			case TypeInventory.FOOD:
				ObtainFood((TypesGameElement.Foods)idItem, cant);
				break;
			case TypeInventory.SEED:
				ObtainSeed((TypesGameElement.Seeds)idItem, cant);
				break;
			case TypeInventory.FRUIT:
				ObtainFruit((TypesGameElement.Fruits)idItem, cant);
				break;
			default:
				throw new TsiunasException("Error al Obtener Item");                
		}
		Debug.Log("Item Obtenido: " + tipoItem.ToString());

	}

	
}
