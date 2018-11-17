using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class CharacterController : MonoBehaviour
    {
        Rigidbody m_Rigidbody;
        CapsuleCollider m_Capsule;
        Vector3 m_PreviousPosition;
        Vector3 m_CurrentPosition;
        Vector3 m_NextMovement;

        public bool IsGrounded { get; protected set; }
        public bool IsCeilinged { get; protected set; }
        public Vector3 Velocity { get; protected set; }
        public Rigidbody Rigidbody { get { return m_Rigidbody; } }

        void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            m_CurrentPosition = m_Rigidbody.position;
            m_PreviousPosition = m_Rigidbody.position;
        }

        void FixedUpdate()
        {
            m_PreviousPosition = m_Rigidbody.position;
            m_CurrentPosition = m_PreviousPosition + m_NextMovement;
            Velocity = (m_CurrentPosition - m_PreviousPosition) / Time.fixedDeltaTime;
            m_Rigidbody.MovePosition(m_CurrentPosition);
            m_NextMovement = Vector3.zero;
        }

        /// <summary>
        /// This moves a rigidbody and so should only be called from FixedUpdate or other Physics messages.
        /// </summary>
        /// <param name="movement">The amount moved in global coordinates relative to the rigidbody2D's position.</param>
        public void Move(Vector3 movement)
        {
            m_NextMovement += movement;
        }

        /// <summary>
        /// This moves the character without any implied velocity.
        /// </summary>
        /// <param name="position">The new position of the character in global space.</param>
        public void Teleport(Vector3 position)
        {
            Vector3 delta = position - m_CurrentPosition;
            m_PreviousPosition += delta;
            m_CurrentPosition = position;
            m_Rigidbody.MovePosition(position);
        }
    }
}