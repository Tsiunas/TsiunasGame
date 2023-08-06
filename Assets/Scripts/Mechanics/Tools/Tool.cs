using System;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using UnityEngine;
using UnityEngine.UI;

public enum ToolType { None, Hoe, Hammer, Axe, Machete, Watering_Can, Hand };
[System.Serializable]
public abstract class Tool : GameElement, IPurchase, IGold
{
    // Attributes
    #region Attributes
    private int price;
    private bool gold;
    [NonSerialized]
    protected ToolTarget toolTarget;
    
    public abstract ToolType ToolType { get; }
    
    public List<TargetType> targetsTypes;

    [field: NonSerialized]
    public event Action<int> OnDurabilityChange = delegate { };
    #endregion


    #region Properties
    public abstract string GetName
    {
        get;
    }

    public string Name
    {
        set { if (_name == null) this._name = value; else throw new InvalidOperationException(GetType().ToString() + " has already been established"); }
    }

    public int Durability
    {
        get
        {
            return this._durability;
        }
        set
        {
            _durability = value;
        }
    }

    public int PurchasePrice
    {
        get { return price; }
        set { price = value; }
    }

    public bool Gold
    {
        get { return gold; }
        set { gold = value; }
    }
    #endregion

    // Constructor
    #region Constructor
    protected Tool(int durability, int cost, bool goldTool, ToolTarget toolTarget, ToolType toolType, params TargetType[] targetsType)
    {
        this._durability = durability;
        this.price = cost;
        this.gold = goldTool;
        this.toolTarget = toolTarget;
        
        this.targetsTypes = new List<TargetType>(targetsType);
        this._filledDurability = durability;
    }

    internal void ClearEventOnDurabilityChange()
    {
        OnDurabilityChange = delegate { };
    }
    #endregion

    // Methods
    #region Methods


    public void SetToolTarget(ToolTarget _toolTarget)
    {
        this.toolTarget = _toolTarget;
    }

    /// <summary>
    /// Método abstracto que debe ser implementado por cada clase derivada y se llama cuando se toca el objetivo correcto
    /// </summary>
    /// <param name="target">Objetivo tocado en la granja.</param>
    public abstract void ValidatedTarget(ToolTarget target);

    /// <summary>
    /// Se usa para determinar que se toco un objetivo en la granja y que su tipo es igual al de esta herramienta
    /// </summary>
    /// <param name="_toolTarget">Objetivo tocado en la granja.</param>
    public virtual bool Use(ToolTarget _toolTarget)
    {
        // asignamos el objetivo tocado en la granja en una variable local
        this.toolTarget = _toolTarget;
        // comprobamos que se haya tocado un elemento válido en la granja
        // se puede hacer tap en cualquier parte, pero solo los objetivos de las herramientas son válidos
        if (this.toolTarget == null)
           { TrackerSystem.Instance.SendTrackingData("user", "interacted", "screen", "screen|game|fallo");
           TrackerSystem.Instance.SendTrackingData("user", "used", "tool", "undefined|undefined|éxito");
            return false;}

        if (this.targetsTypes.Exists((TargetType currentTargetType) => currentTargetType == this.toolTarget.targetType) && IsValidTarget(_toolTarget))
        {
            // Si es un objetivo válido, haga la llamada a ValidatedTarget
            ValidatedTarget(_toolTarget);
            TrackerSystem.Instance.SendTrackingData("user", "interacted", "screen", "screen|game|éxito");
            TrackerSystem.Instance.SendTrackingData("user", "used", "tool", this.ToolType+"|"+this.toolTarget.Name+"|éxito");
            OnDurabilityChange(this.Durability);
            GameObject outGO;
            Util.FeedbackToolUsed(_toolTarget, this.ToolType);
            Util.CreateUIWithAnyPosition((GameObject)Resources.Load("Prefabs/Spark"), _toolTarget.transform.position, 0, out outGO, _toolTarget.gameObject);
            //Cada vez que se usa una herramienta baja el Hambre
            HungerManager.Instance.DecreaseHungerLevel(2);
            return true;
        }
        else
        {
            // Si NO es un objetivo válido: ¡Feedback de objetivo incorrecto!
            TrackerSystem.Instance.SendTrackingData("user", "interacted", "screen", "screen|game|fallo");
            TrackerSystem.Instance.SendTrackingData("user", "used", "tool", this.ToolType+"|"+_toolTarget.Name+"|fallo");
            _toolTarget.ExecuteFeedbackToolTargetWrong();
            return false;
        }

    }
    /// <summary>
    /// Método de ayuda virtual para que la herramienta revise si el target es válido.
    /// </summary>
    /// <param name="toolTarget"></param>
    /// <returns></returns>
    protected virtual bool IsValidTarget(ToolTarget toolTarget)
    {
        return true;
    }

    public GameElement Purchase()
    {
        return this;
    }

    protected void HarvestPlant(ToolTarget target)
    {
        if (target.IsPlowed)
        {
            Plant plantToHarvest = target.GetComponentInChildren<Plant>();
            Cosecha cosechaObtenida = plantToHarvest.DarCosecha();

            StaticCoroutine.DoCoroutine(Util.CreateUILerpGameElement(target.gameObject,
                                         GameObject.Find("Canvas/Inventories/InventoryFruits/PlaceholderFoodActive"),
                                         TexturesManager.Instance.GetSpriteFromSpriteSheet(cosechaObtenida.tipoDelFruto.ToString() + "Fruit"),
                                                                     cosechaObtenida.cantidadFruto, delegate { StoreManager.ObtainFruit(cosechaObtenida.tipoDelFruto, 1); }, 0.1f, true));


            StaticCoroutine.DoCoroutine(Util.CreateUILerpGameElement(target.gameObject,
                                                                     GameObject.Find("Canvas/Inventories/InventorySeeds/PlaceholderSeedActive"),
                                         TexturesManager.Instance.GetSpriteFromSpriteSheet(cosechaObtenida.tipoDeSemilla.ToString() + "Seed"),
                                                                     cosechaObtenida.cantidadSemilla, delegate { StoreManager.ObtainSeed(cosechaObtenida.tipoDeSemilla, 1); }, 0.1f, true));


            // TODO: verificar estado de marchitación de la planta para poder cosecharla o no
            target.IsPlowed = false;
            target.IsPlanted = false;
            target.IsWet = false;
            target.DecreaseResistance();
            target.IsPlowable = true;
        }
    }


    #endregion
}
