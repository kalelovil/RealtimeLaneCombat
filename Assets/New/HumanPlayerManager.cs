using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerManager : AbstractPlayerManager
{
    // TODO Reconfigue for multiplayer, always point to local player
    public static HumanPlayerManager Instance;

    [SerializeField] bool _isLocal;
    protected override bool IsLocal => _isLocal;

    public override LeftOrRight MapSide
    {
        get { return LeftOrRight.Left; }
    }
    public override Color Colour
    {
        get { return Color.blue; }
    }

    protected override void HighlightUnit(NodeUnit unit)
    {
        //TODO Implement
    }

    protected override void SetSelectedUnit_OverrideMethod(NodeUnit value)
    {
        //TODO Implement
    }

    private void Awake()
    {
        if (_isLocal) Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
