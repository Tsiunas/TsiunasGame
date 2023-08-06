using UnityEngine;

public class CameraHandlerXAxis : CameraHandler
{
    // Attributes
    #region Attributes
    private Vector3 lastPanPositionXAxis;
    private static readonly float PanSpeed = 10f;
    private int panFingerIdXAxis; 
    #endregion

    // Methods
    #region Methods
    public override void Start() { }

    public override void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0) && !Util.IsPointerOverUIObject())
        {
            lastPanPositionXAxis = Input.mousePosition; 
        }
        else if (Input.GetMouseButton(0) && !Util.IsPointerOverUIObject())
        {
            beginningPanning |= lastPanPositionXAxis != Input.mousePosition;
            if (CanPanCamera)
                PanCamera(Input.mousePosition);
        }
    }

    public override void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: 
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began && !Util.IsPointerOverUIObject())
                {
                    lastPanPositionXAxis = touch.position;
                    panFingerIdXAxis = touch.fingerId;                   
                }
                else if (touch.fingerId == panFingerIdXAxis && touch.phase == TouchPhase.Moved && !Util.IsPointerOverUIObject())
                {
                    beginningPanning |= lastPanPositionXAxis != Input.mousePosition;
                    if (base.CanPanCamera)
                        PanCamera(touch.position);
                }
                break;
        }
    }

    public override void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = Camera.main.ScreenToViewportPoint(lastPanPositionXAxis - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, 0, 0);
        // Perform the movement
        transform.Translate(move, Space.World);
        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        transform.position = pos;
        // Cache the position
        lastPanPositionXAxis = newPanPosition;
    }
    #endregion
}
