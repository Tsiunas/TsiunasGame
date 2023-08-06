using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Tsiunas.Mechanics;
using UnityEngine.SceneManagement;
using Tsiunas.SistemaDialogos;

/// <summary>
/// Este enum representa los estados de juego
/// </summary>
[System.Serializable]
public enum GameState { InIntro, InGame, InPause, Finished };
public partial class GameManager : Singleton<GameManager>, IManejoVariables {

	// Attributes
	#region Attributes
    // El estado actual del juego
    private GameState gameState = GameState.InIntro;

    internal static IEat GetEatable(UIDragGameElement uIDragGameElement)
    {
        if (uIDragGameElement is UIDragFood)
        {
            return GameManager.Instance.currentDragFood;
        }
        if (uIDragGameElement is UIDragSeed)
        {
            return GameManager.Instance.currentDragSeed;
        }
        if(uIDragGameElement is UIDragFruit)
        {
            return GameManager.Instance.currentDragFruit;
        }
        return null;
    }

    // name PJ
    private string namePJ;

	
	private int money = 20;
	
	private Hoe hoe;
	private bool boughtInStore;


    public ObservedList<Tool> tools = new ObservedList<Tool>();
    public ObservedList<Fruit> fruits = new ObservedList<Fruit>();
    public ObservedList<Seed> seeds = new ObservedList<Seed>();
    public ObservedList<Food> foods = new ObservedList<Food>();

    public List<GameElement> ReturnListOfGameElements (GameElement _gameElement) {
        if (_gameElement is Food) return foods.Cast<GameElement>().ToList();
        if (_gameElement is Tool) return tools.Cast<GameElement>().ToList();
        if (_gameElement is Seed) return seeds.Cast<GameElement>().ToList();
        if (_gameElement is Fruit) return fruits.Cast<GameElement>().ToList();
        return null;
    }

    internal void ActivarMuertePorHambre()
    {
        SceneLoadManager.Instance.CargarEscena("MuerteHambre");
        SetGameState(GameState.InPause);
    }

    internal void ActivarMuerteMamaTule()
    {
        SceneLoadManager.Instance.CargarEscena("MuerteMamaTule");
        SetGameState(GameState.InPause);
    }

    public Tool currentToolActive = null;
    public Food currentDragFood = null;
    public Seed currentDragSeed = null;
    public Fruit currentDragFruit = null;

    // Delegado y evento, para notificar cuando el estado actual del juego cambie
    public delegate void OnGameStateChange(GameState gameState);
    public event OnGameStateChange gameStateChange;

	
	
   
    public Action<int> moneyAmountChange = delegate { };
	
	
    public Action<int> harmonyFlamesLevelChange;

    public CompraAzadon compraAzadon = new CompraAzadon();

	#endregion

	// Properties
	#region Properties
    // Regresa el estado actual del juego
    public string GetNamePJ {
        get { return this.namePJ; }
    }
    public string SetNamePJ {
        set { this.namePJ = value; }
    }

    public GameState GetGameState {
        get { return this.gameState; }
    }	

	public int Money {
		get { return money; }
		set { if(value > 100) money = 100; else if(value < 0) money = 0; else money = value; }
	}

	

   

    public Hoe Hoe {
		protected get { return this.hoe; }
		set { 
			if (this.hoe == null) this.hoe = value;	else throw new InvalidOperationException("Hoe has already been established");
		}
	}

	public bool BoughtInStore {
		get { return this.boughtInStore; }
		set { boughtInStore = value; }
	}

    public bool ComproAzadonEnAlgunaTienda
    {
        get
        {
            return compraAzadon.comproEnTiendaDayami || compraAzadon.comproEnTiendaDonJorge;
        }
    }
    #endregion

    public GameManager () { base.uniqueToAllApp = true; }

