using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tsiunas.Mechanics;

public class Cliente : Singleton<Cliente> {

    private bool unicaVez;
    public int flamas;
    public Cliente()
    {
        uniqueToAllApp = true;
    }
	// Use this for initialization
	void OnEnable () {

        if (!unicaVez) {
            unicaVez = true;
            GameManager.Instance.SetGameState(GameState.InGame);
            StartCoroutine(InitWithGameElements());
        }
    }

    IEnumerator InitWithGameElements() {
        yield return new WaitUntil(() => TexturesManager.Instance.allTexturesHaveBeenLoaded == true );

        StoreManager.BuyTool(_toolType: TypesGameElement.Tools.Axe, _isGoldTool: true);

        StoreManager.BuyTool(_toolType: TypesGameElement.Tools.Machete, _isGoldTool: true);
        StoreManager.BuyTool(_toolType: TypesGameElement.Tools.Axe, _isGoldTool: true);
        StoreManager.BuyTool(_toolType: TypesGameElement.Tools.Machete, _isGoldTool: true);
        StoreManager.BuyTool(_toolType: TypesGameElement.Tools.Hoe, _isGoldTool: true);
        StoreManager.BuyTool(_toolType: TypesGameElement.Tools.Hoe, _isGoldTool: true);

        StoreManager.BuyTool(TypesGameElement.Tools.Hand);

        StoreManager.BuyTool(TypesGameElement.Tools.Hammer);
        StoreManager.BuyTool(TypesGameElement.Tools.Watering_Can);
        //StoreManager.BuyFruitCorn();

        StoreManager.BuyFood(TypesGameElement.Foods.Bread);
        StoreManager.BuyFood(TypesGameElement.Foods.Bread);
        StoreManager.BuyFood(TypesGameElement.Foods.Bread);
        StoreManager.BuyFood(TypesGameElement.Foods.RiceWithBeans);
        StoreManager.BuyFood(TypesGameElement.Foods.Chontaduro);
        StoreManager.BuyFood(TypesGameElement.Foods.Coffe);
        StoreManager.BuyFood(TypesGameElement.Foods.Fish);
        StoreManager.BuyFood(TypesGameElement.Foods.Milk);
        StoreManager.BuyFood(TypesGameElement.Foods.Soup);
        StoreManager.BuyFood(TypesGameElement.Foods.Water);


        //StoreManager.BuyFruitCorn();
        StoreManager.ObtainSeed(TypesGameElement.Seeds.Corn);
        StoreManager.ObtainSeed(TypesGameElement.Seeds.Tsiuna);
        //StoreManager.ObtainFruit(TypesGameElement.Fruits.Tsiuna);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) {
            //StoreManager.BuyTool(TypesGameElement.Tools.Watering_Can);
            StoreManager.BuyFood(TypesGameElement.Foods.Soup);
            StoreManager.BuyFood(TypesGameElement.Foods.Water);
            StoreManager.BuyFruitCorn();
            StoreManager.ObtainSeed(TypesGameElement.Seeds.Corn);
            StoreManager.ObtainSeed(TypesGameElement.Seeds.Tsiuna);

            StoreManager.ObtainFruit(TypesGameElement.Fruits.Corn);
            StoreManager.ObtainFruit(TypesGameElement.Fruits.Tsiuna, 4);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            HarmonyFlamesManager.Instance.IncreaseHarmonyFlamesLevel(flamas);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            HarmonyFlamesManager.Instance.DecreaseHarmonyFlamesLevel(flamas);
        }
    }
}
