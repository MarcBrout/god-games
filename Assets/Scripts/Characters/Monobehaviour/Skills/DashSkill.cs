using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class DashSkill : CooldownSkill<PlayerBehaviour>
    {
        public float dashSpeed = 5f;
        [Tooltip("The time in seconds at which max speed is reached.")]
        public float maxSpeedReachIn = 1f;
        public AnimationCurve curve;
        private bool _isDahing;

        private float m_Duration;

        public override void StartExecute(bool startCooldown = true)
        {
            base.StartExecute(startCooldown);
            m_Duration = 0;
            _isDahing = true;
            //Add start dash effect
        }

        public override void UpdateExecute()
        {
            m_Duration += Time.deltaTime;
            //Debug.Log(m_MonoBehaviour.CInput.normalized * dashSpeed * curve.Evaluate(m_Duration / maxSpeedReachIn));
            m_MonoBehaviour.SetMoveVector(m_MonoBehaviour.CInput.normalized * dashSpeed * curve.Evaluate(m_Duration / maxSpeedReachIn));
        }

        public override void EndExecute()
        {
            _isDahing = false;
            base.EndExecute();
        }

        public bool IsDashing
        {
            get { return _isDahing; }
        }
    }
}
