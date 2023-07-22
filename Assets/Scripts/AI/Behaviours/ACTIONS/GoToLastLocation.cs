using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoToLastLocation : Node
{
    private Transform _transform;
    private Animator _animator;

    private float _waitTime = 3f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;

    public GoToLastLocation(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Vector3 target = (Vector3)GetData("lastTargetLocation");
        if (Vector3.Distance(_transform.position, target) > 0.1f)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, target, Chaser_AI.ChaseSpeed * Time.deltaTime);
            _transform.LookAt(target);

            state = NodeState.RUNNING;
            return state;   
        }
        else
        {
            //Wait at the last location to see if you can find them
            _waitCounter += Time.deltaTime;

            if (!_waiting)
            {
                _waiting = true;
                _animator.SetBool("Running", false);
                _animator.SetBool("Walking", false);
            }


            if (_waitCounter > _waitTime)
            {
                //Clear last known target from dictionary
                ClearData("lastTargetLocation");

                //Revert back to patrolling
                _animator.SetBool("Running", false);
                _animator.SetBool("Walking", true);

                _waitCounter = 0;
                _waiting = false;

                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}
