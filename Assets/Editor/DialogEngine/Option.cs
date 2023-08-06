using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Option", menuName = "Dialog Engine/Option", order = 1)]
public class Option : ScriptableObject
{
	public string sentence;
	public List<Consequence> consequences;
}