    // Methods
    #region Methods
    internal static void SellFruitOrSeed(ISell currentElementToSell, PNJActor actorWhichBuyed)
    {
        currentElementToSell.Sell();
        SoundManager.PlaySound(SoundManager.SonidosGenerales.Vender);
          
        DesplegadorDialogo.Instance.DesplegarLinea(actorWhichBuyed.datosPNJ, new Speech(Instance.GetNamePJ +  ", te compro esto por <size=18><b><color=#a52a2aff>" + currentElementToSell.SellPrice + " monedas de oro</color></b></size>"));


        StaticCoroutine.DoCoroutine(Util.CreateUILerpGameElement(actorWhichBuyed.gameObject,
                                                                 GameObject.Find("Canvas/MoneyUI/Icon"),
                                                                 TexturesManager.Instance.GetSpriteFromSpriteSheet("02_Dinero_02"),
                                                                    5));
	
    }

    private void Awake()
    {
        PersistenceManager.Instance.PerformProfileDataLoading((ProfileData pD) => {
            this.Money = pD.profile_Money;
            SetGameState(pD.profile_GameState);
            this.SetNamePJ = pD.profile_namePJ;

            if (pD.profile_Tools != null)
            {
                pD.profile_Tools.ForEach((x) => this.tools.Add(x));
            }
            if (pD.profile_Seeds != null)
            {
                pD.profile_Seeds.ForEach((x) => this.seeds.Add(x));
            }
            if (pD.profile_Fruits != null)
            {
                pD.profile_Fruits.ForEach((x) => this.fruits.Add(x));
            }
            if (pD.profile_Foods != null)
            {
                pD.profile_Foods.ForEach((x) => this.foods.Add(x));
            }
        });       
        //Forzar el cambio de los Toggles

        if(!WasObtainedTool(ToolType.Hand))
            StoreManager.BuyTool(TypesGameElement.Tools.Hand);
    }

    private new void Start()
    {
        // añade herramienta mano y la selecciona

        EstablecerHerramienta();

        TimeManager.Instance.finishedGameTime += TerminarJuego;

    }

    void EstablecerHerramienta() {
        UIToolInventory uitool = FindObjectOfType<UIToolInventory>();
        if (uitool == null)
            return;
        var toolFinded = uitool.Toggles.Find(toggle => toggle.GetComponent<UIHerramienta>().toolType == (currentToolActive == null ? ToolType.Hand : currentToolActive.ToolType));
        if (toolFinded == null)
            throw new TsiunasException("Herramienta no encontrada, asegurate de haberla adquirido antes", true, "HERRAMIENTAS", "Eduardo");
        else
            toolFinded.isOn = true;
    }

    public void TerminarJuego()
    {
        if (GetGameState != GameState.Finished)
        {
            SceneLoadManager.Instance.CargarEscena("FinalJuego", 1f);
            SetGameState(GameState.Finished);
        }

    }

    /// <summary>
    /// Establece el estado de juego actual
    /// </summary>
    /// <param name="state">estado a establecer</param>
    public void SetGameState(GameState state) {
        this.gameState = state;
        if (gameStateChange != null) gameStateChange(this.gameState);
    }

	

	/// <summary>
	/// Aumenta la cantidad de DINERO
	/// </summary>
	/// <param name="valueToSet">cantidad a incrementar.</param>
	public void IncreaseMoneyAmount (int amountToIncrease) {		
        this.Money += amountToIncrease;
        moneyAmountChange(this.Money);
	}

	/// <summary>
	/// Decrementa la cantidad de DINERO
	/// <param name="valueToSet">cantidad a decrementar.</param>
	public void DecreaseMoneyAmount (int amountToDecrease) {		
        this.Money -= amountToDecrease;
        moneyAmountChange(this.Money);
	}

	

	/// <summary>
	/// Regresa true si ya se obtuvo el azadón, de lo contrario regresa false 
	/// </summary>
	/// <returns><c>true</c>, si ya se obtuvo el azadón, <c>false</c> si aún no se obtuvo el azadón.</returns>
	public bool WasObtainedHoe() {
		return (this.hoe != null) ? true : false;
	}

	/// <summary>
	/// Obtiene el Azadón (GameObject)
	/// </summary>
	public Hoe GetHoe () {
		return this.Hoe;
	}

