using System;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

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

        // TODO: Fire rate

        protected override void Awake()
        {
            base.Awake();

            _input = GetComponent<PlayerInput>();

            #if UNITY_EDITOR
            if (_crosshair == null)
                Debug.LogWarning("Cross hair is not set in the Editor.");
            #endif
            
            GameManager.Instance.Player = this;
        }

        void Start()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            // Debug.DrawLine(transform.position, _targetPos);
        }

        void FixedUpdate()
        {
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

                // else if (dot == 0.5f)
                // {
                //     // perpendicular
                //     transform.forward = Vector3.Lerp(transform.forward, movement, 1f);
                // }
                // else
                // {
                //     // transform.forward = Vector3.Lerp(transform.forward, -movement, 1f);
                // }
            }


            // if (_move != 0f)
            // {
            //     _rb.MovePosition(_rb.position + /*transform.forward * */ _move * _moveSpeed * Time.fixedDeltaTime);
            // }

            // if (_turn != 0f)
            //     gameObject.transform.Rotate(Vector3.up, _turn * _turnSpeed * Time.fixedDeltaTime);

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

        // public void OnRotate(InputValue val)
        // {
        //     var dir = val.Get<Vector2>();
        //
        //     _turn = dir.x;            
        // }

        public void OnAim(InputValue val)
        {
            var mousePos = val.Get<Vector2>();

            _targetPos = _camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _camera.transform.position.y));

            _crosshair.position= mousePos;
        }

        public void OnShoot(InputValue val)
        {
            if (val.isPressed == false) return;

            Shoot();
        }

        public override void Explode()
        {
            // Do not explode
        }
    }
}
