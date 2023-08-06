using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Situation", menuName = "Dialog Engine/Situation", order = 1)]
public class Situation : ScriptableObject
{
	public List<Dialogue> dialogues;
}

