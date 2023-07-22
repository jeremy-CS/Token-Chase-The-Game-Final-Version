//MADE WITH THE HELP OF (https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        //Constructors for Nodes
        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
                Attach(child);
        }

        //Attach function to link nodes together
        private void Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        //Virtual evaluate function
        public virtual NodeState Evaluate() => NodeState.FAILURE;

        //Shared data location
        private Dictionary<string, object> dataContext = new Dictionary<string, object>();

        //Add data to the dictionary
        public void SetData(string key, object value)
        {
            dataContext[key] = value;
        }

        //Get/Find data in the dictionary/tree
        public object GetData(string key)
        {
            //Try to get the value from the node dictionary directly
            object value = null;
            if (dataContext.TryGetValue(key, out value))
                return value;

            //Go up the tree to check if the value is defined somewhere in the branch
            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;

                node = node.parent;
            }
            //Unable to find the data in the tree
            return null;
        }

        //Clear data from the dictionary/tree
        public bool ClearData(string key)
        {
            //Try to find and remove the value from the node dictionary directly
            if (dataContext.ContainsKey(key))
            {
                dataContext.Remove(key);
                return true;
            }

            //Go up the tree to check if the value is defined somewhere in the branch
            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }
            //Unable to find the data in the tree (ignore request)
            return false;
        }
    }
}

