using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Line", menuName = "Dialog Engine/Line", order = 1)]
public class Line : ScriptableObject {
	public string line;
	public Actor actor;
}