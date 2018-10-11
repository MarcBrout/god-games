using Panda;
using System;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using GodsGame;
using System.Collections;

namespace GodsGames
{
    public class MinotaurTasks : MonoBehaviour
    {
        // REFERENCES
        public GameObject lightningStrike;
        public Damager chargeTelegraph;
        public Animator animator;
        public new Rigidbody rigidbody;
        public NavMeshAgent agent;
        public List<GameObject> targets;
        private GameObject _currentTarget;

        // CONFIGURATION
        public string targetTag;
        public float attackRange;
        public float berserkSpeed;
        public float chargeSpeed;
        public float defaultSpeed;
        public float lightningStrikeDelayBeforeDelete = 3.0f;

        [Header("LightningSkill")]
        public float ligntningInvokeDelay = 0.5f;

        [Header("ChargeSkill")]
        public float prepareChargeDuration = 1f;
        public float chargeDuration = 0.7f;
        public float PreparePhaseChargeDelay = 0.5f;

        [Header("Phase")]
        // STATES
        public bool lightningPhase = false;
        public bool berserkMode = false;
        public bool isDead = false;

        public bool _isChargePrepared = false;
        public bool _isCharging = false;


        // DURATIONS
        private TimeSpan _berserkModeMaxDuration = new TimeSpan(0, 0, 10);
        private TimeSpan _maxFocusTimeOnTarget = new TimeSpan(0, 0, 15);
        private TimeSpan _chargeCooldown = new TimeSpan(0, 0, 3);
        private TimeSpan _lightningCooldown = new TimeSpan(0, 0, 3);
        private float _endPrepareChargeTime;

        // DATES
        private DateTime _startFocusTimeOnCurrentTarget;
        private DateTime _berserkModeStartTime;
        private DateTime _lastChargeTime = DateTime.Now;
        private DateTime _lastLightningStrikeTime = DateTime.Now;

        private const string IS_WALKING = "isWalking";
        private const string CHARGING = "charge";
        private const string ATTACKING = "attack";

        private bool _isLighningInvoke = false;
        private Vector3 _chargeTelepgraphInitialPos;
        private PandaBehaviour _bt;

        void Start()
        {
            _bt = gameObject.GetComponent<PandaBehaviour>();
            if (targets.Count == 0)
                targets = new List<GameObject>(GameObject.FindGameObjectsWithTag(targetTag));
            _chargeTelepgraphInitialPos = chargeTelegraph.transform.localPosition;
        }

        private void Update()
        {
            animator.SetBool(IS_WALKING, !agent.isStopped);
        }

        /**
         * TOOLS
         **/

