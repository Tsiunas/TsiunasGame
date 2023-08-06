using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialog Engine/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
	public List<Intervention> interventions;
}

