using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TraySnapper : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SnapToNearestGrid()
    {
        float minDistance = float.MaxValue;
        Vector3 nearestSnapPoint = Vector3.zero;
        Vector2Int nearestGridIndex = Vector2Int.zero;

        foreach (Vector3 snapPoint in GridManager.SnapPoints)
        {
            float dist = Vector3.Distance(transform.position, snapPoint);
            Vector2Int index = GridManager.WorldToGridIndex(snapPoint);

            if (dist < minDistance && !GridManager.IsTileOccupied(index))
            {
                minDistance = dist;
                nearestSnapPoint = snapPoint;
                nearestGridIndex = index;
            }
        }

        // Clear previous tile if already snapped
        Vector2Int currentIndex = GridManager.WorldToGridIndex(transform.position);
        GridManager.SetTileOccupied(currentIndex, false);

        // Move tray to the snap point
        rb.MovePosition(nearestSnapPoint);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Mark the new tile as occupied
        GridManager.SetTileOccupied(nearestGridIndex, true);
    }
}
