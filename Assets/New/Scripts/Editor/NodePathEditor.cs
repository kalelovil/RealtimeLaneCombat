
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeConnection))]
public class NodePathEditor : Editor
{
    NodeConnection _myTarget;

    [SerializeField] PathBridge _bridgePrefab;

    [SerializeField] MapTerrain _forestTerrainPrefab;
    [SerializeField] MapTerrain _hillsTerrainPrefab;
    [SerializeField] MapTerrain _mountainsTerrainPrefab;
    [SerializeField] MapTerrain _swampTerrainPrefab;
    [SerializeField] MapTerrain _waterTerrainPrefab;
    List<MapTerrain> _terrainPrefabList = new List<MapTerrain>();

    private void Awake()
    {
        _myTarget = (NodeConnection)target;

        _terrainPrefabList = new List<MapTerrain>()
        {
            _forestTerrainPrefab,
            _hillsTerrainPrefab,
            _mountainsTerrainPrefab,
            _swampTerrainPrefab,
            _waterTerrainPrefab,
        };
    }

    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Custom Inspector", MonoScript.FromScriptableObject(this), GetType(), false);
        GUI.enabled = true;
        base.OnInspectorGUI();

        if (GUILayout.Button("Add Bridge"))
        {
            _myTarget.AddPathFeature(_bridgePrefab);
        }
        foreach (MapTerrain terrainPrefab in _terrainPrefabList)
        {
            if (GUILayout.Button($"Add {terrainPrefab.name}"))
            {
                _myTarget.AddPathFeature(terrainPrefab);
            }
        }
    }
}