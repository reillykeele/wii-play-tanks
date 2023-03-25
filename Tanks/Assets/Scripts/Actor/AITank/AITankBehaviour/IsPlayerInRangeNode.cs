using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    /// <summary>
    /// Checks whether the player is within range of the tank, irregardless of obstacles.
    /// </summary>
    public class IsPlayerInRangeNode : Node
    {
        private AITankController _tankController;
        private float _range;

        public IsPlayerInRangeNode(AITankController tankController, float range)
        {
            _tankController = tankController;
            _range = range;
        }

        public override NodeState Tick()
        {
            var isInRange = _tankController.IsPlayerInRange(_range);
            if (isInRange)
                return NodeState.Success;

            return NodeState.Failure;
        }
    }
}
