using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private CharacterController _controller;
    private Transform _camera;
    private float _horizontal;
    private float _vertical;

    private Animator _animator;

    [SerializeField] private float _playerSpeed = 5f;
    [SerializeField] private float _jumpHeight = 1f;

    private float _gravity = -9.81f;
    private Vector3 _playerGravity;

    private float turnSmoothVelocity;
    [SerializeField] float turnSmoothTime = 0.1f;

    [SerializeField] private Transform _sensorPosition;
    [SerializeField] private float _sensorRadius = 0.2f;
    [SerializeField] private LayerMask _gorundlayer;

    [SerializeField] private bool _isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        Movement();
        Jump();
    }

    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        _animator.SetFloat("VelX", 0);
        _animator.SetFloat("VelZ", direction.magnitude);

        if (direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;

            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            Vector3 moveDirecton = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            _controller.Move(moveDirecton.normalized * _playerSpeed * Time.deltaTime);
        }
    }

    void Jump()
    {
        _isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _gorundlayer);

        

        if(_isGrounded && _playerGravity.y < 0)
        {
            _playerGravity.y = -2;
            _animator.SetBool("isJumping", !_isGrounded);
        }

        if(_isGrounded && Input.GetButtonDown("Jump"))
        {
            _playerGravity.y = _jumpHeight;
        }

        _playerGravity.y += _gravity * Time.deltaTime;
        _controller.Move(_playerGravity * Time.deltaTime);
    }
}
