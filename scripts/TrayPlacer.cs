using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayPlacer : MonoBehaviour
{
    public GameObject[] trayPrefabs; // assign your tray prefabs here
    public int[] traySnapIndices;    // index in GridManager.SnapPoints to place trays at

    void Start()
    {
        PlaceTrays();
    }

    void PlaceTrays()
    {
        Debug.Log("SnapPoints Count: " + GridManager.SnapPoints.Count);
    
        if (trayPrefabs.Length != traySnapIndices.Length)
        {
            Debug.LogError("Mismatch between tray prefabs and snap indices.");
           return;
       }
    
        for (int i = 0; i < trayPrefabs.Length; i++)
       {
            if (traySnapIndices[i] >= 0 && traySnapIndices[i] < GridManager.SnapPoints.Count)
            {
                Vector3 spawnPosition = GridManager.SnapPoints[traySnapIndices[i]];
               Debug.Log($"Spawning tray {i} at snap point index {traySnapIndices[i]} → position {spawnPosition}");
    
                Instantiate(trayPrefabs[i], spawnPosition + Vector3.up * 1f, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"Invalid snap index: {traySnapIndices[i]}");
            }
        }
    }
    
}
