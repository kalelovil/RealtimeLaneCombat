using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor
{
    MapManager _myTarget;
    private void Awake()
    {
        _myTarget = (MapManager)target;
    }

    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Custom Inspector", MonoScript.FromScriptableObject(this), GetType(), false);
        GUI.enabled = true;
        base.OnInspectorGUI();

        if (GUILayout.Button("Regenerate Connection References"))
        {
            _myTarget.RegenerateConnectionReferences();
        }
    }
}
