using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Tsiunas.Mechanics;


public class UISeed : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //El tipo de la semilla
    public Plant.PlantTypes tipo;
        
    // Attributes
    #region Attributes
    
    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;
    public GameObject fertileTile;
    private GameObject replacementSeed;
    
    #endregion

    // Methods
    #region Methods
    // IBeginDragHandler
    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        replacementSeed = Instantiate(this.gameObject, transform.parent);
        replacementSeed.name = "UISeed";
        FindObjectOfType<CameraHandler>().CanPanCamera = false;
    }
    #endregion

    // IDragHandler
    #region IDragHandler implementation
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<ToolTarget>() != null)
                {
                    fertileTile = hit.collider.gameObject;
                }
                else
                {
                    fertileTile = null;
                }
            }
        }
    }
    #endregion

    // IEndDragHandler
    #region IEndDragHandler implementation
    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;

        if (fertileTile)
        {
            ToolTarget target = fertileTile.GetComponent<ToolTarget>();

            if (target.targetType == TargetType.GROUND)
            {
                if (!target.IsPlanted && target.IsPlowed)
                {
                    target.IsPlanted = true;
                    target.ChangeType(TargetType.PLANT);

                    startParent = fertileTile.transform;
                    Debug.Log("Ferile: " + startParent.name);
                    NotificationCenter.DefaultCenter().PostNotification(this, "Sow", fertileTile);
                    Destroy(this.gameObject);

                }
                else
                    transform.position = startPosition;
            }
            else
                transform.position = startPosition;
        }

        if (transform.parent == startParent)
        {
            Destroy(replacementSeed.gameObject);
            transform.position = startPosition;
            Debug.Log("regreso: " + startParent.name);
        }

        FindObjectOfType<CameraHandler>().CanPanCamera = true;
    }
    #endregion
    #endregion
}
