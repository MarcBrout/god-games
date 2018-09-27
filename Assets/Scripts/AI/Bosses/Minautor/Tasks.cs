using Panda;
using System;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace GodsGames
{
    public class Tasks : MonoBehaviour
    {
        public NavMeshAgent _agent;
        public List<GameObject> _targets;
        public float _attackRange;
        public string _targetTag;
        public Animator _animator;
        private TimeSpan _maxFocusTimeOnTarget = new TimeSpan(0, 0, 0, 15);
        private DateTime _startFocusTimeOnCurrentTarget;
        private GameObject _currentTarget;

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
         * CHARGE
         **/

        [Task]
        public bool CanChargeTarget ()
        {
            // TODO: Use skills here
            return false;
        }

        [Task]
        public void ChargeTarget()
        {
            _animator.SetTrigger(CHARGING);
            Task.current.Succeed();
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
    }
}
