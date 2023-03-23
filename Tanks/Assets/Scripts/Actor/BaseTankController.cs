using UnityEngine;

namespace Actor
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseTankController : MonoBehaviour
    {
        [SerializeField] protected GameObject _projectilePrefab;
        [SerializeField] protected Transform _turretTransform;
        [SerializeField] protected Transform _muzzleTransform;

        [SerializeField] protected float _moveSpeed = 1f;
        [SerializeField] protected float _turnSpeed = 45f;

        protected Rigidbody _rb;
        protected Collider _collider;

        

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

        protected virtual void Shoot()
        {
            var projectileGameObject = Instantiate(_projectilePrefab, _muzzleTransform.position, Quaternion.identity);
            var projectile = projectileGameObject.GetComponent<ProjectileController>();
            projectile.SetDirection(_turretTransform.forward);
        }

        public virtual void Explode()
        {
            Destroy(gameObject);
        }
    }
}
