using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class CheckIfOutOfSight : Node
{
    private static int _enemyLayerMask = 1 << 6;

    private Transform _transform;
    private Animator _animator;

    public CheckIfOutOfSight(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        object LastT = GetData("lastTargetLocation");

        if (t != null)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, Chaser_AI.fovRange, _enemyLayerMask);

            if (colliders.Length == 0)
            {
                Transform lastTT = (Transform)t;
                //FIXME: should clamp to a max distance
                parent.parent.parent.SetData("lastTargetLocation", lastTT.position);
                ClearData("target");

                //Change to animation to running
                _animator.SetBool("Running", true);
                _animator.SetBool("Walking", false);

                state = NodeState.SUCCESS;
                return state;
            }
        }

        if (LastT != null) //There is already a last known location to go to
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
