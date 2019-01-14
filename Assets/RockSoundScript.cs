using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    [RequireComponent(typeof(AudioSource))]
    public class RockSoundScript : MonoBehaviour
    {
        public LayerMask hittableLayers;
        public float lifetime;
        private AudioSource _audioSource;
        private Vector3 _lastPosition;

        private Damager _damager;
        private bool _hit = false;
        public GameObject _Explosion;
        private Collider[] _colliders;
        private MeshRenderer[] _meshes;
        // Use this for initialization
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _damager = GetComponent<Damager>();
            _colliders = GetComponents<Collider>();
            _meshes = GetComponentsInChildren<MeshRenderer>();
        }

        private void Update()
        {
            float _speed = (transform.position - _lastPosition).magnitude / Time.deltaTime;
            _lastPosition = transform.position;
            if (_speed <= 0.1 && !_hit)
            {
                _hit = true;
                AudioManager.Instance.PlayRandomSfx3D("minotaur_step", ref _audioSource);
                if (_damager)
                {
                    _damager.DisableDamage();
                    _damager.enabled = false;
                }
                if (_Explosion)
                {
                    _Explosion.SetActive(true);
                }

                foreach (MeshRenderer m in _meshes)
                {
                    m.enabled = false;
                }

                foreach (Collider c in _colliders)
                {
                    c.enabled = false;
                }

                Invoke("disable", lifetime);
            }
        }

        void disable()
        {
            Destroy(gameObject);
        }
    }
}