using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float groundDistance = 0.2f;
    public float dashSpeed = 5f;
    public LayerMask ground;
    public Vector3 drag;

    private CharacterController _controller;
    private Vector3 _input;
    private Vector3 _velocity;
    private bool _isGrounded = true;
    private Transform _groundChecker;
    private float _angle;

    private Transform _camera;
    private Quaternion _targetRotation;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _groundChecker = transform.GetChild(0);
        _camera = Camera.main.transform;
    }

    void Update()
    {

        GetInput();

        if (_input != Vector3.zero)
        {

            CalculateDirection();
            Rotate();

            Jump();
            Dash();
            Move();
        }
        ApplyGravity();
        ApplyDrag();
        ApplyNewVelocity();

        //_isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);
        //if (_isGrounded && _velocity.y < 0)
        //    _velocity.y = 0f;

        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //_controller.Move(move * Time.deltaTime * Speed);
        //if (move != Vector3.zero)
        //    transform.forward = move;

        //if (Input.GetButtonDown("Jump") && _isGrounded)
        //    _velocity.y += Mathf.Sqrt(JumpHeight * -2f * Gravity);
        //if (Input.GetButtonDown("Dash"))
        //{
        //    _velocity += Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * Drag.x + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * Drag.z + 1)) / -Time.deltaTime)));
        //}


        //_velocity.y += Gravity * Time.deltaTime;

        //_velocity.x /= 1 + Drag.x * Time.deltaTime;
        //_velocity.y /= 1 + Drag.y * Time.deltaTime;
        //_velocity.z /= 1 + Drag.z * Time.deltaTime;

        //_controller.Move(_velocity * Time.deltaTime);
    }


    /// <summary>
    /// input based on Horizontal(q, d, <, >) and Vertical(z, s, ^, v) keys
    /// </summary>
    private void GetInput()
    {
        _input.x = Input.GetAxis("Horizontal");
        _input.y = 0;
        _input.z = Input.GetAxis("Vertical");
    }

    /// <summary>
    /// Direction relative to the camera's rotation
    /// </summary>
    private void CalculateDirection()
    {
        _angle = Mathf.Atan2(_input.x, _input.z);
        _angle *= Mathf.Rad2Deg;
        _angle += _camera.eulerAngles.y;
    }

    /// <summary>
    /// Rotate toward to calculated angle
    /// </summary>
    private void Rotate()
    {
        _targetRotation = Quaternion.Euler(0, _angle, 0);
        transform.localRotation = _targetRotation;
        //transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, turnSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Move the player along its forward axis
    /// </summary>
    private void Move()
    {
        //transform.position = transform.forward * moveSpeed * Time.deltaTime;
        _controller.Move(transform.forward * Time.deltaTime * moveSpeed);
    }

    /// <summary>
    /// The player jump
    /// </summary>
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
            _velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    /// <summary>
    /// The player dashes
    /// </summary>
    private void Dash()
    {
        if (Input.GetButtonDown("Dash"))
            _velocity += Vector3.Scale(transform.forward, dashSpeed * new Vector3((Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * drag.z + 1)) / -Time.deltaTime)));
    }

    /// <summary>
    /// Apply the gravity force to the player
    /// </summary>
    private void ApplyGravity()
    {
        _velocity.y += gravity * Time.deltaTime;
    }

    /// <summary>
    /// Apply drag forces to the player
    /// </summary>
    private void ApplyDrag()
    {
        _velocity.x /= 1 + drag.x * Time.deltaTime;
        _velocity.y /= 1 + drag.y * Time.deltaTime;
        _velocity.z /= 1 + drag.z * Time.deltaTime;
    }

    private void ApplyNewVelocity()
    {
        _controller.Move(_velocity * Time.deltaTime);
    }
}
