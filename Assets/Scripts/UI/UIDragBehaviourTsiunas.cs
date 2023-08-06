using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragBehaviourTsiunas : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	// Attributes
	#region Attributes
	public static GameObject itemBeingDragged;
	Vector3 startPosition;
	Transform startParent;
	public GameObject fertileTile;
	#endregion

	// Methods
	#region Methods
	// IBeginDragHandler
	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
	}
	#endregion

	// IDragHandler
	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{
		transform.position = eventData.position;

		RaycastHit hit; 
		Ray ray = Camera.main.ScreenPointToRay (eventData.position); 
		if (Physics.Raycast (ray, out hit, 100.0f)) {
			if (hit.collider != null) {

				if (hit.collider.tag.Equals ("fertile")) {
					fertileTile = hit.collider.gameObject;
				} else {
					fertileTile = null;
				}
			}
		}
	}
	#endregion

	// IEndDragHandler
	#region IEndDragHandler implementation
	public void OnEndDrag (PointerEventData eventData)
	{		
		itemBeingDragged = null;

		if (fertileTile) {
			if (fertileTile.transform.childCount == 0) {
				startParent = fertileTile.transform;
				Debug.Log ("Ferile: " + startParent.name);
				NotificationCenter.DefaultCenter ().PostNotification (this, "SeedTsiuna", fertileTile);
				Destroy (this.gameObject);
			} else {
				transform.position = startPosition;
				Debug.Log ("regreso: " + startParent.name);
			}
		}

		if(transform.parent == startParent){
			transform.position = startPosition;
			Debug.Log ("regreso: " + startParent.name);
		}
	}
	#endregion
	#endregion
}
