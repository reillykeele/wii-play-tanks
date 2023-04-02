using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class FindWanderDirectionNode : Node
    {
        private AITankController _tankController;
        private float _angle;

        public FindWanderDirectionNode(AITankController tankController, float angle)
        {
            _tankController = tankController;
            _angle = angle;
        }

        public override NodeState Tick()
        {
            var randomAngle = Random.Range(-_angle, _angle);

            if (_tankController.HasDestination() == false)
            {
                if (Physics.Raycast(_tankController.transform.position, _tankController.transform.forward, out var hitInfo, 100f, LayerMask.NameToLayer("Walls")))
                    _tankController.SetDestination(hitInfo.point);
            }
            else if (Physics.Raycast(_tankController.transform.position, _tankController.transform.forward, out var hitInfo, 1f, LayerMask.NameToLayer("Walls")))
            {
                Vector3 pos;
                if (hitInfo.distance < 1f)
                {
                    var lHit = Physics.Raycast(_tankController.transform.position, -_tankController.transform.right, out var lHitInfo, 100f, LayerMask.NameToLayer("Walls"));
                    var rHit = Physics.Raycast(_tankController.transform.position, _tankController.transform.right, out var rHitInfo, 100f, LayerMask.NameToLayer("Walls"));

                    pos = lHitInfo.distance > rHitInfo.distance ? lHitInfo.point : rHitInfo.point;
                }
                else
                {
                    pos = hitInfo.point;
                }

                _tankController.SetDestination(pos);
            }

            return NodeState.Success;
        }
    }
}