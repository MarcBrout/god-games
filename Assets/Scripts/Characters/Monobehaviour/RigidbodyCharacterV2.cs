using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyCharacterV2 : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    public float jumpHeight = 2f;
    public float groundDistance = 0.2f;
    public float dashDistance = 5f;
    public LayerMask ground;
    public bool usingController = false;
    public string verticalAxis = "Vertical_P1";
    public string horizontalAxis = "Horizontal_P1";
    public string rVerticalAxis = "RVertical_P1";
    public string rHorizontalAxis = "RHorizontal_P1";
    public string jumpButton = "Jump_P1";
    public string dashButton = "Dash_P1";


    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private Transform _groundChecker;
    private Vector3 _input;
    private float _angle;
    private Camera _camera;
    private Quaternion _targetRotation;

    private InputsBuffer<Vector3> _inputBuffer;

    public Vector3 test;

    private readonly Vector3[] diagonalDirections = {
          new Vector3(1, 0, 1),
          new Vector3(-1, 0, 1),
          new Vector3(1, 0, -1),
          new Vector3(-1, 0, -1),
    };


    void Start()
    {
        _inputBuffer = new InputsBuffer<Vector3>();
        _body = GetComponent<Rigidbody>();
        _groundChecker = transform.GetChild(0);
        _camera = Camera.main;

    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, groundDistance, ground);
        GetInput();

        //if (_input == Vector3.zero ||
        //    (_inputBuffer.IsInputBuffered(ExtensionMethods.NorthEstVector3, 0.11f) && (_inputBuffer.IsInputBuffered(Vector3.forward) || _inputBuffer.IsInputBuffered(Vector3.right))) ||
        //    (_inputBuffer.IsInputBuffered(ExtensionMethods.NorthWestVector3, 0.11f) && (_inputBuffer.IsInputBuffered(Vector3.forward) || _inputBuffer.IsInputBuffered(Vector3.left))) ||
        //    (_inputBuffer.IsInputBuffered(ExtensionMethods.SouthEstVector3, 0.11f) && (_inputBuffer.IsInputBuffered(Vector3.back) || _inputBuffer.IsInputBuffered(Vector3.right))) ||
        //    (_inputBuffer.IsInputBuffered(ExtensionMethods.SouthWestVector3, 0.11f) && (_inputBuffer.IsInputBuffered(Vector3.back) || _inputBuffer.IsInputBuffered(Vector3.left))))
        //    return;

        //CalculateDirection();
        //  Rotate();
        Jump();
        Dash();
        if (!usingController)
            MouseAim();
        else
            RJoystickAim();
    }


    void FixedUpdate()
    {
        if (_input == Vector3.zero) return;
        Move();
    }

    /// <summary>
    /// input based on Horizonta(q, d, <, >) and Vertical(z, s, ^, v) keys
    /// </summary>
    private void GetInput()
    {
        _input.x = cInput.GetAxisRaw(horizontalAxis);
        _input.y = 0;
        _input.z = cInput.GetAxisRaw(verticalAxis);
        //_inputBuffer.AddInput(_input);
    }

    /// <summary>
    /// Direction relative to the camera's rotation
    /// </summary>
    private void CalculateDirection()
    {
        _angle = Mathf.Atan2(_input.x, _input.z);
        _angle *= Mathf.Rad2Deg;
        _angle += _camera.transform.eulerAngles.y;
    }

    /// <summary>
    /// Rotate toward to calculated angle
    /// </summary>
    private void Rotate()
    {
        _targetRotation = Quaternion.Euler(0, _angle, 0);
        //transform.localRotation = _targetRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, turnSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Move the player along its forward axis
    /// </summary>
    private void Move()
    {
        _body.MovePosition(_body.position + _input * moveSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// The player jump
    /// </summary>
    private void Jump()
    {
        if (cInput.GetButtonDown(jumpButton) && _isGrounded)
            _body.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    /// <summary>
    /// The player dashes
    /// </summary>
    private void Dash()
    {
        if (cInput.GetButtonDown(dashButton))
        {
            Vector3 dashVelocity = Vector3.Scale(_input, dashDistance *
                new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
            _body.AddForce(dashVelocity, ForceMode.VelocityChange);
        }
    }

    /// <summary>
    /// Rotate player with mouse
    /// </summary>
    private void MouseAim()
    {
        Ray cameraRay = _camera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    /// <summary>
    /// Rotate with controller right joystick
    /// </summary>
    private void RJoystickAim()
    {
        Vector3 playerDirection = Vector3.right * cInput.GetAxisRaw(rHorizontalAxis) + Vector3.forward * cInput.GetAxisRaw(rVerticalAxis);
        if (playerDirection.sqrMagnitude > 0f)
        {
            transform.rotation = Quaternion.LookRotation(playerDirection);
        }
    }
}
