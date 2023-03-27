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
        [SerializeField] private RectTransform _crosshair;

        private PlayerInput _input;

        private Camera _camera;

        private float _move = 0f;
        private float _turn = 0f;

        private Vector3 _targetPos;

        protected override void Awake()
        {
            base.Awake();

            _input = GetComponent<PlayerInput>();

            #if UNITY_EDITOR
            if (_crosshair == null)
                Debug.LogWarning("Cross hair is not set in the Editor.");
            #endif
            
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
            // TODO: Smooth rotation
            var movement = new Vector3(_move, 0, _turn);
            if (movement != Vector3.zero)
            {
                _rb.MovePosition(_rb.position + movement * _moveSpeed * Time.fixedDeltaTime);

                var movementDir = movement.normalized;
                
                var dot = Vector3.Dot(transform.forward, movementDir);
                if (Math.Abs(Mathf.Abs(dot) - 1f) < 0.005f)
                {
                    // same direction
                }
                else if (Mathf.Abs(dot) < 0.005f)
                {
                    // 90 deg turn
                    var f = movementDir;
                    var _ = Vector3.zero;
                    transform.forward = f; // Vector3.SmoothDamp(transform.forward, f, ref _, 0.01f);
                }
                else
                {
                    var f = Mathf.Sign(dot) * movementDir;
                    var _ = Vector3.zero;
                    transform.forward = f; Vector3.SmoothDamp(transform.forward, f, ref _, 0.01f);
                }
            }

            var forwardDir = new Vector3(_targetPos.x - transform.position.x, 0f, _targetPos.z - transform.position.z);
            if (forwardDir != Vector3.zero)
                _turretTransform.forward = forwardDir;
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

            _crosshair.position= mousePos;
        }

        public void OnShoot(InputValue val)
        {
            if (val.isPressed == false) return;
            if (GameManager.Instance.IsPaused()) return;

            Shoot();
        }

        public void OnPause()
        {
            _crosshair.gameObject.Disable();
        }

        public void OnResume()
        {
            _crosshair.gameObject.Enable();
        }

        public override void Explode()
        {
            // Do not explode
        }
    }
}
