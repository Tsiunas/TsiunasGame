using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateController : MonoBehaviour {

	// Attributes
	#region Attributes
	public GameObject [] prefabsUIStates;
	private GameObject containerUIState;
	private GameObject tempPrefab;
	public static GameObject tempPrefabStatic;
	#endregion

	// Methods
	#region Methods
	// Use this for initialization
	void Start () {
        DebugGlobals.canDebug = false;
		UIStateManager.Instance.onChangeUIState += eventOnChangeUIState;
	}

	void eventOnChangeUIState (UIStates currentUI)
	{
		InitUIState(currentUI.ToString());
	}

	void InitUIState(string prefabToInstantiate)
	{
		containerUIState = GameObject.Find("Canvas/ContainerUI");

		for (int i = 0; i < prefabsUIStates.Length; i++)
		{
			if(prefabsUIStates[i].name == prefabToInstantiate)
			{
				tempPrefab = prefabsUIStates[i];
			}
		}

		// Si tiene prefabs instanciados
		if(containerUIState.transform.childCount > 0)
		{
			if(tempPrefabStatic.name != prefabToInstantiate + "(Clone)")
			{
				// Los elimino todos
				foreach (Transform child in containerUIState.transform)
				{
					GameObject.Destroy(child.gameObject);
				}

				// Luego de eliminar los anteriores creo el nuevo
				tempPrefabStatic = Instantiate(tempPrefab);
				tempPrefabStatic.transform.SetParent(containerUIState.transform, false);
			}
		}
		else
		{			
			tempPrefabStatic = Instantiate(tempPrefab);
			tempPrefabStatic.transform.SetParent(containerUIState.transform, false);
		}
	}
	#endregion
}
