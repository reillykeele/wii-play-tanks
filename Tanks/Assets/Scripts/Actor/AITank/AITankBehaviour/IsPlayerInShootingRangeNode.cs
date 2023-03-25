using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    /// <summary>
    /// Checks whether the player is within range of the tank, irregardless of obstacles.
    /// </summary>
    public class IsPlayerInShootingRangeNode : Node
    {
        private AITankController _tankController;

        public IsPlayerInShootingRangeNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            var isInRange = _tankController.IsPlayerInShootingRange();
            if (isInRange)
                return NodeState.Success;

            return NodeState.Failure;
        }
    }
}