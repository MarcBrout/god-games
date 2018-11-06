using Panda;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodsGame;
using UnityEngine.AI;

namespace GodsGames
{
    public class CyclopTasks : MonoBehaviour
    {
        public ScoreManager ScoreManager;
        [Header("General configuration")]
        public GameObject[] _throwableObjects;
        public string _targetTag;
        public List<GameObject> _targets;
        private GameObject _currentTarget;
        public Animator _animator;
        public GameObject[] _ropes;
 
        [Header("Basic attack")]
        public float _basicAttackGravity;
        public float _basicAttackAngle;
        public float _basicAttackDuration;
        public float _basicAttackLifetime;
        private TimeSpan _basicAttackCooldown = new TimeSpan(0, 0, 1);
        private DateTime _basicAttackLastUse;

        [Header("Rain of rocks")]
        public int _rorRocksCount;
        public float _rorTimeBetweenRocks;
        public float _rorSpawnRadius;
        public float _rorSpawnHeight;
        public float _rorMass;
        public float _rorSpawnImpulsion;
        public float _rorRockLifetime;

        [Header("Evolved attack")]
        public float _evolvedAttackRockCount;
        public float _evolvedAttackGravity;
        public float _evolvedAttackAngle;
        public float _evolvedAttackDuration;
        public float _evolvedAttackLifetime;
        public float _evolvedAttackTimeBetweenRock;
        private TimeSpan _evolvedAttackCooldown = new TimeSpan(0, 0, 1);
        private DateTime _evolvedAttackLastUse;

        [Header("States")]
        public bool _isDead = false;
        public bool _evolvedAttackActivated = false;
        public bool _rainOfRocksAvailable = false;
        public bool _isUsingRainOfRocks = false;

        private TimeSpan _maxFocusTimeOnTarget = new TimeSpan(0, 0, 6);
        private DateTime _startFocusTimeOnCurrentTarget;
        private PandaBehaviour _bt;
        private AudioSource _audioSource;

        void Start()
        {
            _bt = gameObject.GetComponent<PandaBehaviour>();
            _audioSource = GetComponent<AudioSource>();
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
            _rainOfRocksAvailable = false;
            _isUsingRainOfRocks = false;
            _evolvedAttackActivated = false;
        }

        float GetDistFromCurrentTarget()
        {
            if (_currentTarget == null)
                return -1;
            return (_currentTarget.transform.position - transform.position).sqrMagnitude;
        }

        private IEnumerator ThrowItemCoroutine(Vector3 targetPosition, float angle, float gravity, float duration, float lifetime)
        {
            Vector3 offset = new Vector3(0, transform.localScale.y, 0);
            GameObject thrownItem = GameObject.Instantiate(_throwableObjects[UnityEngine.Random.Range(0, _throwableObjects.Length)], transform.position + offset, new Quaternion());

            float targetDistance = Vector3.Distance(thrownItem.transform.position, targetPosition);
            float velocity = targetDistance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);
            float vx = Mathf.Sqrt(velocity) * Mathf.Cos(angle * Mathf.Deg2Rad);
            float vy = Mathf.Sqrt(velocity) * Mathf.Sin(angle * Mathf.Deg2Rad);
            float flightDuration = targetDistance / vx;

            thrownItem.transform.LookAt(targetPosition);

            for (float elapsedTime = 0; elapsedTime < flightDuration; elapsedTime += Time.deltaTime)
            {
                thrownItem.transform.Translate(0, (vy - (gravity * elapsedTime)) * Time.deltaTime, vx * Time.deltaTime);
                yield return null;
            }

            StartCoroutine(DeactivateDamage(duration, thrownItem.GetComponent<Damager>()));
            Destroy(thrownItem, lifetime);
        }

        private IEnumerator DeactivateDamage(float seconds, Damager damager)
        {
            yield return new WaitForSeconds(seconds);
            damager.DisableDamage();
        }

        /**
         * TARGET UTILS
         **/

