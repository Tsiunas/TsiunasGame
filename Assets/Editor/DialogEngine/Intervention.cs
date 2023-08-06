using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Intervention", menuName = "Dialog Engine/Intervention", order = 1)]
public class Intervention : ScriptableObject
{
	public List<Line> lines;
	public List<LineOption> options;
}

