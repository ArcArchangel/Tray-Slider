using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridTilePrefab;
    public int width = 10;
    public int height = 10;
    public float spacing = 1f;

    public static List<Vector3> SnapPoints = new List<Vector3>();
    public static bool[,] OccupiedTiles;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        SnapPoints.Clear();
        OccupiedTiles = new bool[width, height];

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 spawnPos = new Vector3(x * spacing, 0, z * spacing);
                Instantiate(gridTilePrefab, spawnPos, Quaternion.identity, transform);
                SnapPoints.Add(spawnPos);
            }
        }
    }

    public static Vector2Int WorldToGridIndex(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x);
        int z = Mathf.RoundToInt(worldPos.z);
        return new Vector2Int(x, z);
    }

    public static Vector3 GridToWorldPos(Vector2Int index)
    {
        return new Vector3(index.x, 0, index.y);
    }

    public static bool IsTileOccupied(Vector2Int index)
    {
        if (index.x < 0 || index.x >= OccupiedTiles.GetLength(0) ||
            index.y < 0 || index.y >= OccupiedTiles.GetLength(1))
        {
            return true;
        }
        return OccupiedTiles[index.x, index.y];
    }

    public static void SetTileOccupied(Vector2Int index, bool occupied)
    {
        if (index.x >= 0 && index.x < OccupiedTiles.GetLength(0) &&
            index.y >= 0 && index.y < OccupiedTiles.GetLength(1))
        {
            OccupiedTiles[index.x, index.y] = occupied;
        }
    }
}
