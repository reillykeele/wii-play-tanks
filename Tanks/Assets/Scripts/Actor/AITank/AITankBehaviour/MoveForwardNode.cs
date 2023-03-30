using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class MoveForwardNode : Node
    {
        private AITankController _tankController;
        private float _distance;

        public MoveForwardNode(AITankController tankController, float distance)
        {
            _tankController = tankController;
            _distance = distance;
        }

        public override NodeState Tick()
        {
            var dir = _tankController.transform.forward;

            _tankController.MoveInDirection(dir, _distance);
            
            return NodeState.Success;
        }
    }
}