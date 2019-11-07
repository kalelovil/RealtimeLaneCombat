using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerManager : AbstractPlayerManager
{
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
