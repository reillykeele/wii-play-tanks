using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class FindForwardDirectionNode : Node
    {
        private AITankController _tankController;

        public FindForwardDirectionNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            if (_tankController.HasDestination() == false)
            {
                // If we don't have a destination set, shoot a raycast forward and go that way
                if (Physics.Raycast(_tankController.transform.position, _tankController.transform.forward, out var hitInfo, 100f, LayerMask.NameToLayer("Walls")))
                    _tankController.SetDestination(hitInfo.point);
            }
            else
            {
                var distanceToDestination = _tankController.GetDistanceToDestination();
                if (distanceToDestination < 1.25f)
                {
                    Debug.Log("close");
                    var lHit = Physics.Raycast(_tankController.transform.position, -_tankController.transform.right, out var lHitInfo, 50f, LayerMask.NameToLayer("Walls"));
                    var rHit = Physics.Raycast(_tankController.transform.position, _tankController.transform.right, out var rHitInfo, 50f, LayerMask.NameToLayer("Walls"));

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