	/// <summary>
	/// Establece el Azadón
	/// </summary>
	/// <param name="_hoe">GameObject a establecer</param>
	public void SetHoe (Hoe _hoe) {
		this.Hoe = _hoe;
	}

	public void AddTool (Tool _tool) {
		if (this.tools.Any(toolIntoList => toolIntoList.GetType() == _tool.GetType())) throw new InvalidOperationException (_tool.GetName + " has already been added to tools list");
		else this.tools.Add (_tool);
        
	}

    public Tool GetTool (ToolType type) {
        
        Type _type = Type.GetType (type.ToString());      
		if (_type.BaseType == typeof(Tool)) {
			try {
				//return this.tools.Where (t => t.GetType ().Equals (type)).First ();
                return this.tools.FindLast(t => t.GetType() == _type);
			} catch (Exception ex) {
				throw new InvalidOperationException ("The Tool list not contain a " + _type.ToString ());
			}
		}
		else
			throw new InvalidOperationException ("The BaseType of " + type.ToString() +  " it's not Tool");
	}

    public Food GetFood(FoodType type)
    {
        Type _type = Type.GetType(type.ToString());
        if (_type.BaseType == typeof(Food))
        {
            try
            {
                return this.foods.Find(t => t.GetType() == _type);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("The Food list not contain a " + _type.ToString());
            }
        }
        else
            throw new InvalidOperationException("The BaseType of " + type.ToString() + " it's not Food");
    }

    public Seed GetSeed(SeedType type)
    {
        Type _type = Type.GetType(type.ToString());
        if (_type.BaseType == typeof(Seed))
        {
            try
            {
                return this.seeds.Find(t => t.GetType() == _type);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("The Seed list not contain a " + _type.ToString());
            }
        }
        else
            throw new InvalidOperationException("The BaseType of " + type.ToString() + " it's not Seed");
    }

    public Fruit GetFruit(FruitType type)
    {
        Type _type = Type.GetType(type.ToString());
        if (_type.BaseType == typeof(Fruit))
        {
            try
            {
                return this.fruits.Find(t => t.GetType() == _type);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("The Fruit list not contain a " + _type.ToString());
            }
        }
        else
            throw new InvalidOperationException("The BaseType of " + type.ToString() + " it's not Fruit");
    }

	public bool WasObtainedTool(ToolType type) {
		Type _type = Type.GetType (type.ToString ());
		if (_type.BaseType == typeof(Tool)) {
			if (this.tools.Any (toolIntoList => toolIntoList.GetType () == _type)) return true;
			else return false;
		}
		else throw new InvalidOperationException ("The BaseType of " + type.ToString() +  " it's not Tool");
	}


    #endregion

    #region Implementación IManejoVariables
    int IManejoVariables.SubirVariable(Variables v, int cantidad)
    {
        switch (v)
        {
            case Variables.HAMBRE:
                HungerManager.Instance.IncreaseHungerLevel(cantidad);
                return HungerManager.Instance.Hungry;
            case Variables.DINERO:
                IncreaseMoneyAmount(cantidad);
                return Money;
            case Variables.FA:
                HarmonyFlamesManager.Instance.IncreaseIntenistyHarmonyFlamesLevel(cantidad);
                return HarmonyFlamesManager.Instance.HarmonyFlames;
        }
        return -1;
    }

    int IManejoVariables.BajarVariable(Variables v, int cantidad)
    {
        switch (v)
        {
            case Variables.HAMBRE:
                HungerManager.Instance.DecreaseHungerLevel(cantidad);
                return HungerManager.Instance.Hungry;
            case Variables.DINERO:
                DecreaseMoneyAmount(cantidad);
                return Money;
            case Variables.FA:
                HarmonyFlamesManager.Instance.DecreaseIntenistyHarmonyFlamesLevel(cantidad);
                return HarmonyFlamesManager.Instance.HarmonyFlames;

        }
        return -1;
    }
    #endregion


    public override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        moneyAmountChange(this.Money);

        if (!scene.name.Equals("Town") || !scene.name.Equals("Loading")) {
            EstablecerHerramienta();
        }
       
    }
}
