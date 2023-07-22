using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckIfFrozen : Node
{
    private Transform _transform;

    public CheckIfFrozen(Transform transform)
    {
        _transform = transform;
    }
    public override NodeState Evaluate()
    {
        if (_transform.GetComponent<Chaser_AI>().IsFrozen)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}