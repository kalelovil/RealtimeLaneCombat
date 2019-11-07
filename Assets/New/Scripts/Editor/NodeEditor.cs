using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Node))]
public class NodeEditor : Editor
{
    Node _myTarget;
    private void Awake()
    {
        _myTarget = (Node)target;
    }

    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Custom Inspector", MonoScript.FromScriptableObject(this), GetType(), false);
        GUI.enabled = true;
        base.OnInspectorGUI();

        if (GUILayout.Button("Add NodePath"))
        {
            _myTarget.AddNodePath();
        }
    }
}
