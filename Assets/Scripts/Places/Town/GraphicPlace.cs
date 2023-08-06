using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GraphicPlace : MonoBehaviour
{
	// Attributes
	#region Attributes
	private Image _spriteR;
	public List<Color> colorState;
	private Place _place;
	#endregion

	// Methods
	#region Methods
	void Awake () {
		_spriteR = GetComponent<Image> ();
		_place = GetComponent<Place> ();
		_place.onChangePlaceState += SetColorStatePlace;
	}

	void Start() {		
		SetColorStatePlace (_place.GetStatePlace());
	}

	/// <summary>
	/// Establece el color del componente SpriteRenderer dependediendo del estado del lugar (abierto o cerrado)
	/// </summary>
	/// <param name="_state">Estado del lugar</param>
	void SetColorStatePlace (PlaceState _state)
	{
		this._spriteR.color = _state == PlaceState.Close ? colorState [0] : colorState [1];
	}
	#endregion
}

