using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    /// <summary>
    /// Checks whether the player is visible and within the tank's view cone.
    /// </summary>
    public class IsPlayerInSightNode : Node
    {
        private AITankController _tankController;

        public IsPlayerInSightNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            var isInSight = _tankController.IsPlayerInSight();
            if (isInSight)
                return NodeState.Success;

            return NodeState.Failure;
        }
    }
}