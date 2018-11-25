using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    [RequireComponent(typeof(Collider))]
    public class ShockWaveObject : MonoBehaviour
    {
        public float _SpawnInterval = 0.1f;
        public float _LifeTime = 0.3f;
        private Collider _Collider;

        private void Start()
        {
            _Collider = GetComponent<Collider>();
        }

        // Use this for initialization
        void Awake()
        {
            StartCoroutine(SpawnNext());
            StartCoroutine(Die());
        }

        private void onTriggerEnter(Collider collider)
        {
            Debug.Log("COLLIISSSIOON");
            if (collider.gameObject.layer == 9)
            {
                StopCoroutine(SpawnNext());
            }
        }

        private void onTriggerStay(Collider collider)
        {
            Debug.Log("COLLIISSSIOON");
            if (collider.gameObject.layer == 9)
            {
                StopCoroutine(SpawnNext());
                _Collider.enabled = false;
            }
        }

        private IEnumerator SpawnNext()
        {
            yield return new WaitForSeconds(_SpawnInterval);
            GameObject next = Instantiate(gameObject, transform.position + new Vector3(1, 0, 0), transform.rotation);
        }

        private IEnumerator Die()
        {
            yield return new WaitForSeconds(_LifeTime);
            Destroy(gameObject);
        }
    }
}