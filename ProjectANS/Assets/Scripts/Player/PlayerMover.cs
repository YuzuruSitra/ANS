using UnityEngine;

namespace Player
{
    public class PlayerMover : MonoBehaviour
    {
        //private Transform _parent;
        [Header("歩行速度")]
        [SerializeField]
        private float _walkSpeed;
        [Header("走行速度")]
        [SerializeField]
        private float _runSpeed;
        [Header("重力係数")]
        [SerializeField]
        private float _gravity;
        private CharacterController _controller;
        private Vector3 _moveDirection = Vector3.zero;
        private Vector3 _direction = Vector3.zero;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            
            _moveDirection.x = horizontal;
            _moveDirection.z = vertical;
            
            var speed = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _walkSpeed;
            
            if (_controller.isGrounded)
                _moveDirection.y = 0;
            _moveDirection.y -= _gravity * Time.deltaTime;
            
            if (horizontal != 0 || vertical != 0)
            {
                _direction.x = horizontal;
                _direction.z = vertical;
                _direction.y = 0;
                transform.rotation = Quaternion.LookRotation(_direction);
            }
            
            _controller.Move(_moveDirection * (speed * Time.deltaTime));
        }
    }
}