        void ResetStates()
        {
            _isCharging = false;
            berserkMode = false;
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

            foreach (GameObject target in targets)
            {
                int targetHealth = target.GetComponent<Damageable>().CurrentHealth;

                // Saving the last healthy target
                if (targetHealth != 0)
                    lastValidTarget = target;

                if (target == _currentTarget) {
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
         * LIGHTNING PHASE
         **/
        [Task]
        public bool IsLightningModeActivated()
        {
            return lightningPhase;
        }

        [Task]
        public bool CanInvokeLightningStrike()
        {
            return lightningPhase && DateTime.Now - _lastLightningStrikeTime > _lightningCooldown;
        }

        [Task]
        public void InvokeLightningStrike()
        {
            if (!CanInvokeLightningStrike())
            {
                if (_isLighningInvoke)
                {
                    _isLighningInvoke = false;
                    CancelInvoke("InvokeLightning");
                }
                Task.current.Fail();
                return;
            }
            if (!_isLighningInvoke)
            {
                _isLighningInvoke = true;
                InvokeRepeating("InvokeLightning", 0, ligntningInvokeDelay);
            }
            Task.current.Succeed();
        }

        public void InvokeLightning()
        {
            GameObject lastValidTarget = _currentTarget;
            GameObject targetToAim = _currentTarget;

            foreach (GameObject target in targets)
            {
                int targetHealth = target.GetComponent<Damageable>().CurrentHealth;
                
                if (targetHealth > 0)
                {
                    lastValidTarget = target;
                }

                if (target == _currentTarget)
                {
                    continue;
                }

                targetToAim = target;

                if (targetHealth == 0)
                {
                    targetToAim = lastValidTarget;
                }
            }
            Debug.Log(targetToAim);
            Rigidbody rb = targetToAim.GetComponent<Rigidbody>();
            Vector3 position = targetToAim.transform.position + rb.velocity * Time.deltaTime;
            Quaternion rotation = targetToAim.transform.rotation;
            GameObject obj = Instantiate(lightningStrike, position, rotation);
            Destroy(obj, lightningStrikeDelayBeforeDelete);
            _lastLightningStrikeTime = DateTime.Now;
        }

        /**
         * BERSERK
         **/

        [Task]
        public bool IsBerserk()
        {
            return berserkMode;
        }

        [Task]
        public void DeactivateBerserkMode()
        {
            berserkMode = false;
            agent.speed = defaultSpeed;
            Task.current.Succeed();
        }

        [Task]
        public void ActivateBerserkMode()
        {
            InternalActivateBerserkMode();
            Task.current.Succeed();
        }

        public void InternalActivateBerserkMode()
        {
            agent.speed = berserkSpeed;
            berserkMode = true;
            _berserkModeStartTime = DateTime.Now;
        }

        /**
         * CHARGE
         **/

        [Task]
        public bool IsPreparingCharge()
        {
            return  Time.time < _endPrepareChargeTime;
        }

        [Task]
        public void PrepareCharge()
        {
            agent.isStopped = true;
            _endPrepareChargeTime = Time.time + prepareChargeDuration;
            chargeTelegraph.gameObject.SetActive(true);
            _isChargePrepared = true;
            Task.current.Succeed();
        }

        [Task]
        public void AimAtTarget()
        {
            transform.LookAt(new Vector3(_currentTarget.transform.position.x, transform.position.y, _currentTarget.transform.position.z));
            Task.current.Succeed();
        }
      
        [Task]
        public void WaitChargeDuration()
        {
            _bt.Wait(chargeDuration);
        }

        [Task]
        public void WaitDelayBetweenPreparePhaseAndCharge()
        {
            _bt.Wait(PreparePhaseChargeDelay, false);
        }

        [Task]
        public void Charge()
        {
            if (!_isCharging)
            {
                animator.SetTrigger(CHARGING);
                _isCharging = true;
                chargeTelegraph.transform.SetParent(null);
                chargeTelegraph.Collider.enabled = true;
                _lastChargeTime = DateTime.Now;
                StartCoroutine(ChargeCoroutine());
            }
            Task.current.Succeed();
        }

        private IEnumerator ChargeCoroutine()
        {
            yield return StartCoroutine(transform.MoveOverSeconds(chargeTelegraph.transform.GetChild(0).position, chargeDuration));
            chargeTelegraph.Collider.enabled = false;
            chargeTelegraph.gameObject.SetActive(false);
            chargeTelegraph.transform.SetParent(transform);
            chargeTelegraph.transform.localPosition = _chargeTelepgraphInitialPos;
            agent.speed = berserkMode ? berserkSpeed : defaultSpeed;
            agent.isStopped = false;
            _isCharging = false;
            _isChargePrepared = false;
        }

        /**
         * MOVE
         **/

        [Task]
        public bool IsTooFarFromTarget()
        {
            bool isTooFar = _currentTarget != null && GetDistFromCurrentTarget() > attackRange;

            if (!isTooFar)
                agent.isStopped = true;

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

            agent.SetDestination(_currentTarget.transform.position);
            agent.isStopped = false;

            Task.current.Succeed();
        }

        /**
         * ATTACK
         **/

        [Task]
        public bool CanAttackTarget()
        {
            return GetDistFromCurrentTarget() <= attackRange && !_isChargePrepared && !_isCharging;
        }

        [Task]
        public void AttackTarget()
        {
            animator.SetTrigger(ATTACKING);
            Task.current.Succeed();
        }

        /**
         * DEATH
         **/
        [Task]
        public bool IsDead()
        {
            return isDead;
        }

        /**
         * IDLE
         **/

        [Task]
        public void Idle()
        {
            agent.isStopped = true;
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

        public void OnTakeDamage(Damager damager, Damageable damageable)
        {
            if (!lightningPhase && damageable.CurrentHealth <= damageable.startingHealth / 2)
                lightningPhase = true;
            InternalActivateBerserkMode();
        }

        public void OnDieBoss(Damager damager, Damageable damageable)
        {
            isDead = true;
        }
    }
}
