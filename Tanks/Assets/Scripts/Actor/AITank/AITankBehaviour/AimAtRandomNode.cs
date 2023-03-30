using Manager;
using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class AimAtRandomNode : Node
    {
        private AITankController _tankController;

        private Vector3 _randomDir;
        private float _randomAngle = 360f;
        private float _randomRefreshRate;
        private float _lastRandomTime = 0;

        public AimAtRandomNode(AITankController tankController, float randomAngle, float randomRefreshRate)
        {
            _tankController = tankController;
            _randomAngle = randomAngle;
            _randomRefreshRate = randomRefreshRate;
        }

        public override NodeState Tick()
        {
            var currTime = Time.fixedTime;
            if (_lastRandomTime + _randomRefreshRate < currTime)
            {
                // Calculate new random direction
                var randomAngle = (Random.value * _randomAngle) - (_randomAngle / 2);

                _randomDir =  Quaternion.AngleAxis(randomAngle, Vector3.up) * _tankController.transform.forward;

                _lastRandomTime = currTime;
            }

            _tankController.AimInDirection(_randomDir);

            return NodeState.Success;
        }
    }
}