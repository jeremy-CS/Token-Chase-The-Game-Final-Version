using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoToToken : Node
{
    private Transform _transform;
    private Animator _animator;

    //Script references
    private CaptureManager _captureManager;

    //PathFinding variables
    private int _currentWaypointIndex = 0;
    private List<Vector3> _lastPath;

    public GoToToken(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
        _captureManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<CaptureManager>();
    }

    public override NodeState Evaluate()
    {
        List<Vector3> path = (List<Vector3>)GetData("tokenPath");
        bool updateIndex = (bool)GetData("updateIndex");
        _animator.SetBool("Running", true);

        if (updateIndex)
        {
            _currentWaypointIndex = 0;
            parent.parent.SetData("currentIndex", _currentWaypointIndex);
            parent.parent.SetData("updateIndex", false);
        }

        //Arrived at destination
        if (_currentWaypointIndex == path.Count)
        {
            _currentWaypointIndex = 0;
            parent.parent.SetData("currentIndex", _currentWaypointIndex);
            RemoveTokenData();
        }

        Vector3 wp = path[_currentWaypointIndex];
        if (Vector3.Distance(_transform.position, wp) < 0.1f)
        {
            _transform.position = wp;
            _currentWaypointIndex++;
            parent.parent.SetData("currentIndex", _currentWaypointIndex);
        }
        else
        {
            _transform.position = Vector3.MoveTowards(_transform.position, wp, Seeker_AI.MinSpeed * Time.deltaTime);
            _transform.LookAt(wp);
        }

        state = NodeState.RUNNING;
        return state;
    }

    public void RemoveTokenData()
    {
        //Delete token gameObject
        GameObject targetToken = (GameObject)GetData("targetToken");
        _captureManager.CaptureToken(targetToken);

        //Clear token information from dictionary
        ClearData("targetToken");
        ClearData("tokenPath");
        ClearData("tokenLocation");
    }
}
