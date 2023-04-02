using System;
using System.Linq;
using Manager;
using UnityEngine;
using UnityEngine.AI;
using Util.Helpers;

namespace Actor.AITank
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AITankController : BaseTankController
    {
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

        /// <summary>
        /// 
        /// </summary>
        public float StoppingDistance = 1f;

        public Vector3? LastKnownPlayerLocation;

        protected bool _hasPathTarget = false;
        protected Vector3 _target;

        protected NavMeshAgent _agent;
        protected Vector3 _destination;
        protected NavMeshPath _path;

        protected override void Awake()
        {
            base.Awake();

            _path = new NavMeshPath();

            _agent = GetComponent<NavMeshAgent>();
            //
            // _agent.speed = _moveSpeed;

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

        public override void Explode()
        {
            base.Explode();

            LevelManager.Instance.RemainingTanks--;
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
        public void AimAtDestination() => AimAtPosition(_destination);

        /// <summary>
        /// Rotates the turret to aim in the tank's forward direction.
        /// </summary>
        public void AimForward() => AimInDirection(transform.forward);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos">The position to pathfind to.</param>
        public void MoveTowardsPosition(Vector3 pos)
        {
            var dir = (pos - transform.position);
            dir.y = 0f;

            if (dir != Vector3.zero)
            {
                var movementDir = dir.normalized; //= MathHelper.RoundTo8(dir);

                var dot = Vector3.Dot(transform.forward, movementDir);
                if (Math.Abs(Mathf.Abs(dot) - 1f) < 0.0005f)
                {
                    // same direction; ignore
                    transform.forward = Mathf.Sign(dot) * movementDir;
                    _agent.Move(movementDir * _moveSpeed * Time.fixedDeltaTime);
                }
                else if (Mathf.Abs(dot) < 0.0005f)
                {
                    // = 90 deg turn
                    var targetRot = movementDir;
                    transform.forward = Vector3.RotateTowards(transform.forward, targetRot, _turnSpeed * Time.fixedDeltaTime, 0.0f);
                }
                else
                {
                    // > 90 deg turn
                    var targetRot = Mathf.Sign(dot) * movementDir;
                    transform.forward = Vector3.RotateTowards(transform.forward, targetRot, _turnSpeed * Time.fixedDeltaTime, 0.0f);
                }
            }
        }
        
        /// <summary>
        /// Moves the tank towards the set destination.
        /// </summary>
        public virtual void MoveToDestination()
        {
            if (IsAtDestination())
            {
                Debug.Log("already at destination 1");
                return;
            }

            var nextPos = _path.corners[1];
            if (Vector3.Distance(transform.position, nextPos) < 0.5f)
            {
                RecalculatePath();
                if (IsAtDestination())
                {
                    Debug.Log("already at destination 2");
                    return;
                }
            }

            MoveTowardsPosition(_path.corners[1]);
        }

        /// <summary>
        /// Indicates whether the tank has a destination currently set.
        /// </summary>
        /// <returns>Returns <c>true</c> if a destination is set, otherwise <c>false</c>.</returns>
        public bool HasDestination() => _path.corners.Any();

        /// <summary>
        /// Checks whether the tank is within the stopping distance of the destination.
        /// </summary>
        /// <returns></returns>
        public bool IsAtDestination()
        {
            if (_path.corners.Length <= 1) 
                return true;

            if (_path.corners.Length == 2 && Vector3.Distance(transform.position, _path.corners[1]) < StoppingDistance)
                return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        public void SetDestination(Vector3 pos)
        {
            // If the destination is already set, we don't need to recalculate
            if (_destination == pos)
                return;
            
            if (_agent.CalculatePath(pos, _path) == false || _path.corners.Any() == false)
            {
                // Failed to calculate the path, try to sample the position
                if (NavMesh.SamplePosition(pos, out var hit, 10f, 1))
                {
                    Debug.Log("Sampled position on NavMesh");
                    _agent.CalculatePath(hit.position, _path);
                    _destination = hit.position;
                }
            }
            else
            {
                // Successfully calculated a new path to the destination
                _destination = pos;
            }

            // for (int i = 0; i < _path.corners.Length; i++)
            // {
            //     Debug.Log($"[{i}] {_path.corners[i]}");
            // }

            DebugDrawHelper.DrawPath(_path, Color.red);
        }

        /// <summary>
        /// Gets the distance along the current path to the destination.
        /// </summary>
        /// <returns>Returns <c>float.MaxValue</c> if not path is set, otherwise returns the distance along the path.</returns>
        public float GetPathLength()
        {
            if (HasDestination() == false)
                return float.MaxValue;

            return _path.GetPathLength();
        }

        /// <summary>
        /// Gets the euclidean distance between the tank and the current destination.
        /// </summary>
        /// <returns>Returns <c>float.MaxValue</c> if not path is set, otherwise returns the distance along the path.</returns>
        public float GetDistanceToDestination()
        {
            if (HasDestination() == false)
                return float.MaxValue;

            return Vector3.Distance(transform.position, _destination);
        }

        public void RecalculatePath() => _agent.CalculatePath(_destination, _path);

        /// <summary>
        /// 
        /// </summary>
        public void ClearDestination()
        {
            _path.ClearCorners();

            // _agent.isStopped = true;
            // _agent.ResetPath();
        }

        /// <summary>
        /// Checks whether the player is visible irregardless of the tank's view cone and vision range.
        /// </summary>
        public bool IsPlayerVisible()
        {
            var player = LevelManager.Instance.Player;

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
            var player = LevelManager.Instance.Player;

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
            var player = LevelManager.Instance.Player;

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
            var player = LevelManager.Instance.Player;

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
            var player = LevelManager.Instance.Player;

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
