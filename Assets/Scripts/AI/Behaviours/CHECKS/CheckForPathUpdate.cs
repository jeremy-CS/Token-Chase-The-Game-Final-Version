using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckForPathUpdate : Node
{
    private Transform _transform;
    public CheckForPathUpdate(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        List<Vector3> path = (List<Vector3>)GetData("tokenPath");

        if (path != null)
        {
            int currentIndex = (int)GetData("currentIndex");
            for (int i = currentIndex; i < path.Count; i++)
            {
                Collider[] colliders = Physics.OverlapSphere(path[i], Seeker_AI.fovRange, Seeker_AI.ChaserLayerMask);

                if (colliders.Length > 0)
                {
                    //Need to recalculate a new path to the token
                    state = NodeState.SUCCESS;
                    return state;
                }
            }
        }

        state = NodeState.FAILURE;
        return state;
    }
}
