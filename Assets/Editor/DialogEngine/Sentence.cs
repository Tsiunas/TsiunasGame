using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Sentence", menuName = "Dialog Engine/Sentence", order = 1)]
public class Sentence : ScriptableObject
{
	public string sentence;
	public Consequence consecuence;

}

