using Manager;
using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class AimAtRandomNode : Node
    {
        private AITankController _tankController;

        private Vector3 _randomDir;
        private float _randomRefreshRate;
        private float _lastRandomTime = 0;

        public AimAtRandomNode(AITankController tankController, float randomRefreshRate)
        {
            _tankController = tankController;
            _randomRefreshRate = randomRefreshRate;
        }

        public override NodeState Tick()
        {
            var currTime = Time.fixedTime;
            if (_lastRandomTime + _randomRefreshRate < currTime)
            {
                // Calculate new random direction
                var randomDirection = Random.insideUnitCircle.normalized;
                _randomDir = new Vector3(randomDirection.x, 0, randomDirection.y);

                _lastRandomTime = currTime;
            }

            _tankController.AimInDirection(_randomDir);

            return NodeState.Success;
        }
    }
}