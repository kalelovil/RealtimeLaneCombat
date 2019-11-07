using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StandardMovement))]
public class CanTraverseTerrain : MonoBehaviour
{
    [SerializeField] internal MapTerrain.MapTerrainType _terrainType;

    private void Start()
    {
        StandardMovement movement = GetComponent<StandardMovement>();
        movement._canTraverseTerrainMap.Add(_terrainType, this);
    }
}
