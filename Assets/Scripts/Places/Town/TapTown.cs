using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TapTown : Tap
{
	// Attributes
	#region Attributes
	public List<string> tags;
	public Animator animatorUIPlaceClosed;
	private bool isAnimatingUIPlace;
    public Text txtNombreLugar;
	#endregion

	// Methods
	#region Methods
	public override T TapGameobject<T> (T parameter)
	{
		if (parameter.GetType () == typeof(GameObject)) {

			GameObject go = parameter as GameObject;

			#region Tag: Place
			// Tag: Place
			if (go.tag.Equals (tags[0])) {

				if (!isAnimatingUIPlace) {
					// Verificar el estado del lugar
					Place place = go.GetComponent<Place> ();
					switch (place.GetStatePlace ()) {
					case PlaceState.Open:
                            SoundManager.PlayAndThenInvoke(this, 0, () => SceneLoadManager.Instance.CargarEscena(place.GetNamePlace().ToString()));
						//SceneManager.LoadScene(place.GetNamePlace().ToString());
                            
						Debug.Log ("PlaceState: Open... ir a escena");
						break;
					case PlaceState.Close:
                            SoundManager.PlaySound(this, 1);
						animatorUIPlaceClosed.GetComponentInChildren<Text>().text = place.closeMessage;
                            txtNombreLugar.text = place.nombre;
						Debug.Log ("PlaceState: Close... tocar otro lugar");
						isAnimatingUIPlace = true;
						AnimUIPlaceClose (true);
						AnimUIPlaceClose (false, 2.0f);
						break;
					}
				}
			}
			#endregion

			// All collider elements
		}

		return default(T);
	}

	void AnimUIPlaceClose(bool anim) {
		animatorUIPlaceClosed.SetBool ("UIPlaceClosed", anim);
	}

	void AnimUIPlaceClose(bool anim, float time) {
		StartCoroutine (AnimUIPlaceCloseWithDelay(anim, time));
	}

	IEnumerator AnimUIPlaceCloseWithDelay(bool anim, float time) {
		yield return new WaitForSecondsRealtime (time);
		AnimUIPlaceClose (anim);
		yield return new WaitForSecondsRealtime (animatorUIPlaceClosed.GetCurrentAnimatorStateInfo(0).length);
		isAnimatingUIPlace = false;
	}
	#endregion
}

