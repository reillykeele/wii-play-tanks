using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    /// <summary>
    /// Checks whether the player is visible from the tank's muzzle and within the tank's view cone.
    /// </summary>
    public class IsPlayerInSightOfTurretNode : Node
    {
        private AITankController _tankController;

        public IsPlayerInSightOfTurretNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            var isInSight = _tankController.IsPlayerInSightOfTurret();
            if (isInSight)
                return NodeState.Success;

            return NodeState.Failure;
        }
    }
}