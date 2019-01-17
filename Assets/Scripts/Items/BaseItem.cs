using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GodsGame
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseItem : CooldownSkill<PlayerBehaviour>
    {
        #region Public Var
        public Vector3 pickUpPosition;
        public Vector3 pickUpRotations;
        public Sprite spriteUI;
        public GameObject pickUpOrb;
        #endregion

        #region Private Var
        GameObject m_currentOwner;
        #endregion

        #region Protected Var
        protected Rigidbody m_Rigidbody;
        [SerializeField]
        protected bool m_IsThrowable = true;
        protected Collider m_Collider;
        protected SphereCollider m_SphereCollider;
        protected MeshRenderer m_MeshRenderer;
        protected bool m_using = false;
        //protected Animator m_animator;
        #endregion

        #region Properties
        public int TriggerAnimatorHash { get; protected set; }
        public bool IsThrowable { get { return m_IsThrowable; } }
        public bool Using { get { return m_using; } set { m_using = value; } }
        #endregion

        #region Event
        public UnityEvent OnPickUp;
        public UnityEvent OnDrop;
        public UnityEvent OnThrow;
        #endregion

        public override void Start()
        {
            base.Start();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Collider = GetComponent<Collider>();
            m_SphereCollider = GetComponent<SphereCollider>();
            m_MeshRenderer = GetComponent<MeshRenderer>();
            m_currentOwner = null;
            //m_animator = GetComponent<Animator>();
        }

        public void PickUpItem(PlayerBehaviour user, GameObject itemSocket)
        {
            DisplaySword();
            m_Collider.isTrigger = true;
            m_Rigidbody.isKinematic = true;
            m_Rigidbody.detectCollisions = false;
            transform.SetParent(itemSocket.transform);
            transform.localPosition = pickUpPosition;
            transform.localEulerAngles = pickUpRotations;
            user.gameObject.GetComponent<PlayerEventReceiver>().OnSwordPickUp();
            m_currentOwner = user.gameObject;
            AssignUser(user);
            OnPickUp.Invoke();
        }

        public void DropItem()
        {
            m_Collider.isTrigger = false;
            m_Rigidbody.isKinematic = false;
            this.DelayAction(0.1f, () => { m_Rigidbody.detectCollisions = true; });
            m_currentOwner.gameObject.GetComponent<PlayerEventReceiver>().OnSwordDrop();
            transform.SetParent(null);
            DisplayOrb();
            OnDrop.Invoke();
        }

        public void ThrowItem(Transform direction, float force)
        {
            if (!m_using)
            {
                DropItem();
                transform.rotation = direction.rotation;
                m_Rigidbody.AddForce(direction.forward * force + direction.up * force, ForceMode.Impulse);
                m_Rigidbody.AddRelativeTorque(Vector3.left * force, ForceMode.Impulse);
                m_currentOwner.gameObject.GetComponent<PlayerEventReceiver>().OnSwordDrop();
                OnThrow.Invoke();
            }
        }

        private void DisplayOrb()
        {
            m_Collider.enabled = false;
            m_SphereCollider.enabled = true;
            pickUpOrb.SetActive(true);
            //m_animator.SetBool("isEquipped", false);

        }

        private void DisplaySword()
        {
            m_Collider.enabled = true;
            m_SphereCollider.enabled = false;
            pickUpOrb.SetActive(false);
            //m_animator.SetBool("isEquipped", true);
        }

        public virtual void UseItem(bool startCooldown = true)
        {
            StartExecute(startCooldown);
        }

        /*
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                if (!m_animator.GetBool("isEquipped")) {
                    m_animator.enabled = true;
                }
            {
            }
        }*/
    }
}
