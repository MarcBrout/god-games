using Panda;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodsGame;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace GodsGames
{
    public class CyclopTasks : MonoBehaviour
    {
        [Header("General configuration")]
        public GameObject[] _throwableObjects;
        public string _targetTag;
        public List<GameObject> _targets;
        private GameObject _currentTarget;
        public Animator _animator;
        public List<GameObject> _ropes;
        public int _lastAmountOfRopes;

        [Header("Phases")]
        public int _startTransition;
        public int _endTransition;
        public int _currentTransition;
        public float _transitionDuration;
        public string[] _transitionsMethod;
        public float _transitionObjectsLifetime;
        public int _transitionObjectPoolSize;
        public float _transitionTimeBetweenObjects;
        private List<GameObject> _transitionObjectPool;

        [Header("Global attack")]
        public float _attackLevel = 1f;
        public float _attackEvolveCoeff = 1.2f;

        [Header("Basic attack")]
        public float _basicAttackGravity;
        public float _basicAttackAngle;
        public float _basicAttackDuration;
        public float _basicAttackLifetime;
        private TimeSpan _basicAttackCooldown = new TimeSpan(0, 0, 1);
        private DateTime _basicAttackLastUse;

        [Header("Rain of rocks")]
        public float _rorTimeBetweenRocks;
        public float _rorSpawnRadius;
        public float _rorSpawnHeight;
        public float _rorMass;
        public float _rorSpawnImpulsion;
        public float _rorRockLifetime;

        [Header("Evolved attack")]
        public float _evolvedAttackMinimumLevelToActivate;
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
        public bool _isTransitionAvailable = false;

        private TimeSpan _maxFocusTimeOnTarget = new TimeSpan(0, 0, 6);
        private DateTime _startFocusTimeOnCurrentTarget;
        private PandaBehaviour _bt;
        private AudioSource _audioSource;
        private Collider[] _towerColliders;
        private Rigidbody[] _towerBodies;

        void Start()
        {
            _bt = gameObject.GetComponent<PandaBehaviour>();
            _audioSource = GetComponent<AudioSource>();
            _lastAmountOfRopes = GetActiveRopesCount();
            _towerColliders = transform.root.GetComponentsInChildren<Collider>();
            _towerBodies = transform.root.GetComponentsInChildren<Rigidbody>();
            FillTransitionObjectPool();
            if (_targets.Count == 0)
                _targets = new List<GameObject>(GameObject.FindGameObjectsWithTag(_targetTag));
            foreach (Collider collider in _towerColliders)
            {
                collider.enabled = false;
            }
        }

        private void Update()
        {
            if (_currentTarget)
                transform.LookAt(_currentTarget.transform);
        
        }

        /**
         * TOOLS
         **/

        private void FillTransitionObjectPool()
        {
            _transitionObjectPool = new List<GameObject>();
            for (int i = 0; i < _transitionObjectPoolSize; ++i)
            {
                GameObject item = Instantiate(_throwableObjects[UnityEngine.Random.Range(0, _throwableObjects.Length)]);
                item.SetActive(false);
                item.AddComponent<Rigidbody>();
                _transitionObjectPool.Add(item);
            }

        }

        void ResetStates()
        {
            _isDead = false;
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

            Destroy(thrownItem, lifetime);
        }


        /**
         * ROPES UTILS
         **/

        private int GetActiveRopesCount()
        {
            int ropesAmount = 0;
            foreach (var rope in _ropes)
            {
                if (rope.activeSelf)
                    ropesAmount++;
            }

            return ropesAmount;
        }

        [Task]
        public bool DoesARopeHasBeenCutted()
        {
            int currentRopesAmount = GetActiveRopesCount();
            bool cutted = currentRopesAmount < _lastAmountOfRopes;
            _lastAmountOfRopes = currentRopesAmount;
            return cutted;
        }

        [Task]
        public void ImproveBasicAttackLevel()
        {
            _attackLevel *= _attackEvolveCoeff;
            if (_attackLevel >= _evolvedAttackMinimumLevelToActivate)
                _evolvedAttackActivated = true;
            Task.current.Succeed();
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
         * TRANSITIONS
         **/
        [Task]
        public bool IsTransitionAvailable()
        {
            return _isTransitionAvailable;
        }

        [Task]
        public void GoToNextTransition()
        {
            if (_currentTransition < _endTransition && _currentTransition < _transitionsMethod.Length)
                _currentTransition++;
            Task.current.Succeed();
        }

        [Task]
        public void UseTransition()
        {
            StartCoroutine(_transitionsMethod[_currentTransition]);
            _isTransitionAvailable = false;

            Task.current.Succeed();
        }

        public IEnumerator TransitionRainOfRocks()
        {
            foreach (var item in _transitionObjectPool)
            {
                StartCoroutine("SpawnRainOfRockBoulder", item);
                yield return new WaitForSeconds(_rorTimeBetweenRocks);
            }
            _bt.Wait(_transitionObjectPoolSize * _rorTimeBetweenRocks);
            yield return null;
        }
        private IEnumerator SpawnRainOfRockBoulder(GameObject item)
        {
            Vector2 xz = UnityEngine.Random.insideUnitCircle * _rorSpawnRadius;
            item.transform.position = new Vector3(xz.x, _rorSpawnHeight, xz.y);
            item.transform.rotation = new Quaternion();
            item.SetActive(true);
            item.GetComponent<Rigidbody>().AddForce(new Vector3(0, _rorSpawnImpulsion * _attackLevel, 0), ForceMode.Impulse);
            yield return new WaitForSeconds(_rorRockLifetime);
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.SetActive(false);
            yield return null;
        }

        public IEnumerator TransitionPartialFloodTopRight()
        {
            StartCoroutine("TransitionPartialArenaFlood", new Vector2(1, 1));
            yield return null;
        }

        public IEnumerator TransitionPartialFloodTopLeft()
        {
            StartCoroutine("TransitionPartialArenaFlood", new Vector2(-1, 1));
            yield return null;
        }

        public IEnumerator TransitionFloodTop()
        {
            StartCoroutine("TransitionArenaFlood", 1);
            yield return null;
        }

        public IEnumerator TransitionFloodBottom()
        {
            StartCoroutine("TransitionArenaFlood", -1);
            yield return null;
        }

        public IEnumerator TransitionPartialArenaFlood(Vector2 initialZone)
        {
            foreach (var item in _transitionObjectPool)
            {
                SpawnTransitionRock(item, initialZone);
                yield return new WaitForSeconds(_transitionTimeBetweenObjects);
            }
            yield return new WaitForSeconds(_transitionObjectsLifetime);
            StartCoroutine("EmptyTransitionObjectPool");
            yield return null;
        }

        public IEnumerator TransitionArenaFlood(int directionY)
        {
            var directionsX = new[] { -1, 1 };

            foreach (var item in _transitionObjectPool)
            {
                int directionX = directionsX[UnityEngine.Random.Range(0, directionsX.Length)];
                SpawnTransitionRock(item, new Vector2(directionX, directionY));
                yield return new WaitForSeconds(_transitionTimeBetweenObjects / 2);
            }
            yield return new WaitForSeconds(_transitionObjectsLifetime);
            StartCoroutine("EmptyTransitionObjectPool");
            yield return null;
        }


        public void SpawnTransitionRock(GameObject item, Vector2 initialZone)
        {
            Vector2 initialPos = new Vector2(initialZone.x * _rorSpawnRadius / 2, initialZone.y * _rorSpawnRadius / 2);
            Vector2 xz = initialPos + UnityEngine.Random.insideUnitCircle * _rorSpawnRadius / 2;
            item.transform.position = new Vector3(xz.x, _rorSpawnHeight, xz.y);
            item.SetActive(true);
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().AddForce(new Vector3(0, _rorSpawnImpulsion * _attackLevel, 0), ForceMode.Impulse);
        }

        public IEnumerator EmptyTransitionObjectPool()
        {
            foreach (var item in _transitionObjectPool)
            {
                item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                item.SetActive(false);
                yield return new WaitForSeconds(_transitionTimeBetweenObjects / 10);
            }
            yield return null;
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

        public void OnDamageBoss()
        {
            _isTransitionAvailable = true;
        }

        public void OnDieBoss()
        {
            _isDead = true;
                _animator.SetTrigger("isDead");
                _currentTarget = null;
                foreach (Collider collider in _towerColliders)
                {
                    collider.enabled = true;
                }
                foreach (Rigidbody rigidbody in _towerBodies)
                {
                    rigidbody.constraints = RigidbodyConstraints.None;
                }
            _audioSource.Play();
            StartCoroutine(LoadLevelCompleteScene());
        }

        IEnumerator LoadLevelCompleteScene()
        {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("LevelComplete");
        }
    }
}
