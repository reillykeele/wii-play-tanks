using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class AimAtDestinationNode : Node
    {
        private AITankController _tankController;

        public AimAtDestinationNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            _tankController.AimAtDestination();

            return NodeState.Success;
        }
    }
}