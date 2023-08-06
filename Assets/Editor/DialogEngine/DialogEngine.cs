using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogEngine", menuName = "Dialog Engine/DialogEngine", order = 1)]
public class DialogEngine : ScriptableObject
{
	public List<Situation> situations;
}

