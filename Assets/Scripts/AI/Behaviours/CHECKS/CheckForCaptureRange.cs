using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckForCaptureRange : Node
{
    private Transform _transform;
    private Animator _animator;

    public CheckForCaptureRange(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");

        if (t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;

        if (Vector3.Distance(_transform.position, target.position) <= Chaser_AI.CaptureRange)
        {
            //Chaser captured the target
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
