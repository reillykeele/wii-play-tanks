using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class ShootNode : Node
    {
        private AITankController _tankController;

        public ShootNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            _tankController.Shoot();

            return NodeState.Success;
        }
    }
}