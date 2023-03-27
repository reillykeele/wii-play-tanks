using Manager;
using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class AimAtPlayerNode : Node
    {
        private AITankController _tankController;

        public AimAtPlayerNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            var player = LevelManager.Instance.Player;

            _tankController.AimAtPosition(player.transform.position);

            return NodeState.Success;
        }
    }
}
