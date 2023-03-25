using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class ClearPathNode : Node
    {
        private AITankController _tankController;

        public ClearPathNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            _tankController.ClearDestination();
            return NodeState.Success;
        }
    }
}
