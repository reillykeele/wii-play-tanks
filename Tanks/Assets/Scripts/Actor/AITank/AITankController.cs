using System.Linq;
using Manager;
using UnityEngine;
using UnityEngine.AI;

namespace Actor.AITank
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AITankController : BaseTankController
    {
        protected NavMeshAgent _agent;

        /// <summary>
        /// The angle from the turret forward that the tank can see.
        /// </summary>
        public float VisionAngle = 30;

        /// <summary>
        /// The max distance from the turret that the tank can see.
        /// </summary>
        public float VisionRange = 10;

        /// <summary>
        /// The max distance from the turret that the tank will try to shoot the player from.
        /// </summary>
        public float ShootRange = 7.5f;

        /// <summary>
        /// The min distance from the player that the tank will navigate to.
        /// </summary>
        public float MovementRange = 5;

        public Vector3? LastKnownPlayerLocation;

        protected bool _hasPathTarget = false;
        protected Vector3 _target;

        protected override void Awake()
        {
            base.Awake();

            _agent = GetComponent<NavMeshAgent>();

            _agent.speed = _moveSpeed;

            // _agent.updatePosition = false;
            // _agent.updateRotation = false;
        }

        void Update()
        {

        }

        void FixedUpdate()
        {
            
        }

        public override void Shoot()
        {
            base.Shoot();
        }

        /// <summary>
        /// Rotates the turret to aim in a specified direction.
        /// </summary>
        /// <param name="dir">Normalized direction vector to aim to.</param>
        public void AimInDirection(Vector3 dir)
        {
            _turretTransform.forward = Vector3.Lerp(_turretTransform.forward, dir, 0.1f);
        }

        /// <summary>
        /// Rotates the turret to aim at a specified position.
        /// </summary>
        /// <param name="pos">The position to aim at.</param>
        public void AimAtPosition(Vector3 pos)
        {
            var dir = (pos - transform.position).normalized;

            _turretTransform.forward = Vector3.Lerp(_turretTransform.forward, dir, 0.1f);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AimAtDestination() => AimAtPosition(_agent.destination);

        /// <summary>
        /// Rotates the turret to aim in the tank's forward direction.
        /// </summary>
        public void AimForward() => AimInDirection(transform.forward);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos">The position to pathfind to.</param>
        public bool MoveToPosition(Vector3 pos)
        {
            var success = _agent.SetDestination(pos);

            // _agent.Move

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearDestination()
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }

        /// <summary>
        /// Checks whether the player is visible irregardless of the tank's view cone and vision range.
        /// </summary>
        public bool IsPlayerVisible()
        {
            var player = GameManager.Instance.Player;

            var dirToPlayer = (player.transform.position - _turretTransform.position).normalized;
            Physics.Raycast(_turretTransform.position, dirToPlayer, out var hitInfo);

            if (hitInfo.transform.tag == "Player")
            {
                LastKnownPlayerLocation = hitInfo.transform.position;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether the player is visible from the player's front and within the tank's view cone.
        /// </summary>
        public bool IsPlayerInSight()
        {
            var player = GameManager.Instance.Player;

            // Check the angle between the player and the AI tank
            var dirToPlayer = (player.transform.position - _turretTransform.position).normalized;
            var angleToPlayer = Vector3.Angle(_turretTransform.forward, dirToPlayer);

            // If the angle is within our vision cone, cast a ray
            if (angleToPlayer > VisionAngle)
                return false;

            Physics.Raycast(_turretTransform.position, dirToPlayer, out var hitInfo);

            // If the ray hit the player first, the player is visible
            if (hitInfo.transform.tag == "Player")
            {
                LastKnownPlayerLocation = hitInfo.transform.position;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether the player is within range of the tank, irregardless of obstacles.
        /// </summary>
        public bool IsPlayerInRange(float range)
        {
            var player = GameManager.Instance.Player;

            var dist = Vector3.Distance(transform.position, player.transform.position);

            if (dist < range)
                return true;

            return false;
        }

        public bool IsPlayerInVisionRange() => IsPlayerInRange(VisionRange);
        public bool IsPlayerInShootingRange() => IsPlayerInRange(ShootRange);
        public bool IsPlayerInMovementRange() => IsPlayerInRange(MovementRange);

        /// <summary>
        /// Checks whether the player is visible and within range of the tank.
        /// </summary>
        public bool IsPlayerVisibleAndInRange(float range)
        {
            var player = GameManager.Instance.Player;

            var dirToPlayer = (player.transform.position - _turretTransform.position).normalized;
            Physics.Raycast(_turretTransform.position, dirToPlayer, out var hitInfo);

            if (hitInfo.transform.tag == "Player")
            {
                LastKnownPlayerLocation = hitInfo.transform.position;

                if (hitInfo.distance < range)
                    return true;
            }

            return false;
        }

        public bool IsPlayerVisibleAndInVisionRange() => IsPlayerVisibleAndInRange(VisionRange);
        public bool IsPlayerVisibleAndInShootingRange() => IsPlayerVisibleAndInRange(ShootRange);
        public bool IsPlayerVisibleAndInMovementRange() => IsPlayerVisibleAndInRange(MovementRange);

        /// <summary>
        /// Checks whether the player is visible and within range of the tank.
        /// </summary>
        public bool IsPlayerInSightAndInRange(float range)
        {
            var player = GameManager.Instance.Player;

            // Check the angle between the player and the AI tank
            var dirToPlayer = (player.transform.position - _turretTransform.position).normalized;
            var angleToPlayer = Vector3.Angle(_turretTransform.forward, dirToPlayer);

            // If the angle is within our vision cone, cast a ray
            if (angleToPlayer > VisionAngle)
                return false;

            Physics.Raycast(_turretTransform.position, dirToPlayer, out var hitInfo);

            // If the ray hit the player first, the player is visible
            if (hitInfo.transform.tag == "Player")
            {
                LastKnownPlayerLocation = hitInfo.transform.position;

                if (hitInfo.distance < range)
                    return true;
            }

            return false;
        }

        public bool IsPlayerInSightAndInVisionRange() => IsPlayerInSightAndInRange(VisionRange);
        public bool IsPlayerInSightAndInShootingRange() => IsPlayerInSightAndInRange(ShootRange);
        public bool IsPlayerInSightAndInMovementRange() => IsPlayerInSightAndInRange(MovementRange);
    }
}
