using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    MapGenerator MapGen;
    private static float _dangerMultiplier = 1000;
    
    //A* Pathfinding
    public static List<Vector3> AStar(Waypoint start, Waypoint goal)
    {
        //Initialize Open/Closed Lists
        List<Waypoint> open = new List<Waypoint>();
        HashSet<Waypoint> closed = new HashSet<Waypoint>();

        //Add start node to open list
        open.Add(start);

        //Loop through every neighbors to find the goal
        while (open.Count > 0)
        {
            Waypoint currentNode = open[0];

            //Find node with lowest fCost in the open list
            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].FCost < currentNode.FCost || (open[i].FCost == currentNode.FCost && open[i].hCost < currentNode.hCost))
                {
                    currentNode = open[i];
                }
            }

            //Remove current node from open list and add it to closed list
            open.Remove(currentNode);
            closed.Add(currentNode);

            //Check if goal node is reached
            if (currentNode == goal) return RetracePath(start, goal);//Goal found


            //Get neighbors of current node and add them to open list
            foreach (Waypoint neighbour in currentNode.Neighbors)
            {
                //Skip neighbour if already in closed list
                if (closed.Contains(neighbour)) continue;

                //Calculate cost of neighbors
                float newGCost = currentNode.gCost + G_Cost(currentNode.Position, neighbour.Position);

                //If new cost to node is lower or node is not in open list, add it, plus instantiate new costs
                if (newGCost < neighbour.gCost || !open.Contains(neighbour))
                {
                    neighbour.gCost = newGCost;
                    neighbour.hCost = H_Cost(neighbour.Position, goal.Position);
                    neighbour.parent = currentNode;

                    if (!open.Contains(neighbour))
                        open.Add(neighbour);
                }
            }
        }
        return null; //No path can be used to reach the goal
    }

    //Cost Calculation (Weights/Heuristics)
    public static float G_Cost(Vector3 a, Vector3 b)
    {
        return Mathf.Abs((a.x - b.x) * (a.x - b.x)) + Mathf.Abs((a.z - b.z) * (a.z - b.z));
    }

    public static float H_Cost(Vector3 node, Vector3 goal)
    {
        float _baseValue = Mathf.Abs((node.x - goal.x) * (node.x - goal.x)) + Mathf.Abs((node.z - goal.z) * (node.z - goal.z));

        Collider[] colliders = Physics.OverlapSphere(node, Seeker_AI.DangerRange, Seeker_AI.ChaserLayerMask);

        if (colliders.Length > 0)
        {
            return _baseValue * _dangerMultiplier;
        }
        else
        {
            return _baseValue;
        }
    }

    //Get path from end node to root node
    public static List<Vector3> RetracePath(Waypoint start, Waypoint end)
    {
        List<Vector3> path = new List<Vector3>();
        Waypoint currentNode = end;

        //Add all nodes to a list
        while (currentNode != start)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.parent;
        }
        path.Add(start.Position);

        //Reverse list to get path from start to end node
        path.Reverse();
        return path;
    }
}