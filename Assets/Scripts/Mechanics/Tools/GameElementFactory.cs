using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface IToolFactory
/// </summary>
public interface IGameElementFactory {
    Tool CreateTool(TypesGameElement.Tools typeTool, bool isGoldTool);
    Food CreateFood(TypesGameElement.Foods typeFood);
    Seed CreateSeed(TypesGameElement.Seeds typeSeed);
    Fruit CreateFruit(TypesGameElement.Fruits typeFruit);
}

/// <summary>
/// Usa el patrón de diseño: Abstract Factory
/// </summary>
public class GameElementFactory : MonoBehaviour, IGameElementFactory
{
	// Attributes
	#region Attributes
	// Singleton Pattern
	#region Singleton Pattern
    /// <summary>
    /// Instancia de la clase ToolFactory
    /// </summary>
    private static GameElementFactory instance;

    public static GameElementFactory Instance
	{
		get
		{
			if(instance == null)
			{
                GameObject gameElementFactory = new GameObject("GameElementFactory");
                DontDestroyOnLoad(gameElementFactory);
                instance = gameElementFactory.AddComponent<GameElementFactory>();
			}

			return instance;
		}
	}
    #endregion
    #endregion

    // Methods
    #region Methods
    /// <summary>
    /// Sirve para crear una herramienta
    /// (Sirve para crear un nuevo objeto de Tipo Tool dependiendo de la constante que se pase como parámetro)
    /// </summary>
    /// <returns>La herramienta creada.</returns>
    /// <param name="typeTool">Tipo de Herramienta.</param>
    /// <param name="isGoldTool">Si se setea como <c>true</c> la herramienta a crear es de oro.</param>
    public Tool CreateTool(TypesGameElement.Tools typeTool, bool isGoldTool = false)
    {
        // Dependiendo del tipo crea cada herramienta con sus características particulares
        Tool toolCreated = null;
        switch (typeTool)
        {
            case TypesGameElement.Tools.Axe:
                // En caso de ser un Hacha ésta puede tener 2 objetivos válidos: un Árbol o un Tronco cortado
                toolCreated = new Axe(GetToolDurability(ToolHelp.Durability.axe, isGoldTool), ToolHelp.Costs.axe, isGoldTool, null, ToolType.Axe, TargetType.TREE, TargetType.CUTTRUNK, TargetType.PLANT);
                break;

            case TypesGameElement.Tools.Hammer:
                toolCreated = new Hammer(GetToolDurability(ToolHelp.Durability.hammer, isGoldTool), ToolHelp.Costs.hammer, isGoldTool, null, ToolType.Hammer, TargetType.ROCK, TargetType.PLANT);
                break;

            case TypesGameElement.Tools.Hoe:
                toolCreated = new Hoe(GetToolDurability(ToolHelp.Durability.hoe, isGoldTool), ToolHelp.Costs.hoe, isGoldTool, null, ToolType.Hoe, TargetType.GROUND, TargetType.PLANT);
                break;

            case TypesGameElement.Tools.Machete:
                // En caso de ser un Machete éste puede tener 2 objetivos válidos: una Planta o la Maleza
                toolCreated = new Machete(GetToolDurability(ToolHelp.Durability.machete, isGoldTool), ToolHelp.Costs.machete, isGoldTool, null, ToolType.Machete, TargetType.PLANT , TargetType.UNDERGROWTH);
                break;

            case TypesGameElement.Tools.Watering_Can:
                toolCreated = new Watering_Can(GetToolDurability(ToolHelp.Durability.watering_can, isGoldTool), ToolHelp.Costs.watering_can, isGoldTool, null, ToolType.Watering_Can, TargetType.PLANT);
                break;

            case TypesGameElement.Tools.Hand:
                toolCreated = new Hand(ToolHelp.Durability.hand, ToolHelp.Costs.hand, isGoldTool, null, ToolType.Hand, TargetType.PLANT);
                break;
        }

        toolCreated.finishedDurability += (thisGameElement) => { GameManager.Instance.tools.Remove((Tool)thisGameElement); };
        return toolCreated;
    }

