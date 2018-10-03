using Panda;
using System;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using GodsGame;

namespace GodsGames
{
    public class Tasks : MonoBehaviour
    {
        // REFERENCES
        public Animator _animator;
        public Rigidbody _rigidbody;
        public NavMeshAgent _agent;
        public List<GameObject> _targets;
        private GameObject _currentTarget;

        // CONFIGURATION
        public string _targetTag;
        public float _attackRange;
        public float _chargeDuration;
        public float _berserkSpeed;
        public float _chargeSpeed;
        public float _defaultSpeed;

        // STATES
        public bool _berserkMode;
        public bool _isCharging;

        // DURATIONS
        private TimeSpan _berserkModeMaxDuration = new TimeSpan(0, 0, 10);
        private TimeSpan _maxFocusTimeOnTarget = new TimeSpan(0, 0, 15);
        private TimeSpan _chargeCooldown = new TimeSpan(0, 0, 3);

        // DATES
        private DateTime _startFocusTimeOnCurrentTarget;
        private DateTime _berserkModeStartTime;
        private DateTime _lastChargeTime = DateTime.Now;

        private const string IS_WALKING = "isWalking";
        private const string CHARGING = "charge";
        private const string ATTACKING = "attack";

        void Start ()
        {
            if (_targets.Count == 0)
                _targets = new List<GameObject>(GameObject.FindGameObjectsWithTag(_targetTag));
	    }

        private void Update()
        {
            _animator.SetBool(IS_WALKING, !_agent.isStopped);
        }

        /**
         * TOOLS
         **/

        void ResetStates ()
        {
            _isCharging = false;
            _berserkMode = false;
        }

        float GetDistFromCurrentTarget()
        {
            if (_currentTarget == null)
                return -1;
            return (_currentTarget.transform.position - transform.position).sqrMagnitude;
        }

        /**
         * TARGET UTILS
         **/

        [Task]
        public bool HasToSwitchTarget ()
        {
            return DateTime.Now - _startFocusTimeOnCurrentTarget > _maxFocusTimeOnTarget;
        }

        [Task]
        public bool HasCurrentTarget ()
        {
            return _currentTarget != null;
        }

        [Task]
        public void AcquireNewTarget()
        {
            float closestDistance = Mathf.Infinity;
            GameObject closestTarget = _currentTarget;

            foreach (GameObject target in _targets)
            {
                if (_currentTarget == target)
                    continue;

                Vector3 diff = transform.position - target.transform.position;
                float distance = diff.sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }

            _currentTarget = closestTarget;
            _startFocusTimeOnCurrentTarget = DateTime.Now;
            Task.current.Succeed();
        }

        /**
         * BERSERK
         **/
        
        [Task]
        public bool IsBerserk()
        {
            return _berserkMode;
        }

        [Task]
        public bool NeedToDeactivateBerserk()
        {
            return _berserkMode && (DateTime.Now - _berserkModeStartTime) > _berserkModeMaxDuration;
        }

        [Task]
        public void DeactivateBerserkMode()
        {
            _berserkMode = false;
            _agent.speed = _defaultSpeed;
            Task.current.Succeed();
        }

        [Task]
        public void ActivateBerserkMode()
        {
            InternalActivateBerserkMode();
            Task.current.Succeed();
        }

        private void InternalActivateBerserkMode()
        {
            _agent.speed = _berserkSpeed;
            _berserkMode = true;
            _berserkModeStartTime = DateTime.Now;
        }

        /**
         * CHARGE
         **/

        [Task]
        public bool CanChargeTarget ()
        {
            return _berserkMode && !_isCharging && DateTime.Now - _lastChargeTime > _chargeCooldown;
        }

        [Task]
        public void ChargeTarget()
        {
            _lastChargeTime = DateTime.Now;
            _isCharging = true;
            _agent.speed = _chargeSpeed;
            _animator.SetTrigger(CHARGING);
            Invoke("StopCharging", _chargeDuration);
            Task.current.Succeed();
        }

        private void StopCharging()
        {
            _isCharging = false;
            _agent.speed = _berserkMode ? _berserkSpeed : _defaultSpeed;
        }

        /**
         * MOVE
         **/

        [Task]
        public bool IsTooFarFromTarget()
        {
            bool isTooFar = _currentTarget != null && GetDistFromCurrentTarget() > _attackRange;

            if (!isTooFar)
                _agent.isStopped = true;

            return isTooFar;
        }

        [Task]
        public void MoveTowardTarget()
        {
            if (_currentTarget == null)
            {
                Task.current.Fail();
                return;
            }

            _agent.SetDestination(_currentTarget.transform.position);
            _agent.isStopped = false;

            Task.current.Succeed();
        }

        /**
         * ATTACK
         **/

        [Task]
        public bool CanAttackTarget ()
        {
            return GetDistFromCurrentTarget() <= _attackRange;
        }

        [Task]
        public void AttackTarget ()
        {
            _animator.SetTrigger(ATTACKING);
            Task.current.Succeed();
        }

        /**
         * IDLE
         **/

        [Task]
        public void Idle ()
        {
            _agent.isStopped = true;
            Task.current.Succeed();
        }

        /**
         * GENERICS
         **/

        [Task]
        public void Success ()
        {
            Task.current.Succeed();
        }

        [Task]
        public void Failure ()
        {
            Task.current.Fail();
        }

        public void OnTakeDamage(Damager damager, Damageable damageable)
        {
            ActivateBerserkMode();
        }
    }
}
