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
        private TimeSpan _lightningCooldown;
        private TimeSpan _shockWaveCooldown;

        // DATES
        private DateTime _startFocusTimeOnCurrentTarget;
        private DateTime _berserkModeStartTime;
        private DateTime _lastShockWaveTime = DateTime.Now;
        private DateTime _lastLightningStrikeTime = DateTime.Now;
        private float _endPrepareShockWaveTime;

        private const string IS_WALKING = "isWalking";
        private const string CHARGING = "charge";
        private const string ATTACKING = "attack";

        private bool _isLighningInvoke = false;
        private PandaBehaviour _bt;

        // SHOKWAVE
        private int _SWIndex = 0;

        [Header("External references")]
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
        public GameObject[] _ShockWaves;

        public float _shockWaveRange;
        public float _shockWaveHeight;
        public float _prepareShockWaveDuration;
        public float _shockWaveDuration;
        public float _preparePhaseShockWaveDelay;
        public float _shockWaveDelayBetweenTwoRocks;
        public float _shockWaveRockLifeTime;
        public float _shockWaveRockMinHeight;
        public float _shockWaveRockMaxHeight;
   
        [Header("States")]
        public bool lightningPhase;
        public bool berserkMode;
        public bool isDead;

        public bool _isShockWavePrepared;
        public bool _isUsingShockWave;

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
            _isUsingShockWave = false;
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
            if (!_isUsingShockWave)
            {
                transform.LookAt(new Vector3(_currentTarget.transform.position.x, transform.position.y, _currentTarget.transform.position.z));
            }
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
            _bt.Wait(_preparePhaseShockWaveDelay, false);
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
            //Vector3 heading = _currentTarget.transform.position - transform.position;
            //Vector3 basePosition = transform.position;
            //Vector3 direction = heading / heading.magnitude;
            //float range = Mathf.Max(_shockWaveRange, heading.magnitude);
            //basePosition.y = 0;
            //direction.y = 0;

            //float i = 0;
            //while (i < range)
            //{
            //    Vector3 initialPosition = basePosition + direction * i;
            //    initialPosition.y = UnityEngine.Random.Range(_shockWaveRockMinHeight, _shockWaveRockMaxHeight);
            //    GameObject boulder = Instantiate(_throwableObjects[UnityEngine.Random.Range(0, _throwableObjects.Count)], initialPosition, new Quaternion());
            //    Destroy(boulder, _shockWaveRockLifeTime);
            //    yield return new WaitForSeconds(_shockWaveDelayBetweenTwoRocks);
            //    i += 1;
            //}
            _ShockWaves[_SWIndex % 2].transform.position = new Vector3(transform.position.x, transform.position.y + _shockWaveHeight, transform.position.z);
            _ShockWaves[_SWIndex % 2].transform.rotation = transform.rotation;
            _ShockWaves[_SWIndex % 2].SetActive(true);
            _ShockWaves[(++_SWIndex) % 2].SetActive(false);
            yield return new WaitForSeconds(1);
            agent.speed = berserkMode ? berserkSpeed : defaultSpeed;
            agent.isStopped = false;
            _isUsingShockWave = false;
            _isShockWavePrepared = false;
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
            StartCoroutine(LoadLevelCompleteScene());
        }

        IEnumerator LoadLevelCompleteScene()
        {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("LevelComplete");
        }
    }
}
