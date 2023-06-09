using System;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;
using Util.Helpers;

namespace Actor
{
    [RequireComponent(typeof(PlayerInput))]
    public class TankController : BaseTankController
    {
        private PlayerInput _input;

        private Camera _camera;

        private float _move = 0f;
        private float _turn = 0f;

        private Vector3 _targetPos;

        protected override void Awake()
        {
            base.Awake();

            _input = GetComponent<PlayerInput>();

            // LevelManager.Instance.Player = this;
            GameManager.Instance.OnPauseGameEvent.AddListener(OnPause);
            GameManager.Instance.OnResumeGameEvent.AddListener(OnResume);
        }

        void Start()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            if (Keyboard.current?.escapeKey.wasPressedThisFrame == true)
            {
                GameManager.Instance.PauseGame();
            }
        }

        void FixedUpdate()
        {
            // Aim the turret
            // If we are in a cutscene, we still want to aim the turret. 
            var turretForwardDir = new Vector3(_targetPos.x - transform.position.x, 0f, _targetPos.z - transform.position.z);
            if (turretForwardDir != Vector3.zero)
                _turretTransform.forward = turretForwardDir;

            if (GameManager.Instance.IsPlaying() == false)
                return;
            
            var movement = new Vector3(_move, 0, _turn);
            if (movement != Vector3.zero)
            {
                var movementDir = movement.normalized;

                var dot = Vector3.Dot(transform.forward, movementDir);
                if (Math.Abs(Mathf.Abs(dot) - 1f) < 0.0005f)
                {
                    // same direction; ignore
                    transform.forward = Mathf.Sign(dot) * movementDir;
                    _rb.MovePosition(_rb.position + movement * _moveSpeed * Time.fixedDeltaTime);
                }
                else if (Mathf.Abs(dot) < 0.0005f)
                {
                    // = 90 deg turn
                    var targetRot = movementDir;
                    transform.forward = Vector3.RotateTowards(transform.forward, targetRot,  _turnSpeed * Time.fixedDeltaTime, 0.0f);
                }
                else
                {
                    // > 90 deg turn
                    var targetRot = Mathf.Sign(dot) * movementDir;
                    transform.forward = Vector3.RotateTowards(transform.forward, targetRot, _turnSpeed * Time.fixedDeltaTime, 0.0f);
                }
            }
        }

        public void OnMove(InputValue val)
        {
            var dir = val.Get<Vector2>();

            _move = dir.x;
            _turn = dir.y;
        }

        public void OnAim(InputValue val)
        {
            var mousePos = val.Get<Vector2>();

            _targetPos = _camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _camera.transform.position.y));
        }

        public void OnShoot(InputValue val)
        {
            if (GameManager.Instance.IsPlaying() == false)
                return;

            if (val.isPressed == false) return;

            Shoot();
        }

        public void OnPause() { }

        public void OnResume() { }

        public override void Explode()
        {
           LevelManager.Instance.ResetLevel();
           Destroy(gameObject);
        }
    }
}
