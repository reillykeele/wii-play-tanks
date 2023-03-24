using Manager;
using UnityEngine;
using UnityEngine.AI;

namespace Actor.AITank
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AITankController : BaseTankController
    {
        protected NavMeshAgent _agent;

        public float VisionAngle = 30;
        public float Range = 10;

        protected override void Awake()
        {
            base.Awake();

            _agent = GetComponent<NavMeshAgent>();

            _agent.speed = _moveSpeed;
        }

        void Update()
        {

        }

        void FixedUpdate()
        {
            
        }

        public void AimAtPosition(Vector3 pos)
        {
            var dir = (pos - transform.position).normalized;

            _turretTransform.forward = Vector3.Lerp(_turretTransform.forward, dir, 0.1f);
        }

        /// <summary>
        /// Checks whether the player is within range of the tank, irregardless of obstacles.
        /// </summary>
        public bool IsPlayerInRange()
        {
            var player = GameManager.Instance.Player;

            var dist = Vector3.Distance(transform.position, player.transform.position);

            if (dist < Range)
                return true;

            return false;
        }

        /// <summary>
        /// Checks whether the player is visible irregardless of the tank's view cone.
        /// </summary>
        public bool IsPlayerVisible()
        {
            var player = GameManager.Instance.Player;

            var dirToPlayer = (player.transform.position - transform.position).normalized;
            Physics.Raycast(_muzzleTransform.position, dirToPlayer, out var hitInfo);

            if (hitInfo.transform.tag == "Player")
                return true;

            return false;
        }

        /// <summary>
        /// Checks whether the player is visible from the player's front and within the tank's view cone.
        /// </summary>
        public bool IsPlayerInSight()
        {
            var player = GameManager.Instance.Player;

            // Check the angle between the player and the AI tank
            var dirToPlayer = (player.transform.position - transform.position).normalized;
            var angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);

            // If the angle is within our vision cone, cast a ray
            if (angleToPlayer > VisionAngle)
                return false;

            Physics.Raycast(transform.position, dirToPlayer, out var hitInfo);

            // If the ray hit the player first, the player is visible
            if (hitInfo.transform.tag == "Player")
                return true;

            return false;
        }

        /// <summary>
        /// Checks whether the player is visible from the tank's muzzle and within the tank's view cone.
        /// </summary>
        public bool IsPlayerInSightOfTurret()
        {
            var player = GameManager.Instance.Player;

            // Check the angle between the player and the AI tank
            var dirToPlayer = (player.transform.position - _muzzleTransform.position).normalized;
            var angleToPlayer = Vector3.Angle(_muzzleTransform.forward, dirToPlayer);

            // If the angle is within our vision cone, cast a ray
            if (angleToPlayer > VisionAngle)
                return false;

            Physics.Raycast(_muzzleTransform.position, dirToPlayer, out var hitInfo);

            // If the ray hit the player first, the player is visible
            if (hitInfo.transform.tag == "Player")
                return true;

            return false;
        }
    }
}