        [Task]
        public bool HasToSwitchTarget()
        {
            bool hasInvalidTarget = HasCurrentTarget() && _currentTarget.GetComponent<Damageable>().CurrentHealth <= 0;
            return DateTime.Now - _startFocusTimeOnCurrentTarget > _maxFocusTimeOnTarget || hasInvalidTarget;
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

                if (targetToAim == null)
                {
                    continue;
                }

                Vector3 diff = transform.position - targetToAim.transform.position;
                float distance = diff.sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = targetToAim;
                }
            }

            _currentTarget = closestTarget && closestTarget.GetComponent<Damageable>().CurrentHealth > 0 ? closestTarget : null;
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
            _animator.SetTrigger("LaunchRock");
            _animator.SetBool("RightHand", UnityEngine.Random.Range(0, 2) == 1);
            transform.LookAt(_currentTarget.transform);
            StartCoroutine(ThrowItemCoroutine(_currentTarget.transform.position, _basicAttackAngle, _basicAttackGravity, _basicAttackDuration, _basicAttackLifetime));
            _basicAttackLastUse = DateTime.Now;
            Task.current.Succeed();
        }

        /**
         * EVOLVED ATTACK
         **/

        [Task]
        public bool IsEvolvedAttackAvailable()
        {
            return _evolvedAttackActivated && DateTime.Now - _evolvedAttackLastUse > _evolvedAttackCooldown;
        }

        [Task]
        public void UseEvolvedAttack()
        {
            transform.LookAt(_currentTarget.transform);
            StartCoroutine(EvolvedAttackCoroutine());
            _evolvedAttackLastUse = DateTime.Now;
            Task.current.Succeed();
        }

        private IEnumerator EvolvedAttackCoroutine()
        {
            for (int i = 0; i < _evolvedAttackRockCount; ++i)
            {
                NavMeshAgent agent = _currentTarget.GetComponent<NavMeshAgent>();
                float speed = agent.speed;
                int coefX = UnityEngine.Random.Range(-i, i + 1);
                int coefZ = UnityEngine.Random.Range(-i, i + 1);

                Vector3 targetPosition = _currentTarget.transform.position + new Vector3(coefX, 0, coefZ) * speed;

                StartCoroutine(ThrowItemCoroutine(targetPosition, _evolvedAttackAngle, _evolvedAttackGravity, _evolvedAttackDuration, _evolvedAttackLifetime));
                yield return new WaitForSeconds(_evolvedAttackTimeBetweenRock);
            }
        }

        /**
         * RAIN OF ROCKS MECHANICS
         **/

        [Task]
        public bool IsRainOfRocksAvailable()
        {
            return _rainOfRocksAvailable && !_isUsingRainOfRocks;
        }

        [Task]
        public void ActivateRainOfRocks()
        {
            if (!IsRainOfRocksAvailable())
            {
                Task.current.Fail();
                return;
            }

            _isUsingRainOfRocks = true;
            InvokeRepeating("SpawnRock", 0, _rorTimeBetweenRocks);
            Task.current.Succeed();
        }

        [Task]
        public void WaitForRainOfRocksDuration()
        {
            _bt.Wait(_rorRocksCount * _rorTimeBetweenRocks);
        }

        [Task]
        public void DeactivateRainOfRocks()
        {
            CancelInvoke("SpawnRock");
            _isUsingRainOfRocks = false;
            Task.current.Succeed();
        }

        private void SpawnRock()
        {
            Vector2 xz = UnityEngine.Random.insideUnitCircle * _rorSpawnRadius;
            Vector3 initialPosition = new Vector3(xz.x, _rorSpawnHeight, xz.y);
            GameObject item = Instantiate(_throwableObjects[UnityEngine.Random.Range(0, _throwableObjects.Length)], initialPosition, new Quaternion());
            item.AddComponent<Rigidbody>();
            item.GetComponent<Rigidbody>().AddForce(new Vector3(0, _rorSpawnImpulsion, 0), ForceMode.Impulse);
            Destroy(item, _rorRockLifetime);
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
            ScoreManager.AddScore(2, Time.timeSinceLevelLoad);
        }
    }
}
