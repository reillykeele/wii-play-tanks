using UnityEngine;
using UnityEngine.InputSystem;
using Util.Helpers;

namespace Actor
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInput))]
    public class TankController : MonoBehaviour
    {
        [SerializeField] private RectTransform _crosshair;

        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private float _turnSpeed = 45f;

        private Rigidbody _rb;
        private PlayerInput _input;

        private Transform _cannonTransform;

        private Camera _camera;

        private float _move = 0f;
        private float _turn = 0f;

        private Vector3 _targetPos;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _input = GetComponent<PlayerInput>();

            _cannonTransform = gameObject.GetChildObject("Cannon").transform;
        }

        void Start()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            Debug.DrawLine(transform.position, _targetPos);
        }

        void FixedUpdate()
        {

            if (_move != 0f)
                _rb.MovePosition(_rb.position + transform.forward * _move * _moveSpeed * Time.fixedDeltaTime);

            if (_turn != 0f)
                gameObject.transform.Rotate(Vector3.up, _turn * _turnSpeed * Time.fixedDeltaTime);

            _cannonTransform.forward = new Vector3(_targetPos.x - transform.position.x, 0f, _targetPos.z - transform.position.z);
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

            if (_crosshair != null)
                _crosshair.position= mousePos;
            else 
                Debug.LogWarning("Cross hair is not set in the Editor.");
        }
    }
}