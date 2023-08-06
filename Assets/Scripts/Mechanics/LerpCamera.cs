using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpCamera : MonoBehaviour {

	// Attributes
	#region Attributes
	#region Code Lerp
	private float timeTakenDuringLerp;
	private float distanceToMove = 1;
	private bool _isLerping;
	private Vector3 _startPosition;
	private Vector3 _endPosition;
	private float _timeStartedLerping;
	private float timeSinceStarted; 
	private float percentageComplete;
	public delegate void OnLerpFinished();
	public event OnLerpFinished lerpFinished;
	#endregion
	#endregion

	// Methods
	#region Methods
	// Use this for initialization

	/// <summary>
	/// Se usa para realizar interpolación de la cámara 
	/// </summary>
	/// <param name="endPos">Posición a donde se quiere dirigir</param>
	/// <param name="timeLerp">Tiempo que debe tomar la interpolación</param>
	public void CreateLerpCamera(Vector2 endPos, float timeLerp) {
		_startPosition = this.transform.position;
		_endPosition = new Vector3(endPos.x, this.transform.position.y, endPos.y) + Vector3.forward * distanceToMove;
		_timeStartedLerping = Time.time;
		timeTakenDuringLerp = timeLerp;
		_isLerping = true;
	}

	// Update is called once per frame
	void LateUpdate () {
		if (_isLerping) {
			float timeSinceStarted = Time.time - _timeStartedLerping;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

			this.transform.position = Vector3.Lerp (_startPosition, _endPosition, percentageComplete);

			if (percentageComplete >= 1.0f) {
				_isLerping = false;
				Debug.Log ("Termino la animación");
				if (lerpFinished != null)
					lerpFinished ();
			}
		}

	}
	#endregion

}
