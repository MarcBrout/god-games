using Panda;
using System;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using GodsGame;
using System.Collections;
using UnityEngine.SceneManagement;

namespace GodsGames
{
    [Serializable]
    public class MinotaurPhase
    {
        [Header("General configuration")]
        public float speed;
        [Header("Durations")]
        public double maxFocusTimeOnTarget;
        public double berserkModeMaxDuration;
        public double lightningCooldown;
        public double shockWaveCooldown;
    }

    public class MinotaurTasks : MonoBehaviour
    {
        // Private members
        private GameObject _currentTarget;

        // DURATIONS
        private TimeSpan _berserkModeMaxDuration;
        private TimeSpan _maxFocusTimeOnTarget;
        //private TimeSpan _chargeCooldown;
        private TimeSpan _lightningCooldown;
        private TimeSpan _shockWaveCooldown;

        // DATES
        private DateTime _startFocusTimeOnCurrentTarget;
        private DateTime _berserkModeStartTime;
        //private DateTime _lastChargeTime = DateTime.Now;
        private DateTime _lastShockWaveTime = DateTime.Now;
        private DateTime _lastLightningStrikeTime = DateTime.Now;
        private float _endPrepareShockWaveTime;

        private const string IS_WALKING = "isWalking";
        private const string CHARGING = "charge";
        private const string ATTACKING = "attack";

        private bool _isLighningInvoke = false;
        private PandaBehaviour _bt;


        [Header("External references")]
        public ScoreManager ScoreManager;
        public GameObject lightningStrike;
        public Animator animator;
        public new Rigidbody rigidbody;
        public NavMeshAgent agent;
        public List<GameObject> targets;
        public List<GameObject> _throwableObjects;

        [Header("General configuration")]
        public string targetTag;
        public float attackRange;
        public float berserkSpeed;
        public float defaultSpeed;
        public float lightningStrikeDelayBeforeDelete = 3.0f;

        [Header("Phases")]
        public MinotaurPhase[] _phasesList;
        public int _startPhase;
        public int _endPhase;
        public int _phasesStep;
        public int _currentPhase;

        [Header("LightningSkill")]
        public float _ligntningInvokeDelay = 0.5f;

        [Header("ShockWaveSkill")]
        public float _prepareShockWaveDuration;
        public float _shockWaveDuration;
        public float preparePhaseShockWaveDelay;

        //[header("chargeskill")]
        //public float preparechargeduration = 1f;
        //public float chargeduration = 0.7f;
        //public float preparephasechargedelay = 0.5f;

        [Header("States")]
        public bool lightningPhase;
        public bool berserkMode;
        public bool isDead;

        public bool _isShockWavePrepared;
        //public bool _isChargePrepared = false;
        public bool _isUsingShockWave;
        //public bool _isCharging = false;

        void Start()
        {
            ResetStates();
            _bt = gameObject.GetComponent<PandaBehaviour>();
            if (targets.Count == 0)
                targets = new List<GameObject>(GameObject.FindGameObjectsWithTag(targetTag));

            _currentPhase = _startPhase;
            ActivatePhase();
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
            lightningPhase = false;
            berserkMode = false;
            isDead = false;

            _isShockWavePrepared = false;
            //public bool _isChargePrepared = false;
            _isUsingShockWave = false;
            //public bool _isCharging = false;
        }

        float GetDistFromCurrentTarget()
        {
            if (_currentTarget == null)
                return -1;
            return (_currentTarget.transform.position - transform.position).sqrMagnitude;
        }

        void UpdateGameDurations(MinotaurPhase phase)
        {
            _berserkModeMaxDuration = TimeSpan.FromSeconds(phase.berserkModeMaxDuration);
            _maxFocusTimeOnTarget = TimeSpan.FromSeconds(phase.maxFocusTimeOnTarget);
            _lightningCooldown = TimeSpan.FromSeconds(phase.lightningCooldown);
            _shockWaveCooldown = TimeSpan.FromSeconds(phase.shockWaveCooldown);
        }

        void UpdateGameConfiguration(MinotaurPhase phase)
        {
            defaultSpeed = phase.speed;
        }

        void ActivatePhase()
        {
            MinotaurPhase phase = _phasesList[_currentPhase];

            UpdateGameDurations(phase);
            UpdateGameConfiguration(phase);
        }

        void GoToNextPhase()
        {
            _currentPhase += _phasesStep;
            if (_currentPhase >= _phasesList.Length || _currentPhase > _endPhase)
                _currentPhase = _endPhase > _phasesList.Length - 1 ? _phasesList.Length - 1 : _endPhase;
            ActivatePhase();
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
                InvokeRepeating("InvokeLightning", 0, _ligntningInvokeDelay);
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
         * SHOCK WAVE
         **/

        [Task]
        public bool IsPreparingShockWave()
        {
            return  Time.time < _endPrepareShockWaveTime;
        }

        [Task]
        public void PrepareShockWave()
        { 
            agent.isStopped = true;
            _endPrepareShockWaveTime = Time.time + _prepareShockWaveDuration;
            _isShockWavePrepared = true;
            Task.current.Succeed();
        }

        [Task]
        public void AimAtTarget()
        {
            transform.LookAt(new Vector3(_currentTarget.transform.position.x, transform.position.y, _currentTarget.transform.position.z));
            Task.current.Succeed();
        }
      
        [Task]
        public void WaitShockWaveDuration()
        {
            _bt.Wait(_shockWaveDuration);
        }

        [Task]
        public void WaitDelayBetweenPreparePhaseAndShockWave()
        {
            _bt.Wait(preparePhaseShockWaveDelay, false);
        }

        [Task]
        public void ShockWave()
        {
            if (!_isUsingShockWave)
            {
                animator.SetTrigger(CHARGING); // TODO: SHOCKWAVE
                _isUsingShockWave = true;
                _lastShockWaveTime = DateTime.Now;
                StartCoroutine(ShockWaveCoroutine());
            }
            Task.current.Succeed();
        }

        private IEnumerator ShockWaveCoroutine()
        {
            Vector3 heading = _currentTarget.transform.position - transform.position;
            float distance = heading.magnitude;
            Vector3 basePosition = transform.position;
            Vector3 direction = heading / distance;
            basePosition.y = 0;
            direction.y = 0;

            float i = 0;
            while (i < distance)
            {
                Vector3 initialPosition = basePosition + direction * i;
                initialPosition.y = 0;
                GameObject boulder = Instantiate(_throwableObjects[UnityEngine.Random.Range(0, _throwableObjects.Count)], initialPosition, new Quaternion());
                Destroy(boulder, 0.5f);
                yield return new WaitForSeconds(0.01f);
                i += 1;
            }
            agent.speed = berserkMode ? berserkSpeed : defaultSpeed;
            agent.isStopped = false;
            _isUsingShockWave = false;
            _isShockWavePrepared = false;
            yield return new WaitForSeconds(1);
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
            return GetDistFromCurrentTarget() <= attackRange && !_isShockWavePrepared && !_isUsingShockWave;
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
            GoToNextPhase();
            if (!lightningPhase && damageable.CurrentHealth <= damageable.startingHealth / 2)
                lightningPhase = true;
            InternalActivateBerserkMode();
        }

        public void OnDieBoss(Damager damager, Damageable damageable)
        {
            isDead = true;
            PlayerPrefs.SetInt("lvl1", (int)Time.timeSinceLevelLoad);
            ScoreManager.AddScore(1, Time.timeSinceLevelLoad);
            StartCoroutine(LoadLevelCompleteScene());
        }

        IEnumerator LoadLevelCompleteScene()
        {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("LevelComplete");
        }
    }
}
