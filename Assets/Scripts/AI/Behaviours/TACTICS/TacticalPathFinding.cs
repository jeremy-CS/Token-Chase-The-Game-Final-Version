using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TacticalPathFinding : Node
{
    private Transform _transform;

    //PathFinding variables
    private List<Waypoint> _map;
    private Waypoint _targetWaypoint;
    private Waypoint _nearestWaypoint;

    public TacticalPathFinding(Transform transform)
    {
        _transform = transform;
        _map = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapGenerator>().GetMap().graphNodes;
    }

    public override NodeState Evaluate()
    {
        _targetWaypoint = (Waypoint)GetData("tokenLocation");
        GetNearestWaypoint();

        if (_targetWaypoint == null || _nearestWaypoint == null )
        {
            state = NodeState.FAILURE;
            return state;
        }

        parent.parent.SetData("tokenPath", PathFinder.AStar(_nearestWaypoint, _targetWaypoint));
        parent.parent.SetData("updateIndex", true);

        state = NodeState.SUCCESS;
        return state;
    }

    public void GetNearestWaypoint()
    {
        _nearestWaypoint = _map[0];

        foreach (Waypoint node in _map)
        {
            if (Vector3.Distance(_nearestWaypoint.Position, _transform.position) > Vector3.Distance(node.Position, _transform.position))
            {
                _nearestWaypoint = node;
            }
        }
    }
}
