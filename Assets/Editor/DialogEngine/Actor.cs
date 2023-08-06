using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Actor", menuName = "Dialog Engine/Actor", order = 1)]
public class Actor : ScriptableObject
{
	public string actorName;
	public Sprite actorResource;
}

