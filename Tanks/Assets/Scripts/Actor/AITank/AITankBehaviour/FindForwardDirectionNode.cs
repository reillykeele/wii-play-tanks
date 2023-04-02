using Data.Constants;
using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class FindForwardDirectionNode : Node
    {
        private AITankController _tankController;
        private float _stoppingDistance;

        private static LayerMask LAYER_MASK = LayerMask.GetMask(Layer.Walls, Layer.Tank);

        public FindForwardDirectionNode(AITankController tankController, float stoppingDistance)
        {
            _tankController = tankController;
            _stoppingDistance = stoppingDistance;
        }

        public override NodeState Tick()
        {
            if (_tankController.HasDestination() == false)
            {
                // If we don't have a destination set, shoot a raycast forward and go thatF way
                if (Physics.Raycast(_tankController.transform.position, _tankController.transform.forward, out var hitInfo, LAYER_MASK))
                    _tankController.SetDestination(hitInfo.point);
            }
            else
            {
                var distanceToDestination = _tankController.GetDistanceToDestination();
                if (distanceToDestination < _stoppingDistance * 2f || (Physics.Raycast(_tankController.transform.position, _tankController.transform.forward, 1.5f, LayerMask.GetMask(Layer.Tank))))
                {
                    var lHit = Physics.Raycast(_tankController.transform.position, -_tankController.transform.right, out var lHitInfo, LAYER_MASK);
                    var rHit = Physics.Raycast(_tankController.transform.position, _tankController.transform.right, out var rHitInfo, LAYER_MASK);

                    if (lHit && lHitInfo.distance > rHitInfo.distance)
                    {
                        if (lHitInfo.distance > distanceToDestination)
                        {
                            _tankController.SetDestination(lHitInfo.point);
                            return NodeState.Success;
                        }
                    }
                    else
                    {
                        if (rHit && rHitInfo.distance > distanceToDestination)
                        {
                            _tankController.SetDestination(rHitInfo.point);
                            return NodeState.Success;
                        }
                    }
                }
            }

            return NodeState.Success;
        }
    }
}