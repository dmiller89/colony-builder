using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    Tile.TileType buildModeTile = Tile.TileType.Grass;

    [Header("Zoom")]
    public float zoomSpeed = 1;
    public int zoomMin = 5;
    public int zoomMax = 20;

    [Header("Pan")]
    public float panSpeed = 0.05f;
    public float panMultiplier = 2f;

    [Header("Misc")]
    public GameObject selectionIndicator;
    private Vector3 cursorPosition;
    private Vector3 lastFramePosition;
    private Tile tileUnderMouse;
    private Vector3 dragStartPosition;
    private List<GameObject> selectionIndicators = new List<GameObject>();

    void Start () 
    {
        
    }

    void Update()
    {
        CameraZoom();
        CameraPan();
        KeyboardPan();
        ControlCursorCircle();
        MouseInteraction();
    }

    void CameraZoom()
    {
        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, zoomMin, zoomMax);
    }

    void CameraPan()
    {
        Vector3 framePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            Vector3 frameDifference = lastFramePosition - framePosition;
            Camera.main.transform.Translate(frameDifference);
        }

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }

    void KeyboardPan()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 panDirection = new Vector3(inputX, inputY, 0f);

        if (panDirection.magnitude > 1f) {
            panDirection.Normalize();
        }

        float newSpeed = panSpeed;

        if (Input.GetKey(KeyCode.LeftShift)) {
            newSpeed *= panMultiplier;
        }

        Camera.main.transform.Translate(panDirection * newSpeed);
    }

    void ControlCursorCircle()
    {
        cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tileUnderMouse = WorldController.Instance.GetTileAtWorldCoord(cursorPosition);
    }

    void MouseInteraction()
    {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            dragStartPosition = cursorPosition;
        }
        
        int startX = Mathf.FloorToInt(dragStartPosition.x);
        int endX = Mathf.FloorToInt(cursorPosition.x);
        int startY = Mathf.FloorToInt(dragStartPosition.y);
        int endY = Mathf.FloorToInt(cursorPosition.y);

        if (endX < startX) {
            int tmp = endX;
            endX = startX;
            startX = tmp;
        }

        if (endY < startY) {
            int tmp = endY;
            endY = startY;
            startY = tmp;
        }

        while (selectionIndicators.Count > 0) {
            GameObject indicator = selectionIndicators[0];
            selectionIndicators.RemoveAt(0);
            Pooler.Despawn(indicator);
        }

        if (Input.GetMouseButton(0)) {
            // display selection indicators
            for (int x = startX; x <= endX; x++) {
                for (int y = startY; y <= endY; y++) {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null) {
                        // show indicator
                        GameObject indicator = Pooler.Spawn(selectionIndicator, new Vector3(x,y,0f), Quaternion.identity);
                        indicator.transform.SetParent(this.transform);
                        selectionIndicators.Add(indicator);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            for (int x = startX; x <= endX; x++) {
                for (int y = startY; y <= endY; y++) {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null) {
                        t.Type = buildModeTile;
                    }
                }
            }
        }
    }

    public void SetMode_CreateTile()
    {
        buildModeTile = Tile.TileType.Grass;
    }

    public void SetMode_DestroyTile()
    {
        buildModeTile = Tile.TileType.Empty;
    }



}
