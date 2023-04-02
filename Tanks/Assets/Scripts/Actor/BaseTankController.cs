using UnityEngine;

namespace Actor
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseTankController : MonoBehaviour
    {
        [SerializeField] protected GameObject _projectilePrefab;
        [SerializeField] protected Transform _turretTransform;
        [SerializeField] public Transform _muzzleTransform;

        [SerializeField] protected float _moveSpeed = 2f;
        [SerializeField] [Range(0f, 720f)] private float _turnSpeedDeg = Mathf.PI;
        [SerializeField] protected float _fireRate = 0.5f; // half a second

        protected float _turnSpeed => _turnSpeedDeg * (Mathf.PI / 180f);

        protected Rigidbody _rb;
        protected Collider _collider;

        protected float _lastShotTime = 0;

        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();

            #if UNITY_EDITOR
            if (_projectilePrefab == null)
                Debug.LogWarning("Projectile prefab is not set in the Editor.");

            if (_turretTransform == null)
                Debug.LogWarning("Turret Transform is not set in the Editor.");

            if (_muzzleTransform == null)
                Debug.LogWarning("Muzzle Transform is not set in the Editor.");
            #endif
        }

        public virtual void Shoot()
        {
            var currTime = Time.fixedTime;
            if (_lastShotTime + _fireRate >= currTime)
                return;

            var projectileGameObject = Instantiate(_projectilePrefab, _muzzleTransform.position, Quaternion.identity);
            var projectile = projectileGameObject.GetComponent<ProjectileController>();
            
            projectile.SetDirection(_turretTransform.forward);
            projectile.SetShooter(gameObject);

            _lastShotTime = currTime;
        }

        public virtual void Explode()
        {
            Destroy(gameObject);
        }
    }
}
