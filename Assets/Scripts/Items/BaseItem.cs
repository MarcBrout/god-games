﻿using UnityEngine;
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
        public CooldownSkill<PlayerBehaviour> skill;
        #endregion

        #region Private Var
        #endregion

        #region Protected Var
        protected Rigidbody m_Rigidbody;
        [SerializeField]
        protected bool m_IsThrowable = true;
        protected Collider m_Collider;
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
            CreateSkill();
        }

        public void PickUpItem(PlayerBehaviour user, GameObject itemSocket)
        {
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

        public virtual void UseItem(bool startCooldown = true)
        {
            skill.Execute(startCooldown);
        }

        public abstract void CreateSkill();
    }
}
