using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChangeWithDelay : MonoBehaviour {
	// Attributes
	#region Attributes
	public UIStates uiStateToGo;
	public float delay;
	#endregion

	// Methods
	#region Methods
	// Use this for initialization
	void Start () {
		UIStateManager.Instance.SetUIState(uiStateToGo, delay);
	}
	#endregion
}
