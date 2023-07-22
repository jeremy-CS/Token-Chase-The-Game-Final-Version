using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckForChasers : Node
{
    private Transform _transform;

    public CheckForChasers(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");

        Collider[] colliders = Physics.OverlapSphere(_transform.position, Seeker_AI.fovRange, Seeker_AI.ChaserLayerMask);

        if (t == null)
        {
            if (colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);

                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        if (colliders.Length == 0) //Enemy is no longer in FOV
        {
            ClearData("target");
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
