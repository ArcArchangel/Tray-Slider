using UnityEngine;

public class TraySwipeMover : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2Int currentGridIndex;
    private GridManager gridManager;
    private bool isTouched = false;
    private Vector2 swipeStartPos;
    private Vector2 swipeEndPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        gridManager = FindObjectOfType<GridManager>();
        currentGridIndex = GridManager.WorldToGridIndex(transform.position);
        GridManager.SetTileOccupied(currentGridIndex, true);
    }

    void Update()
    {
        if (Application.isMobilePlatform)
            HandleTouchInput();
        else
            HandleMouseInput();
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
                    {
                        isTouched = true;
                        swipeStartPos = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                    if (isTouched)
                    {
                        swipeEndPos = touch.position;
                        TryMoveTray();
                        isTouched = false;
                    }
                    break;
            }
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
            {
                isTouched = true;
                swipeStartPos = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0) && isTouched)
        {
            swipeEndPos = Input.mousePosition;
            TryMoveTray();
            isTouched = false;
        }
    }

    void TryMoveTray()
    {
        Vector2 swipeDelta = swipeEndPos - swipeStartPos;
        if (swipeDelta.magnitude < 30f) return;

        Vector2Int direction;

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            direction = swipeDelta.x > 0 ? Vector2Int.right : Vector2Int.left;
        else
            direction = swipeDelta.y > 0 ? Vector2Int.up : Vector2Int.down;

        Vector2Int targetIndex = currentGridIndex + direction;

        if (GridManager.IsTileOccupied(targetIndex)) return;

        GridManager.SetTileOccupied(currentGridIndex, false);
        GridManager.SetTileOccupied(targetIndex, true);
        currentGridIndex = targetIndex;

        Vector3 targetPos = GridManager.GridToWorldPos(targetIndex);
        rb.MovePosition(new Vector3(targetPos.x, transform.position.y, targetPos.z));
    }
}