    public Food CreateFood(TypesGameElement.Foods typeFood)
    { 
        Food foodCreated = null;
        switch (typeFood)
        {
            // Comidas
            #region Comidas
            case TypesGameElement.Foods.Bread:
                foodCreated = new Bread(FoodHelp.Costs.bread, FoodHelp.Durability.bread, FoodHelp.SatietyIncrease.bread, FoodType.Bread);
                break;
            case TypesGameElement.Foods.RiceWithBeans:
                foodCreated = new RiceWithBeans(FoodHelp.Costs.riceWithBeans, FoodHelp.Durability.riceWithBeans, FoodHelp.SatietyIncrease.riceWithBeans, FoodType.RiceWithBeans);
                break;
            case TypesGameElement.Foods.Soup:
                foodCreated = new Soup(FoodHelp.Costs.soup, FoodHelp.Durability.soup, FoodHelp.SatietyIncrease.soup, FoodType.Soup);
                break;
            case TypesGameElement.Foods.Coffe:
                foodCreated = new Coffe(FoodHelp.Costs.coffe, FoodHelp.Durability.coffe, FoodHelp.SatietyIncrease.coffe, FoodType.Coffe);
                break;
            case TypesGameElement.Foods.Milk:
                foodCreated = new Milk(FoodHelp.Costs.milk, FoodHelp.Durability.milk, FoodHelp.SatietyIncrease.milk, FoodType.Milk);
                break;
            case TypesGameElement.Foods.Water:
                foodCreated = new Water(FoodHelp.Costs.water, FoodHelp.Durability.water, FoodHelp.SatietyIncrease.water, FoodType.Water);
                break;
            case TypesGameElement.Foods.Chontaduro:
                foodCreated = new Chontaduro(FoodHelp.Costs.chontaduro, FoodHelp.Durability.chontaduro, FoodHelp.SatietyIncrease.chontaduro, FoodType.Chontaduro);
                break;
            case TypesGameElement.Foods.Fish:
                foodCreated = new Fish(FoodHelp.Costs.fish, FoodHelp.Durability.fish, FoodHelp.SatietyIncrease.fish, FoodType.Fish);
                break;
            #endregion
        }

        foodCreated.finishedDurability += (thisGameElement) => { GameManager.Instance.foods.Remove((Food)thisGameElement); };
        return foodCreated;
    }

    public Seed CreateSeed(TypesGameElement.Seeds typeSeed)
    {  Seed seedCreated = null;
        switch (typeSeed)
        {
            // Semillas
            #region Semillas
            case TypesGameElement.Seeds.Corn:
                seedCreated = new CornSeed(sellPrice: SeedHelp.SalePrices.corn, satietyIncrease: SeedHelp.SatietyIncrease.corn, durability: SeedHelp.Durability.corn, seedType: SeedType.CornSeed);
                break;
            case TypesGameElement.Seeds.Tsiuna:
                seedCreated = new TsiunaSeed(sellPrice: SeedHelp.SalePrices.tsiuna, satietyIncrease: SeedHelp.SatietyIncrease.tsiuna, durability: SeedHelp.Durability.tsiuna, seedType: SeedType.TsiunaSeed);
                break;
                #endregion
        }
        seedCreated.finishedDurability += (thisGameElement) => { GameManager.Instance.seeds.Remove((Seed)thisGameElement); };
        return seedCreated;
    }

    public Fruit CreateFruit(TypesGameElement.Fruits typeFruit)
    {
        Fruit fruitCreated = null;
        switch (typeFruit)
        {
            // Frutos
            #region Frutos
            case TypesGameElement.Fruits.Corn:
                fruitCreated = new CornFruit(sellPrice: FruitHelp.SalePrices.corn, purchasePrice: FruitHelp.PurchasePrices.corn, satietyIncrease: FruitHelp.SatietyIncrease.corn, durability: FruitHelp.Durability.corn, fruitType: FruitType.CornFruit);
                break;
            case TypesGameElement.Fruits.Tsiuna:
                fruitCreated = new TsiunaFruit(sellPrice: FruitHelp.SalePrices.tsiuna, satietyIncrease: FruitHelp.SatietyIncrease.tsiuna, durability: FruitHelp.Durability.tsiuna, fruitType: FruitType.TsiunaFruit);
                break;

                #endregion
        }
        fruitCreated.finishedDurability += (thisGameElement) => { GameManager.Instance.fruits.Remove((Fruit)thisGameElement); };
        return fruitCreated;
    }

    int GetToolDurability(int baseDurability, bool isGold) {
        return isGold ? (baseDurability + ToolHelp.Durability.plusForGold) : baseDurability; 
    }
	#endregion
}

