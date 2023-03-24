using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    /// <summary>
    /// Checks whether the player is visible irregardless of the tank's view cone.
    /// </summary>
    public class IsPlayerVisibleNode : Node
    {
        private AITankController _tankController;

        public IsPlayerVisibleNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            var isVisible = _tankController.IsPlayerVisible();
            if (isVisible)
                return NodeState.Success;

            return NodeState.Failure;
        }
    }
}
