using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    /// <summary>
    /// Checks whether the player is within range of the tank, irregardless of obstacles.
    /// </summary>
    public class IsPlayerInRangeNode : Node
    {
        private AITankController _tankController;

        public IsPlayerInRangeNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            var isInRange = _tankController.IsPlayerInRange();
            if (isInRange)
                return NodeState.Success;

            return NodeState.Failure;
        }
    }
}
