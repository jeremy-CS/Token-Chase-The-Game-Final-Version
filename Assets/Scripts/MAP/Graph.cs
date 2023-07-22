using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    //List of nodes in the graph
    public List<Waypoint> graphNodes;

    //Constructors for a graph
    public Graph() : this(null) { }
    public Graph(List<Waypoint> NodeList)
    {
        if (NodeList == null)
            this.graphNodes = new List<Waypoint>();
        else
            this.graphNodes = NodeList;
    }

    //Adding nodes to the graph
    public void AddNode(Vector3 aPos)
    {
        graphNodes.Add(new Waypoint(aPos));
    }

    public void AddNode(Waypoint node)
    {
        graphNodes.Add(node);
    }

    //Adding undirected edges to nodes
    public void AddUndirectedEdge(Waypoint from, Waypoint to)
    {
        from.Neighbors.Add(to);
        to.Neighbors.Add(from);
    }
}

public class Waypoint
{
    //Member variables
    public Vector3 Position;
    public List<Waypoint> Neighbors;
    public Waypoint parent;

    //Node Cost variables
    public float gCost = 0; //Distance from current node to the goal node
    public float hCost = 0; //Heuristic value of a node
    public float FCost { get { return gCost + hCost; } } //Total cost of a node

    //Constructors for a base node
    public Waypoint() { }
    public Waypoint(Vector3 aPos) : this(aPos, null) { }
    public Waypoint(Vector3 aPos, List<Waypoint> neighbors)
    {
        if (neighbors == null)
            Neighbors = new List<Waypoint>();
        else
            Neighbors = neighbors;

        this.Position = aPos;
    }
}
