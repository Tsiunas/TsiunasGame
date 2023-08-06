using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hoe : Tool {

	public override string GetName {
		get {
			return typeof(Hoe).ToString ();
		}
	}

    public override ToolType ToolType
    {
        get
        {
            return ToolType.Hoe;
        }
    }

    public Hoe(int durability, int cost, bool goldTool, ToolTarget toolTarget, ToolType toolType, params TargetType[] targetsType) : base(durability, cost, goldTool, toolTarget, toolType, targetsType)
    {
		this.Name = this.GetName;
		this.Durability = durability;
        this.PurchasePrice = cost;
        this.Gold = goldTool;
		this.toolTarget = toolTarget;
		
        this.targetsTypes = new List<TargetType>(targetsType);
	}
    public Hoe():this(50,35,false, null, ToolType.Hoe, TargetType.GROUND)
    {

    }
	public override void ValidatedTarget (ToolTarget target)
	{
        switch (target.targetType)
        {
            case TargetType.GROUND:
                if (target.IsPlowable)
                {
                    if (!target.IsPlowed)
                    {
                        DecreaseDurability();
                        target.DecreaseResistance();
                        target.IsPlowed = true;
                        target.IsPlowable = false;
                        //target.ChangeSprite(true);
                    }
                }               
                break;
            case TargetType.PLANT:
                HarvestPlant(target);
                break;
        }
        target.PlaySoundValidatedTarget(ToolHelp.Sounds.hoe);
    }

    protected override bool IsValidTarget(ToolTarget toolTarget)
    {
        bool valid = false;
        switch (toolTarget.targetType)
        {
            case TargetType.PLANT:
                valid = toolTarget.IsPlowed;
                break;
            case TargetType.GROUND:
                valid = toolTarget.IsPlowable;
                break;
        }

        return valid;

    }
}
