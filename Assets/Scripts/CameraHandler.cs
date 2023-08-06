using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System;

public class CameraHandler : MonoBehaviour
{

    // Attributes
    #region Attributes
    //public float[] BoundsY = new float[] { -2.5f, 2.5f };
    //public float[] BoundsX = new float[] { -2.5f, 2.5f };

    [SerializeField]
    private float[] BoundsZ;

    public float[] BoundsX;

    private Vector3 lastPanPosition;
    private static readonly float PanSpeed = 10f;
    private int panFingerId;

    [SerializeField]
    private bool canPanCamera;

    protected bool beginningPanning;
    // agrupa los layer 8 y 9 (solo sobre estos se podra hacer tag)
    int layerElementsMask = (1 << 8) | ( 1 << 9);
    public Tap tapController;
    #endregion

    // Properties
    #region Properties
    public bool CanPanCamera
    {
        get
        {
            return canPanCamera;
        }
        set
        {
            canPanCamera = value;
        }
    }

    public bool BeginningPanning
    {
        get
        {
            return beginningPanning;
        }
    }
    #endregion

    // Methods
    #region Methods
    public virtual void Start()
    {
        GetTapController();
    }

    /// <summary>
    /// Establece la clase derivada de Tap en el objeto tapController (la implementada en cada escena), dependiendo del nombre de la escena activa actualmente
    /// </summary>
    void GetTapController()
    {
        if (tapController == null)
            tapController = (Tap)FindObjectOfType(Type.GetType("Tap" + SceneManager.GetActiveScene().name));
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            HandleTouch();
        }
        else
        {
            HandleMouse();
        }
    }
    public virtual void HandleMouse()
    {

        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0) && !Util.IsPointerOverUIObject())
        {
            lastPanPosition = Input.mousePosition; 
        }
        else if (Input.GetMouseButton(0) && !Util.IsPointerOverUIObject())
        {
            beginningPanning |= lastPanPosition != Input.mousePosition;
            if (canPanCamera)
                PanCamera(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layerElementsMask))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.red, 100.0f);
                if (hit.collider != null)
                {
                    if (!beginningPanning)
                        tapController.TapGameobject(hit.collider.gameObject);
                }
            }

            beginningPanning = false;
        }

    }

    public virtual  void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Panning
                    // If the touch began, capture its position and its finger ID.
                    // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began && !Util.IsPointerOverUIObject())
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;                   
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved && !Util.IsPointerOverUIObject())
                {
                    beginningPanning |= lastPanPosition != Input.mousePosition;
                    if (canPanCamera)
                        PanCamera(touch.position);
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Ended)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100.0f, layerElementsMask))
                    {
                        if (hit.collider != null)
                        {
                            if (!beginningPanning)
                                tapController.TapGameobject(hit.collider.gameObject);
                        }
                    }
                    beginningPanning = false;
                }
                break;
        }
    }

    public virtual void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = Camera.main.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);
        // Perform the movement
        transform.Translate(move, Space.World);
        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(transform.position.z, BoundsZ[1], BoundsZ[0]);
        transform.position = pos;
        // Cache the position
        lastPanPosition = newPanPosition;
    }
    #endregion
}
