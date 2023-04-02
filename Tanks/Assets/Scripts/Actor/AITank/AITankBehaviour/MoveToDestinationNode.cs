using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class MoveToDestinationNode : Node
    {
        private AITankController _tankController;

        public MoveToDestinationNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            _tankController.MoveToDestination();

            return NodeState.Success;
        }
    }
}