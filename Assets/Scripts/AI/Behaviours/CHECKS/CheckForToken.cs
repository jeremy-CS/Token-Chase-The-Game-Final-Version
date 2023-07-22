using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckForToken : Node
{
    private Transform _transform;
    private List<GameObject> _tokens = new List<GameObject>();
    private Transform _targetToken;
    private Waypoint _targetWaypoint;
    private Waypoint _nearestWaypoint;
    private List<Waypoint> _map;

    public CheckForToken(Transform transform)
    {
        _transform = transform;
        AddTokens(GameObject.FindGameObjectsWithTag("Token"), _tokens);
        _map = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapGenerator>().GetMap().graphNodes;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("tokenLocation");
        object tp = GetData("tokenPath");
        object u = GetData("updateIndex");

        if (u == null)
        {
            parent.parent.SetData("updateIndex", true);
        }

        if (t == null || tp == null)
        {
            //Set token data
            UpdateTokens();

            if (_tokens.Count == 0)
            {
                //No more token on the map
                state = NodeState.FAILURE;
                return state;
            }

            GetTargetToken(_tokens);
            GetNearestWaypoint();
            parent.parent.SetData("tokenPath", PathFinder.AStar(_nearestWaypoint, _targetWaypoint));
        }

        state = NodeState.SUCCESS;
        return state;
    }

    public void GetTargetToken(List<GameObject> tokens)
    {
        GameObject targetToken = tokens[0];
        foreach (GameObject token in tokens)
        {
            if (token != null)
            {
                _targetToken = token.transform;
                break;
            }
        }

        //Find nearest token if there is not one stored already
        foreach (GameObject token in tokens)
        {
            if (token != null && Vector3.Distance(_targetToken.position, _transform.position) > Vector3.Distance(token.transform.position, _transform.position))
            {
                _targetToken = token.transform;
                targetToken = token;
            }
        }

        //Set target token gameObject
        parent.parent.SetData("targetToken", targetToken);

        //Find waypoint equivalent of token placement
        foreach (Waypoint node in _map)
        {
            if (node.Position == new Vector3(_targetToken.position.x, 0, _targetToken.position.z))
            {
                _targetWaypoint = node;
                parent.parent.SetData("tokenLocation", _targetWaypoint);
                break;
            }
        }
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

    public void UpdateTokens()
    {
        AddTokens(GameObject.FindGameObjectsWithTag("Token"), _tokens);
    }

    public void AddTokens(GameObject[] tokens, List<GameObject> tokenList)
    {
        tokenList.Clear();

        foreach (GameObject token in tokens)
        {
            tokenList.Add(token);
        }
    }
}
