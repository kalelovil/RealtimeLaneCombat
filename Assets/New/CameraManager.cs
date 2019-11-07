using System;
using UnityEngine;

internal class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; internal set; }

    private void Awake()
    {
        Instance = this;
    }

    internal void FocusOn(NodeUnit selectedUnit)
    {
        //TODO Implement
    }
}