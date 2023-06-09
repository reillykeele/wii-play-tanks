﻿using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class MoveToKnownPlayerLocationNode : Node
    {
        private AITankController _tankController;

        public MoveToKnownPlayerLocationNode(AITankController tankController)
        {
            _tankController = tankController;
        }

        public override NodeState Tick()
        {
            var pos = _tankController.LastKnownPlayerLocation;

            if (pos == null) return NodeState.Failure;

            _tankController.SetDestination(pos.Value);
            _tankController.MoveToDestination();

            // var success = _tankController.MoveToPosition(pos.Value);
            // if (success)
            return NodeState.Success;

            // Debug.Log("MoveToPosition Failure");
            // return NodeState.Failure;

        }
    }
}