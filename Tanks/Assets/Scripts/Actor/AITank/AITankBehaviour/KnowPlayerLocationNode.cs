using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    /// <summary>
    /// Checks whether the tank has any idea where the player is.
    /// </summary>
    public class KnowPlayerLocationNode : Node
    {
        private AITankController _tankController;

        public KnowPlayerLocationNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            var lastKnownPlayerLocation = _tankController.LastKnownPlayerLocation;
            if (lastKnownPlayerLocation != null)
                return NodeState.Success;

            return NodeState.Failure;
        }
    }
}