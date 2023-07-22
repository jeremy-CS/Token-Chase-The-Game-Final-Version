using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Capture : Node
{
    private Animator _animator;

    //Reference to enemy 
    private Transform _lastTarget;
    private CaptureManager _captureManager;

    
    public Capture(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
        _captureManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<CaptureManager>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if (target != _lastTarget)
        {
            _lastTarget = target;
        }

        bool targetCaptured = _captureManager.CaptureEnemy(target);
        if (targetCaptured)
        {
            //Capturing Target in range
            ClearData("target");

            //Revert back to patrolling
            _animator.SetBool("Running", false);
            _animator.SetBool("Walking", true);

            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
