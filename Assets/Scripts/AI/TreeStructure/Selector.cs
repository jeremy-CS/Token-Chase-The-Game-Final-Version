using System.Collections.Generic;

namespace BehaviourTree
{
    public class Selector : Node
    {
        //Constructors
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                //Return early if a child is running or has succeeded
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;

                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    
                    default:
                        continue;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}
