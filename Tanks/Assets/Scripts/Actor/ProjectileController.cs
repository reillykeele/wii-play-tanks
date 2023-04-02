using System.Linq;
using Cinemachine;
using UnityEngine;

namespace Actor
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class ProjectileController : MonoBehaviour
    {
        private CinemachineImpulseSource _impulseSource;

        [Header("Effects")] 
        [SerializeField] private ParticleSystem _explosionParticleSystem;
        [SerializeField] private float _impulseForce = 0.1f;

        [Header("Settings")] 
        [SerializeField] private float _speed;
        [SerializeField] private int _bounces;

        [HideInInspector] private GameObject _shooter;
        [HideInInspector] private Vector3 _direction;

        private int _remainingBounces;

        void Awake()
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();

            _remainingBounces = _bounces;
        }

        void FixedUpdate()
        {
            transform.position += _direction * _speed * Time.fixedDeltaTime;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Projectile")
            {
                Explode();
            }
            else if (collision.gameObject.tag == "Wall")
            {
                if (_remainingBounces <= 0)
                    Explode();
                else
                {
                    _remainingBounces--;

                    var contact = collision.contacts.First();
                    var dir = Vector3.Reflect(_direction, contact.normal);

                    SetDirection(dir);
                }
            }
            else if (collision.gameObject.tag == "Enemy" ||
                     collision.gameObject.tag == "Player")
            {
                // If the projectile collides with the shooter and we haven't bounced
                // off any walls, we want to ignore the collision
                if (collision.gameObject == _shooter && _remainingBounces == _bounces)
                    return;

                Explode();

                collision.gameObject.GetComponent<BaseTankController>().Explode();
            }
        }

        public void SetShooter(GameObject shooter) => _shooter = shooter;
        
        public void SetDirection(Vector3 dir)
        {
            _direction = dir;
            transform.forward = dir;
        }

        void Explode()
        {
            var explosionDir = -transform.position.normalized;
            _impulseSource.GenerateImpulse(explosionDir * _impulseForce);

            _explosionParticleSystem.transform.parent = null;
            _explosionParticleSystem.Play();

            // TODO: Play audio effect

            var mainModule = _explosionParticleSystem.main;
            Destroy(_explosionParticleSystem.gameObject, mainModule.duration);

            Destroy(gameObject);
        }
    }
}
