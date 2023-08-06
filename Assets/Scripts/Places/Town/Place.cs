using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enums
#region Enums
public enum PlaceState { Close, Open, UnStated }
#endregion

public class Place : MonoBehaviour {

	// Attributes
	#region Attributes
	private PlaceState placeState;
	public NamePlacesTown placeName;
	public delegate void OnChangePlaceState(PlaceState changedState);
	public event OnChangePlaceState onChangePlaceState;
	public string closeMessage = "¡Esta cerrado!";
    public string nombre;
	#endregion

	// Methods
	#region Methods
	/// <summary>
	/// Regresa la constante de la enumeración: PlaceState, referente al estado del lugar
	/// </summary>
	/// <returns>The state place.</returns>
	public PlaceState GetStatePlace() {
		return this.placeState;
	}

	/// <summary>
	/// Regresa la constante de la enumeración: NamePlacesTown, referente al nombre del lugar
	/// </summary>
	/// <returns>The name place.</returns>
	public NamePlacesTown GetNamePlace() {
		return this.placeName;
	}

	void Start() {
        if (GameManager.Instance.GetGameState == GameState.InGame)
        {
            if (this.placeName == NamePlacesTown.Store)
                this.placeName = NamePlacesTown.tiendaDonJorge;
            if (this.placeName == NamePlacesTown.Agroinsumos)
                this.placeName = NamePlacesTown.ElRefugio;
        }
		SetStatePlace(Util.GetPlaceState (this.placeName, this.placeState));
	}
	/// <summary>
	/// Establece el estado del lugar
	/// Dispara el evento de cambio de estado: onChangePlaceState
	/// </summary>
	/// <param name="_state">Estado actual</param>
	public void SetStatePlace(PlaceState _state) {		
		this.placeState = _state;
		if (onChangePlaceState != null)
			onChangePlaceState (this.placeState);
	}
	#endregion
}
