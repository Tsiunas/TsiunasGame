using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprout : MonoBehaviour {

	public List<Sprite> spriteStates; 
	// Use this for initialization
	public void ChangeSpriteState (bool withered) {
		this.GetComponent<SpriteRenderer> ().sprite = withered ? spriteStates [1] : spriteStates [0];
	}
}
