using UnityEngine;

namespace Util.AI.BehaviourTree
{
    public class DebugLogNode : Node
    {
        private string _message;

        public DebugLogNode(string message)
        {
            _message = message;
        }

        public override NodeState Tick()
        {
            Debug.Log(_message);

            return NodeState.Running;
        }
    }
}