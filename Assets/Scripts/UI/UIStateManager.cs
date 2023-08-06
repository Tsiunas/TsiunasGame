using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIStates { UIMain, UIMainMenu }
public class UIStateManager : MonoBehaviour {
	// Attributes
	#region Attributes
	// Singleton Pattern
	#region Singleton Pattern
	private static UIStateManager instance;

	public static UIStateManager Instance
	{
		get
		{
			if(instance == null)
			{
				GameObject UIManager = new GameObject("UIStateManager");
				DontDestroyOnLoad(UIManager);
				instance = UIManager.AddComponent<UIStateManager>();
			}

			return instance;
		}
	}
	#endregion

	public UIStates currentUIState;

	public delegate void OnChangeUIState(UIStates uiCurrent);
	public event OnChangeUIState onChangeUIState;
	#endregion

	// Methods
	#region Methods
	void Start() {
		currentUIState = UIStates.UIMain;
		SetUIState(currentUIState, 0.1f);
	}

	/// <summary>
	/// Cambia la interfaz por la de la siguiente pantalla
	/// </summary>
	/// <param name="uiStateReceived"> La pantalla que deseamos cargar.</param>
	public void SetUIState(UIStates uiStateReceived)
	{
		// Obtenemos la pantalla actual
		UIStates tempUIState = currentUIState;
		// Si la pantalla actual es diferente de la que se pasa traves del parámetro entonces
		if(uiStateReceived != tempUIState)
		{
			// Seteo la pantalla actual con un nuevo valor
			currentUIState = uiStateReceived;
			// Disparo el evento que de cambio de pantallas
			if(onChangeUIState != null)
				onChangeUIState(currentUIState);
		}
		else
		{
			// Disparo el evento que de cambio de paso
			if(onChangeUIState != null)
				onChangeUIState(currentUIState);

		}

	}

	public IEnumerator SetUIStateWithDelay(UIStates uiStateReceived, float delay) {
		yield return new WaitForSeconds(delay);
		// Obtenemos la pantalla actual
		UIStates tempUIState = currentUIState;
		// Si la pantalla actual es diferente de la que se pasa traves del parámetro entonces
		if(uiStateReceived != tempUIState)
		{
			// Seteo la pantalla actual con un nuevo valor
			currentUIState = uiStateReceived;
			// Disparo el evento que de cambio de pantallas
			if(onChangeUIState != null)
				onChangeUIState(currentUIState);
		}
		else
		{
			// Disparo el evento que de cambio de paso
			if(onChangeUIState != null)
				onChangeUIState(currentUIState);

		}
	}

	public void SetUIState(UIStates stateReceived, float delay)
	{
		StartCoroutine(SetUIStateWithDelay(stateReceived, delay));
	}
	#endregion
}