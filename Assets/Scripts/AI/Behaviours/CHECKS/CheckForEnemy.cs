using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class CheckForEnemy : Node
{
    private static int _enemyLayerMask = 1 << 6;

    private Transform _transform;
    private Animator _animator;

    public CheckForEnemy(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        
        Collider[] colliders = Physics.OverlapSphere(_transform.position, Chaser_AI.fovRange, _enemyLayerMask);
        
        if (t == null)
        {
            if (colliders.Length > 0)
            {
                parent.parent.parent.SetData("target", colliders[0].transform);

                //Change to animation to running
                _animator.SetBool("Running", true);
                _animator.SetBool("Walking", false);

                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        if (colliders.Length == 0) //Enemy is no longer in FOV
        {
            Debug.Log("Lost target");
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
