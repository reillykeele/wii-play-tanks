using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    /// <summary>
    /// Checks whether the player is within range of the tank, irregardless of obstacles.
    /// </summary>
    public class IsPlayerInMovementRangeNode : Node
    {
        private AITankController _tankController;

        public IsPlayerInMovementRangeNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            var isInRange = _tankController.IsPlayerInMovementRange();
            if (isInRange)
                return NodeState.Success;

            return NodeState.Failure;
        }
    }
}