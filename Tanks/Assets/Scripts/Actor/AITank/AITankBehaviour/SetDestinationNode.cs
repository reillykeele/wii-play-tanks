using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class SetDestinationNode : Node
    {
        private AITankController _tankController;
        private Vector3 _pos;

        public SetDestinationNode(AITankController tankController, Vector3 pos)
        {
            _tankController = tankController;
            _pos = pos;
        }

        public override NodeState Tick()
        {
            _tankController.SetDestination(_pos);
            _tankController.MoveToDestination();
            
            return NodeState.Success;
        }
    }
}