using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hammer : Tool {

	public override string GetName {
		get {
			return typeof(Hammer).ToString ();
		}
	}

    public override ToolType ToolType
    {
        get
        {
            return ToolType.Hammer;
        }
    }

    public Hammer(int durability, int cost, bool goldTool, ToolTarget toolTarget, ToolType toolType, params TargetType[] targetsType) : base(durability, cost, goldTool, toolTarget, toolType, targetsType)
    {
        this.Name = this.GetName;
        this.Durability = durability;

        this.Gold = goldTool;
		this.toolTarget = toolTarget;
		
        this.targetsTypes = new List<TargetType>(targetsType);
    }

	public override void ValidatedTarget (ToolTarget target)
	{
        switch (target.targetType)
        {
            case TargetType.PLANT:
                HarvestPlant(target);
                break;
            case TargetType.ROCK:
                DecreaseDurability();
                target.DecreaseResistance();
                break;
        }
        target.PlaySoundValidatedTarget(ToolHelp.Sounds.hammer);
    }
}
