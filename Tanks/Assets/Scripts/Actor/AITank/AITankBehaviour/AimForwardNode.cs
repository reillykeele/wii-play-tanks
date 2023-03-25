using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class AimForwardNode : Node
    {
        private AITankController _tankController;

        public AimForwardNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            _tankController.AimForward();

            return NodeState.Success;
        }
    }
}