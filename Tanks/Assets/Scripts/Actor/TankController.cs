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

            if (_move != 0f)
                _rb.MovePosition(_rb.position + transform.forward * _move * _moveSpeed * Time.fixedDeltaTime);

            if (_turn != 0f)
                gameObject.transform.Rotate(Vector3.up, _turn * _turnSpeed * Time.fixedDeltaTime);

            var forwardDir = new Vector3(_targetPos.x - transform.position.x, 0f, _targetPos.z - transform.position.z);
            if (forwardDir != Vector3.zero)
                _turretTransform.forward = forwardDir;
        }

        public void OnMove(InputValue val)
        {
            var dir = val.Get<Vector2>();

            _move = dir.y;
        }

        public void OnRotate(InputValue val)
        {
            var dir = val.Get<Vector2>();

            _turn = dir.x;
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

            Shoot();
        }

        public override void Explode()
        {
            // Do not explode
        }
    }
}
