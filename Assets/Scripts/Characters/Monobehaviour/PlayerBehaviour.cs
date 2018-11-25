using UnityEngine;
using SceneLinkedSMB;
using UnityEngine.AI;

namespace GodsGame
{
    [RequireComponent(typeof(Damageable))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerBehaviour : MonoBehaviour
    {
        #region public Var
        [Header("Movement")]
        public float moveSpeed = 5f;
        public float turnSpeed = 10f;
        public float groundAcceleration = 100f;
        public float groundDeceleration = 100f;
        public float jumpHeight = 2f;
        public float groundDistance = 0.2f;
        public float dashSpeed = 5f;
        public LayerMask ground;
        public bool usingController = false;
        public bool useEightDirectionMovement = false;

        [Header("InputNames")]
        public string verticalAxis = "Vertical_P1";
        public string horizontalAxis = "Horizontal_P1";
        public string rVerticalAxis = "RVertical_P1";
        public string rHorizontalAxis = "RHorizontal_P1";
        public string jumpButton = "Jump_P1";
        public string dashButton = "Dash_P1";
        public string throwItemButton = "ThrowItem_P1";
        public string useItemButton = "UseItem_P1";

        [Header("DustPool")]
        public float dustEffectRepeatDelay = 0.3f;
        #endregion

        #region private Var
        private Transform _GroundChecker;
        private Vector3 _Input;
        private Vector3 _CurrentDirection;
        private Vector3 m_MoveVector;
        private Camera _Camera;
        private Quaternion _TargetRotation;
        private Animator _Animator;
        private ItemHandler _itemHandler;
        private DustEffectPool _DustEffectPool;
        private CharacterController _CharacterController;
        #endregion

        #region protected var
        protected readonly int _HashHorizontalSpeedPara = Animator.StringToHash("HorizontalSpeed");
        protected readonly int _HashVerticalSpeedPara = Animator.StringToHash("VerticalSpeed");
        protected readonly int _HashJumpSpeedPara = Animator.StringToHash("JumpSpeed");
        protected readonly int _HashGroundedPara = Animator.StringToHash("Grounded");
        protected readonly int _HashHurtPara = Animator.StringToHash("Hurt");
        protected readonly int _HashDashPara = Animator.StringToHash("Dash");
        protected readonly int _HashIdlePara = Animator.StringToHash("Idle");
        protected readonly int _HashUseItemPara = Animator.StringToHash("UseItem");
        protected readonly int _HashUseSwordPara = Animator.StringToHash("UseSword");
        protected readonly int _HashUseShieldPara = Animator.StringToHash("UseShield");
        protected readonly int _HashDiedPara = Animator.StringToHash("Died");
        protected readonly int _HashHitPara = Animator.StringToHash("Hit");
        #endregion

        #region properties
        public NavMeshAgent NavMeshAgent { get; private set; }
        public CapsuleCollider CapsuleCollider { get; private set; }
        public Rigidbody Body { get; private set; }
        public Vector3 CInput { get { return _Input; } }
        public Damageable Damageable { get; private set; }
        public bool IsGrounded
        {
            get { return _Animator.GetBool(_HashGroundedPara); }
            set { _Animator.SetBool(_HashGroundedPara, value); }
        }
        public DashSkill DashSkill { get; protected set; }
        #endregion

        private void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            CapsuleCollider = GetComponent<CapsuleCollider>();
            _DustEffectPool = GetComponent<DustEffectPool>();
            _CharacterController = GetComponent<CharacterController>();
            _CurrentDirection = new Vector3(0, 0, 0);
            Body = GetComponent<Rigidbody>();
            _GroundChecker = transform.GetChild(0);
            _Camera = Camera.main;
            _Animator = GetComponent<Animator>();
            Damageable = GetComponent<Damageable>();
            DashSkill = new DashSkill(this);
            _itemHandler = GetComponent<ItemHandler>();
        }

        private void Start()
        {
            SceneLinkedSMB<PlayerBehaviour>.Initialise(_Animator, this);
        }

        /// <summary>
        /// Check if the player is on the ground
        /// </summary>
        public void CheckForGrounded()
        {
            IsGrounded = Physics.CheckSphere(_GroundChecker.position, groundDistance, ground);
        }

        /// <summary>
        /// Check if the player is idle
        /// </summary>
        /// <returns></returns>
        public bool CheckForIdle()
        {
            return _Input.x == 0 && _Input.z == 0;
        }

        public void GoToIdleState()
        {
            _Animator.SetTrigger(_HashIdlePara);
        }

        /// <summary>
        /// input based on Horizontal(q, d, <, >) and Vertical(z, s, ^, v) keys
        /// </summary>
        public void GetInput()
        {
            _Input.x = cInput.GetAxisRaw(horizontalAxis);
            _Input.y = 0;
            _Input.z = cInput.GetAxisRaw(verticalAxis);
        }

        public void GroundedHorizontalMovement(bool useInput, float speedScale = 1f)
        {
            Vector3 input = _Input.normalized;
            float desiredSpeedH = useInput ? input.x * moveSpeed * speedScale : 0f;
            float desiredSpeedV = useInput ? input.z * moveSpeed * speedScale : 0f;
            float accelerationH = useInput && input.z != 0 ? groundAcceleration : groundDeceleration;
            float accelerationV = useInput && input.z != 0 ? groundAcceleration : groundDeceleration;
            m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeedH, accelerationH * Time.deltaTime);
            m_MoveVector.z = Mathf.MoveTowards(m_MoveVector.z, desiredSpeedV, accelerationV * Time.deltaTime);
        }

