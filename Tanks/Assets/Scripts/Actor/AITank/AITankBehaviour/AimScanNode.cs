using System;
using Manager;
using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank.AITankBehaviour
{
    public class AimScanNode : Node
    {
        private AITankController _tankController;

        
        private float _scanAngle = 45f;
        private float _scanSpeed = 0.25f;

        private float _current;
        private float _target;

        private readonly Quaternion _rotation1;
        private readonly Quaternion _rotation2;

        public AimScanNode(AITankController tankController, float scanAngle, float scanSpeed)
        {
            _tankController = tankController;
            
            _scanAngle = scanAngle;
            _scanSpeed = scanSpeed;

            _rotation1 = Quaternion.AngleAxis(-_scanAngle, Vector3.up);
            _rotation2 = Quaternion.AngleAxis(_scanAngle, Vector3.up);
        }

        public override NodeState Tick()
        {
            _current = Mathf.MoveTowards(_current, _target, _scanSpeed * Time.fixedDeltaTime);
            if (Math.Abs(_current - _target) < 0.0005f)
                _target = Mathf.Abs(1 - _target);

            var scanDir = Vector3.Lerp(
                _rotation1 * _tankController.transform.forward, 
                _rotation2 * _tankController.transform.forward,
                Mathf.PingPong(_current, 0.5f) * 2);

            _tankController.AimInDirection(scanDir);

            return NodeState.Success;
        }
    }
}