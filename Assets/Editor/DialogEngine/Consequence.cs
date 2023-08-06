using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Consequence", menuName = "Dialog Engine/Consequence", order = 1)]
public abstract class Consequence : ScriptableObject
{
	public abstract void ExecuteConsequence ();
}

