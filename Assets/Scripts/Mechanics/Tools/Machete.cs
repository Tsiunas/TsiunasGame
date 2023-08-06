using UnityEngine;
using System.Collections;
using Tsiunas.Mechanics;
using System.Collections.Generic;

[System.Serializable]
public class Machete : Tool
{
	public override string GetName {
		get {
			return typeof(Machete).ToString ();
		}
	}

    public override ToolType ToolType
    {
        get
        {
            return ToolType.Machete;
        }
    }

    public Machete(int durability, int cost, bool goldTool, ToolTarget toolTarget, ToolType toolType, params TargetType[] targetsType) : base(durability, cost, goldTool, toolTarget, toolType, targetsType)
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
        


        target.DecreaseResistance();
        DecreaseDurability();
        target.IsPlowed = false;
        target.IsPlanted = false;
        target.IsWet = false;
        target.PlaySoundValidatedTarget(ToolHelp.Sounds.machete);


	}
}

