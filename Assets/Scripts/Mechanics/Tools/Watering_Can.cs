using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;

[System.Serializable]
public class Watering_Can : Tool
{

	public override string GetName {
		get {
			return typeof(Watering_Can).ToString ();
		}
	}

    public override ToolType ToolType
    {
        get
        {
            return ToolType.Watering_Can;
        }
    }

    public Watering_Can(int durability, int cost, bool goldTool, ToolTarget toolTarget, ToolType toolType, params TargetType[] targetsType) : base(durability, cost, goldTool, toolTarget, toolType, targetsType)
    {
		this.Name = this.GetName;
		this.Durability = durability;
        this.PurchasePrice = cost;
        this.Gold = goldTool;
		this.toolTarget = toolTarget;		
        this.targetsTypes = new List<TargetType>(targetsType);
	}

	public override void ValidatedTarget (ToolTarget target)
	{
        Plant plantToWater = target.GetComponentInChildren<Plant>();
        if (plantToWater.estado < Plant.PlantStates.Madura)
        {
            // Acelera el crecimiento en un 10%
            plantToWater.regada = true;
        }

        if (!target.IsWet)
        {
            DecreaseDurability();
            target.IsWet = true;
            target.ChangeColor(true);
        }
        target.PlaySoundValidatedTarget(ToolHelp.Sounds.watering_can);
	}
}

