using System;
using UnityEngine;
public class PathBridge : PathFeature
{
    public enum State
    {
        Intact,
        Collapsed,
    }
    [SerializeField] State _currentState;
    public State CurrentState { get { return _currentState; } set { SetCurrentState(value); } }
    private void SetCurrentState(State value)
    {
        _currentState = value;
    }

    public override int GetAttackPenalty()
    {
        int penalty = 0;
        switch (_currentState)
        {
            case State.Intact:
                penalty = 0;
                break;
            case State.Collapsed:
                penalty = -4;
                break;
            default:
                break;
        }
        return penalty;
    }
    public override int GetMovePenalty()
    {
        int penalty = 0;
        switch (_currentState)
        {
            case State.Intact:
                penalty = 0;
                break;
            case State.Collapsed:
                penalty = -4;
                break;
            default:
                break;
        }
        return penalty;
    }

    public override bool GetIsMoveBlocked(NodeUnit unit)
    {
        return false;
    }
}