using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LerpUIElement : MonoBehaviour {

	// Attributes
	#region Attributes
	public Vector2 finalPos;
	private RectTransform rTransform;

	#region Code Lerp
	public float timeTakenDuringLerp;
	public float distanceToMove;
	private bool _isLerping;
	private Vector3 _startPosition;
	private Vector3 _endPosition;
	private float _timeStartedLerping;
	private float timeSinceStarted; 
	private float percentageComplete;

    public Action OnEndLerp = delegate { };

    private Image _image;
	#endregion
	#endregion

	// Methods
	#region Methods
    public void SetSprite(Sprite spriteToAsign) {
        this._image.sprite = spriteToAsign;
        this._image.preserveAspect = true;
    }

    private void Awake()
    {
        this._image = GetComponent<Image>();
    }
    // Use this for initialization
    public virtual void Start () {
        
		rTransform = GetComponent<RectTransform> ();
		_isLerping = true;
		_timeStartedLerping = Time.time;
		_startPosition = new Vector3(rTransform.anchoredPosition.x, rTransform.anchoredPosition.y, 0);
		_endPosition = new Vector3(finalPos.x, finalPos.y, 0) + Vector3.forward * distanceToMove;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (_isLerping) {
			 timeSinceStarted = Time.time - _timeStartedLerping;
			 percentageComplete = timeSinceStarted / timeTakenDuringLerp;

			rTransform.anchoredPosition = Vector3.Lerp (_startPosition, _endPosition, percentageComplete);

			if (percentageComplete >= 1.0f) {
				_isLerping = false;
                if (OnEndLerp != null)
                    OnEndLerp();
            }
        }
    }

    public void DestroyLerpUIElement() {
        Destroy(this.gameObject);
    }
	#endregion
}
