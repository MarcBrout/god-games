using Panda;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodsGame;

namespace GodsGames
{
    public class CyclopTasks : MonoBehaviour
    {
        [Header("General configuration")]
        public GameObject _throwableObject;
        public string _targetTag;
        public List<GameObject> _targets;
        private GameObject _currentTarget;

        [Header("Basic attack")]
        public float _basicAttackGravity;
        public float _basicAttackAngle;
        public float _basicAttackTimeBeforeDestroy;
        private TimeSpan _basicAttackCooldown = new TimeSpan(0, 0, 1);
        private DateTime _basicAttackLastUse;

        [Header("States")]
        public bool _isDead = false;

        private TimeSpan _maxFocusTimeOnTarget = new TimeSpan(0, 0, 6);
        private DateTime _startFocusTimeOnCurrentTarget;
        private PandaBehaviour _bt;

        void Start()
        {
            _bt = gameObject.GetComponent<PandaBehaviour>();
            if (_targets.Count == 0)
                _targets = new List<GameObject>(GameObject.FindGameObjectsWithTag(_targetTag));
        }

        private void Update()
        {

        }

        /**
         * TOOLS
         **/

        void ResetStates()
        {
            _isDead = false;
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
        public bool HasToSwitchTarget()
        {
            return DateTime.Now - _startFocusTimeOnCurrentTarget > _maxFocusTimeOnTarget;
        }

        [Task]
        public bool HasCurrentTarget()
        {
            return _currentTarget != null;
        }

        [Task]
        public void AcquireNewTarget()
        {
            float closestDistance = Mathf.Infinity;
            GameObject closestTarget = _currentTarget;
            GameObject lastValidTarget = _currentTarget;

            foreach (GameObject target in _targets)
            {
                int targetHealth = target.GetComponent<Damageable>().CurrentHealth;

                // Saving the last healthy target
                if (targetHealth != 0)
                    lastValidTarget = target;

                if (target == _currentTarget)
                {
                    continue;
                }

                GameObject targetToAim = target;
                // Changing target to last healthy saved
                if (targetHealth == 0)
                {
                    targetToAim = lastValidTarget;
                }

                Vector3 diff = transform.position - targetToAim.transform.position;
                float distance = diff.sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = targetToAim;
                }
            }

            _currentTarget = closestTarget;
            _startFocusTimeOnCurrentTarget = DateTime.Now;
            Task.current.Succeed();
        }

        /**
         * BASIC ATTACK MECHANICS
         **/

        [Task]
        public bool CanUseBasicAttack()
        {
            return _currentTarget != null && _basicAttackLastUse != null && DateTime.Now - _basicAttackLastUse > _basicAttackCooldown;
        }

        [Task]
        public void UseBasicAttack()
        {
            StartCoroutine("BasicAttackCoroutine");
            _basicAttackLastUse = DateTime.Now;
            Task.current.Succeed();
        }

        private IEnumerator BasicAttackCoroutine()
        {
            Vector3 offset = new Vector3(0, transform.localScale.y, 0);
            GameObject thrownItem = GameObject.Instantiate(_throwableObject, transform.position + offset, new Quaternion());

            float targetDistance = Vector3.Distance(thrownItem.transform.position, _currentTarget.transform.position);
            float velocity = targetDistance / (Mathf.Sin(2 * _basicAttackAngle * Mathf.Deg2Rad) / _basicAttackGravity);
            float vx = Mathf.Sqrt(velocity) * Mathf.Cos(_basicAttackAngle * Mathf.Deg2Rad);
            float vy = Mathf.Sqrt(velocity) * Mathf.Sin(_basicAttackAngle * Mathf.Deg2Rad);
            float flightDuration = targetDistance / vx;

            thrownItem.transform.LookAt(_currentTarget.transform);

            for (float elapsedTime = 0; elapsedTime < flightDuration; elapsedTime += Time.deltaTime)
            {
                thrownItem.transform.Translate(0, (vy - (_basicAttackGravity * elapsedTime)) * Time.deltaTime, vx * Time.deltaTime);
                yield return null;
            }

            GameObject.Destroy(thrownItem, _basicAttackTimeBeforeDestroy);
        }

        /**
         * DEATH
         **/
        [Task]
        public bool IsDead()
        {
            return _isDead;
        }

        /**
         * IDLE
         **/

        [Task]
        public void Idle()
        {
            Task.current.Succeed();
        }

        /**
         * GENERICS
         **/

        [Task]
        public void Success()
        {
            Task.current.Succeed();
        }

        [Task]
        public void Failure()
        {
            Task.current.Fail();
        }

        public void OnDieBoss(Damager damager, Damageable damageable)
        {
            _isDead = true;
        }
    }
}
