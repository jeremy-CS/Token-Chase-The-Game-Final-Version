using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class UseFreezeBomb : Node
{
    private Transform _transform;
    private Chaser_AI _chaserTarget;
    public UseFreezeBomb(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        _chaserTarget = (Chaser_AI)GetData("targetChaser");

        if (_chaserTarget == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        _chaserTarget.Freeze();

        state = NodeState.SUCCESS;
        return state;
    }
}
