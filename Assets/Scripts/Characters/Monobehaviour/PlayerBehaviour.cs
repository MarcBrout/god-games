using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using SceneLinkedSMB;

namespace GodsGame
{
    [RequireComponent(typeof(Damageable))]
    [RequireComponent(typeof(Animator))]
    public class PlayerBehaviour : MonoBehaviour
    {
        #region public Var
        public float moveSpeed = 5f;
        public float turnSpeed = 10f;
        public float jumpHeight = 2f;
        public float groundDistance = 0.2f;
        public float dashDistance = 5f;
        public LayerMask ground;
        public bool usingController = false;

        [Space]
        public string verticalAxis = "Vertical_P1";
        public string horizontalAxis = "Horizontal_P1";
        public string rVerticalAxis = "RVertical_P1";
        public string rHorizontalAxis = "RHorizontal_P1";
        public string jumpButton = "Jump_P1";
        public string dashButton = "Dash_P1";
        public string throwItemButton = "ThrowItem_P1";
        #endregion

        #region private Var
        [Space]
        private Transform _GroundChecker;
        private Vector3 _Input;
        private Camera _Camera;
        private Quaternion _TargetRotation;
        private Animator _Animator;
        private ItemHandler _itemHandler;
        #endregion

        #region protected var
        protected readonly int _HashHorizontalSpeedPara = Animator.StringToHash("HorizontalSpeed");
        protected readonly int _HashVerticalSpeedPara = Animator.StringToHash("VerticalSpeed");
        protected readonly int _HashJumpSpeedPara = Animator.StringToHash("JumpSpeed");
        protected readonly int _HashGroundedPara = Animator.StringToHash("Grounded");
        protected readonly int _HashHurtPara = Animator.StringToHash("Hurt");
        protected readonly int _HashDashPara = Animator.StringToHash("Dash");
        protected readonly int _HashUseItemPara = Animator.StringToHash("UseItem");
        protected readonly int _HashUseSwordPara = Animator.StringToHash("UseSword");
        protected readonly int _HashUseShieldPara = Animator.StringToHash("UseShield");
        #endregion

        #region properties
        public Rigidbody Body { get; private set; }
        public Vector3 CInput { get { return _Input; } }
        public DashSkill DashSkill { get; protected set; }
        public bool IsGrounded
        {
            get { return _Animator.GetBool(_HashGroundedPara); }
            set { _Animator.SetBool(_HashGroundedPara, value); }
        }
        public Damageable Damageable { get; private set; }
        #endregion

        void Start()
        {
            Body = GetComponent<Rigidbody>();
            _GroundChecker = transform.GetChild(0);
            _Camera = Camera.main;
            _Animator = GetComponent<Animator>();
            Damageable = GetComponent<Damageable>();
            DashSkill = new DashSkill(this);
            _itemHandler = GetComponent<ItemHandler>();
            SceneLinkedSMB<PlayerBehaviour>.Initialise(_Animator, this);
        }

        public void CheckForGrounded()
        {
            IsGrounded = Physics.CheckSphere(_GroundChecker.position, groundDistance, ground);
        }

        /// <summary>
        /// input based on Horizontal(q, d, <, >) and Vertical(z, s, ^, v) keys
        /// </summary>
        public void GetInput()
        {
            _Input.x = cInput.GetAxisRaw(horizontalAxis);
            _Input.y = 0;
            _Input.z = cInput.GetAxisRaw(verticalAxis);
            _Animator.SetFloat(_HashHorizontalSpeedPara, _Input.x);
            _Animator.SetFloat(_HashVerticalSpeedPara, _Input.z);
        }

        /// <summary>
        /// Move the player along its forward axis
        /// </summary>
        public void Move()
        {
            Body.MovePosition(Body.position + _Input.normalized * moveSpeed * Time.fixedDeltaTime);
        }

        /// <summary>
        /// Check for jump input 
        /// </summary>
        /// <returns></returns>
        public bool CheckForJumpInput()
        {
            return cInput.GetButtonDown(jumpButton) && IsGrounded;
        }

        /// <summary>
        /// The player jump
        /// </summary>
        public void Jump()
        {
            Body.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            _Animator.SetFloat(_HashJumpSpeedPara, Body.velocity.y);
        }

        /// <summary>
        /// Check for dash input
        /// </summary>
        public bool CheckForDashInput()
        {
            return cInput.GetButtonDown(dashButton) && DashSkill.CanUse();
        }

        /// <summary>
        /// The player dashes
        /// </summary>
        public void TransitionToDash()
        {
            _Animator.SetTrigger(_HashDashPara);
        }

        public void Dash()
        {
            Debug.Log("Player execute");
            DashSkill.Execute();
        }

        /// <summary>
        /// Throw the item equiped item of the player
        /// </summary>
        ///
        public bool CheckForThrowInput()
        {
            return cInput.GetButton(throwItemButton) && _itemHandler.CanThrow();
        }
        public void ThrowItem()
        {
            _itemHandler.ThrowItem();
        }

        /// <summary>
        /// Choose the way the layer will rotate according to controls
        /// </summary>
        public void RotateAim()
        {
            if (!usingController)
                MouseAim();
            else
                RJoystickAim();
        }

        /// <summary>
        /// Rotate player with mouse
        /// </summary>
        private void MouseAim()
        {
            Ray cameraRay = _Camera.ScreenPointToRay(Input.mousePosition);
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
}
