using System.Collections.Generic;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIDragGameElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField]
    private GameObject iconPrefab;
    [HideInInspector]
    public GameObject hoverObject;
    protected Transform canvas;

   
    public virtual void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
    }

    public abstract void DropInStomach();

    #region IBeginDragHandler implementation
    public virtual void BeginDrag(PointerEventData eventData)
    {

        hoverObject = (GameObject)Instantiate(iconPrefab);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (GameManager.Instance.GetGameState == GameState.InGame)
            if (FindObjectOfType<CameraHandler>() != null)
                FindObjectOfType<CameraHandler>().CanPanCamera = false;
        hoverObject.GetComponent<Image>().sprite = GetComponent<UIElementoJuego>().icon.sprite;
        hoverObject.transform.SetParent(canvas, false);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDrag(eventData);
    }
    #endregion

    #region IDragHandler implementation
    public virtual void OnDrag(PointerEventData eventData)
    {
        
        hoverObject.transform.position = eventData.position;
    }

    #endregion

    #region IEndDragHandler implementation
    public virtual void EndDrag(PointerEventData eventData)
    {

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (GameManager.Instance.GetGameState == GameState.InGame)
            if (FindObjectOfType<CameraHandler>() != null)
                FindObjectOfType<CameraHandler>().CanPanCamera = true;
        Destroy(hoverObject);

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);


        if (results.Count > 0)
        {
            if (results[0].gameObject.tag.Equals(StringsHelper.Tags.HUNGRY))
            {
                //Comer la comida
                HungerManager.EatFood(GameManager.GetEatable(this));
                //DropInStomach();
                // Reproduce sonido de comer
                
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag(eventData);
    }

    protected Component DetectComponentRequired(PointerEventData data)
    {
        Ray ray = Camera.main.ScreenPointToRay(data.position);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.transform.Check<PNJActor>() != null)
                return hit.collider.gameObject.transform.Check<PNJActor>();
        }

        RaycastHit hit3D;
        Ray ray3D = Camera.main.ScreenPointToRay(data.position);
        if (Physics.Raycast(ray3D, out hit3D, 100.0f))
        {
            if (hit3D.collider != null) {
                if (hit3D.collider.gameObject.transform.Check<ToolTarget>() != null)
                    return hit3D.collider.gameObject.transform.Check<ToolTarget>();
            }
        }
        return null;
    }


    #endregion
}

