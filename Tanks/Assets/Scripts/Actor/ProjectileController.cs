using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [Header("Effects")] [SerializeField] private ParticleSystem _explosionParticleSystem;

    [Header("Settings")] [SerializeField] private float _speed;
    [SerializeField] private int _bounces;

    [HideInInspector] public Vector3 _direction;

    private int _remainingBounces;
    // private bool _canCollideWithPlayer = false;

    void Awake()
    {
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
            Explode();

            collision.gameObject.GetComponent<BaseTankController>().Explode();
        }
    }

    public void SetDirection(Vector3 dir)
    {
        _direction = dir;
        transform.forward = dir;
    }

    void Explode()
    {
        _explosionParticleSystem.transform.parent = null;
        _explosionParticleSystem.Play();

        // TODO: Play audio effect

        var mainModule = _explosionParticleSystem.main;
        Destroy(_explosionParticleSystem.gameObject, mainModule.duration);

        Destroy(gameObject);
    }
}
