using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class Patrol : Node
{
    //Patrol attributes
    private Transform _transform;
    private Transform[] _waypoints;
    private Animator _animator;

    //Waypoint travel attributes
    private int _currentWaypointIndex = 0;
    private bool _updateIndex = true;

    private float _waitTime = 3f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = true;

    public Patrol(Transform transform, Transform[] waypoints )
    {
        _transform = transform;
        _waypoints = waypoints;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        if (_updateIndex || (bool)GetData("updateIndex"))
        {
            _currentWaypointIndex = FindClosestPatrolPoint();
            parent.SetData("updateIndex", false);
        }

        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter > _waitTime)
            {
                _waiting = false;
                //Walking animation starts, done waiting
                _animator.SetBool("Walking", true);
            }
        }
        else
        {
            Transform wp = _waypoints[_currentWaypointIndex];
            if (Vector3.Distance(_transform.position, wp.position) < 0.1f)
            {
                _transform.position = wp.position;
                _waitCounter = 0f;
                _waiting = true;

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                //Idle animation starts, starts waiting
                _animator.SetBool("Walking", false);
            }
            else
            {
                _transform.position = Vector3.MoveTowards(_transform.position, wp.position, Chaser_AI.PatrolSpeed * Time.deltaTime);
                _transform.LookAt(wp.position);
            }
        }

        state = NodeState.RUNNING;
        return state;
    }

    public int FindClosestPatrolPoint()
    {
        int index = 0;
        foreach (Transform waypoint in _waypoints)
        {
            if (Vector3.Distance(waypoint.position, _transform.position) < 0.2f)
            {
                _updateIndex = false;
                break;
            }
            else index++;
        }

        return index;
    }
}
