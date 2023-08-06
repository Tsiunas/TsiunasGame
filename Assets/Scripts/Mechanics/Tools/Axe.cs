using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Axe : Tool
{
	public override string GetName {
		get {
			return typeof(Axe).ToString ();
		}
	}

    public override ToolType ToolType
    {
        get
        {
            return ToolType.Axe;
        }
    }

    public Axe(int durability, int cost, bool goldTool, ToolTarget toolTarget, ToolType toolType, params TargetType[] targetsType) : base(durability, cost, goldTool, toolTarget, toolType, targetsType)
    {
		this.Name = this.GetName;
		this.Durability = durability;
        this.PurchasePrice = cost;
        this.Gold = goldTool;
		this.toolTarget = toolTarget;		
        this.targetsTypes = new List<TargetType>(targetsType);
	}

    /// <summary>
    /// Método abstracto que debe ser implementado por cada clase derivada y se llama cuando se toca el objetivo correcto
    /// </summary>
    /// <param name="target">Objetivo tocado en la granja.</param>
    public override void ValidatedTarget (ToolTarget target)
	{
        switch (target.targetType)
        {
            case TargetType.PLANT:
                HarvestPlant(target);
                break;
            case TargetType.TREE: case TargetType.CUTTRUNK:
                DecreaseDurability();               
                target.DecreaseResistance();
                break;
        }
        target.PlaySoundValidatedTarget(ToolHelp.Sounds.axe);
    }
}