        // Public functions - called mostly by StateMachineBehaviours in the character's Animator Controller but also by Events.
        public void SetMoveVector(Vector3 newMoveVector)
        {
            m_MoveVector = newMoveVector;
        }

        public void SetHorizontalMovement(float newHorizontalMovement)
        {
            m_MoveVector.x = newHorizontalMovement;
        }

        public void SetVerticalMovement(float newVerticalMovement)
        {
            m_MoveVector.z = newVerticalMovement;
        }

        public void IncrementMovement(Vector3 additionalMovement)
        {
            m_MoveVector += additionalMovement;
        }

        public void IncrementHorizontalMovement(float additionalHorizontalMovement)
        {
            m_MoveVector.x += additionalHorizontalMovement;
        }

        public void IncrementVerticalMovement(float additionalVerticalMovement)
        {
            m_MoveVector.z += additionalVerticalMovement;
        }

        /// <summary>
        /// Transform the player input so the correct animation is played relative to the player rotation
        /// </summary>
        private void TransformInputRelativelyToMouse()
        {
            Vector3 mousePos = _Camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 localPos = transform.InverseTransformPoint(mousePos).normalized;
            localPos.y = 0;
            // Get the angle we should rotate the input. This angle is equal to zero wen the player is facing top
            float refAngle = Vector3.SignedAngle(Vector3.back, localPos, Vector3.up);
            //Multiply the input vector by the refAngle 
            Vector3 newInput = Quaternion.Euler(0, refAngle, 0) * _Input;
            _Animator.SetFloat(_HashHorizontalSpeedPara, newInput.x);
            _Animator.SetFloat(_HashVerticalSpeedPara, newInput.z);
        }

        private void FixedUpdate()
        {
            if (useEightDirectionMovement)
                TransformInputRelativelyToMouse();
            else
            {
                _Animator.SetFloat(_HashHorizontalSpeedPara, _Input.x);
                _Animator.SetFloat(_HashVerticalSpeedPara, _Input.z);
            }
            if (!useEightDirectionMovement && _Input.normalized != _CurrentDirection && CheckForIdle() == false)
            {
                _CurrentDirection = _Input.normalized;
                transform.LookAt(new Vector3(_CurrentDirection.x * 180, _CurrentDirection.y, _CurrentDirection.z * 180));
            }
            _CharacterController.Move(m_MoveVector * Time.fixedDeltaTime);
        }

        public void DoStepDust()
        {
            if (CheckForIdle())
                _DustEffectPool.StopStepDust();
            else
                _DustEffectPool.StartStepDust(dustEffectRepeatDelay);
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
        /// Use the item equiped item of the player
        /// </summary>
        ///
        public bool CheckForUseItemInput()
        {
            return cInput.GetButton(useItemButton) && _itemHandler.CanUseItem();
        }

        public void UseItem()
        {
            //_Animator.SetTrigger(_HashUseItemPara);
            _Animator.SetTrigger(_itemHandler.Item.TriggerAnimatorHash);
            _itemHandler.UseItem();
        }

        /// <summary>
        /// Choose the way the layer will rotate according to controls
        /// </summary>
        public void RotateAim()
        {
            if (useEightDirectionMovement)
            {
                if (!usingController)
                    MouseAim();
                else
                    RJoystickAim();
            }
        }

        /// <summary>
        /// Rotate the player in the choosen direction
        /// </summary>
        /// <param name="aimDirection"></param>
        public void RotateAim(Vector3 aimDirection)
        {
            transform.rotation = Quaternion.LookRotation(aimDirection);
        }

        public void GetHit(Damager damager, Damageable damageable)
        {
            if (damageable.CurrentHealth > 0)
                _Animator.SetTrigger(_HashHitPara);
        }

        public void Die(Damager damager, Damageable damageable)
        {
            _Animator.SetTrigger(_HashDiedPara);
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
                Vector3 lookAtPoint = cameraRay.GetPoint(rayLength);
                Debug.DrawLine(cameraRay.origin, lookAtPoint, Color.blue);

                transform.LookAt(new Vector3(lookAtPoint.x, transform.position.y, lookAtPoint.z));
            }
        }

        /// <summary>
        /// Rotate with controller right joystick
        /// </summary>
        private void RJoystickAim()
        {
            Vector3 direction = Vector3.right * cInput.GetAxisRaw(rHorizontalAxis) + Vector3.forward * cInput.GetAxisRaw(rVerticalAxis);
            if (direction.sqrMagnitude > 0f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}
