using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TapFarm : Tap {
	// Attributes
	#region Attributes
	public List<string> tags;
	#endregion

	// Methods
	#region Methods
	public override T TapGameobject<T> (T parameter)
	{
		Debug.Log ("Parameter: " + parameter);
		if (parameter.GetType () == typeof(GameObject)) {			
			GameObject go = parameter as GameObject;
			// Tag: tsiuna
			if (go.tag.Equals (tags[0])) {
				Destroy (go);
				NotificationCenter.DefaultCenter().PostNotification(this, "DragTsiuna", new object [] { Camera.main.WorldToScreenPoint(go.transform.position), Input.mousePosition });
				// TrackerSystem.Instance.SendTrackingData("usuario", "Recolecto Tsiuna", "botón", "éxito");
			}
			// All collider elements
			// Tag: Door
			if (go.tag.Equals(tags[1])) {
				Util.SetPlaceState (NamePlacesTown.Farm, PlaceState.Close);
                Util.SetPlaceState(NamePlacesTown.Agroinsumos, PlaceState.Close);
				Util.SetPlaceState (NamePlacesTown.Store, PlaceState.Open);
				
				SceneManager.LoadScene ("Town");
			}
		}
		return default(T);
	}
	#endregion
}
