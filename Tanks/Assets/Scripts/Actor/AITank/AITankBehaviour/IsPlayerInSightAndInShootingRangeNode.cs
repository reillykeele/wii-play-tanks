using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class IsPlayerInSightAndInShootingRangeNode : Node
    {
        private AITankController _tankController;

        public IsPlayerInSightAndInShootingRangeNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            var isInRange = _tankController.IsPlayerInSightAndInShootingRange();
            if (isInRange)
                return NodeState.Success;

            return NodeState.Failure;
        }
    }
}
