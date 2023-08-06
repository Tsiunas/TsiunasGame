using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using UnityEngine;

[System.Serializable]
public class Hand : Tool
{
    public override string GetName
    {
        get { return typeof(Hand).ToString(); }
    }

    public override ToolType ToolType
    {
        get
        {
            return ToolType.Hand;
        }
    }

    public Hand(int durability, int cost, bool goldTool, ToolTarget toolTarget, ToolType toolType, params TargetType[] targetsType) : base(durability, cost, goldTool, toolTarget, toolType, targetsType)
    {
        this.Name = this.GetName;
        this.Durability = durability;
        this.Gold = goldTool;
        this.toolTarget = toolTarget;
        
        this.targetsTypes = new List<TargetType>(targetsType);
    }

    public override void ValidatedTarget(ToolTarget target)
    {
        HarvestPlant(target);
        target.PlaySoundValidatedTarget(ToolHelp.Sounds.hand);       
    }
}
