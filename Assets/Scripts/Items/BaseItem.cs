using UnityEngine;
using UnityEngine.Events;

namespace GodsGame
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseItem : MonoBehaviour
    {
        #region Public Var
        public Vector3 pickUpPosition;
        public Vector3 pickUpRotations;
        public Sprite spriteUI;
        public GameObject pickUpOrb;
        public CooldownSkill<PlayerBehaviour> skill;
        #endregion

        #region Private Var
        #endregion

        #region Protected Var
        protected Rigidbody m_Rigidbody;
        [SerializeField]
        protected bool m_IsThrowable = true;
        protected Collider m_Collider;
        protected SphereCollider m_SphereCollider;
        protected MeshRenderer m_MeshRenderer;
        #endregion

        #region Properties
        public int TriggerAnimatorHash { get; protected set; }
        public bool IsThrowable { get { return m_IsThrowable; } }
        #endregion

        #region Event
        public UnityEvent OnPickUp;
        public UnityEvent OnDrop;
        public UnityEvent OnThrow;
        #endregion

        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Collider = GetComponent<Collider>();
            m_SphereCollider = GetComponent<SphereCollider>();
            m_MeshRenderer = GetComponent<MeshRenderer>();
            CreateSkill();
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
            if (skill != null)
                skill.AssignUser(user);
            OnPickUp.Invoke();
        }

        public void DropItem()
        {
            m_Collider.isTrigger = false;
            m_Rigidbody.isKinematic = false;
            this.DelayAction(0.1f, () => { m_Rigidbody.detectCollisions = true; });
            transform.SetParent(null);
            DisplayOrb();
            OnDrop.Invoke();
        }

        public void ThrowItem(Transform direction, float force)
        {
            DropItem();
            transform.rotation = direction.rotation;
            m_Rigidbody.AddForce(direction.forward * force + direction.up * force, ForceMode.Impulse);
            m_Rigidbody.AddRelativeTorque(Vector3.left * force, ForceMode.Impulse);
            OnThrow.Invoke();
        }

        private void DisplayOrb()
        {
            m_MeshRenderer.enabled = false;
            m_Collider.enabled = false;
            m_SphereCollider.enabled = true;
            pickUpOrb.SetActive(true);
        }

        private void DisplaySword()
        {
            m_MeshRenderer.enabled = true;
            m_Collider.enabled = true;
            m_SphereCollider.enabled = false;
            pickUpOrb.SetActive(false);
        }

        public virtual void UseItem(bool startCooldown = true)
        {
            skill.StartExecute(startCooldown);
        }

        public abstract void CreateSkill();
    }
}
