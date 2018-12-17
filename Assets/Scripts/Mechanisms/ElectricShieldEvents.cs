using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class ElectricShieldEvents : MonoBehaviour
    {
        public AnimationCurve _DisableCurve;
        public AnimationCurve _EnableCurve;

        Collider _collider;
        private AudioSource _audioSource;
        private ParticleSystem[] _particleSystems;
        private Light _light;
        private float _intensity;
        private Vector3 _shieldScale;

        // Use this for initialization
        void Start()
        {
            _collider = GetComponent<Collider>();
            _audioSource = GetComponent<AudioSource>();
            _particleSystems = GetComponentsInChildren<ParticleSystem>();
            _light = GetComponentInChildren<Light>();
            _shieldScale = transform.localScale;
            _intensity = _light.intensity;
        }

        public void Disable(Activable activable, Trigger trigger)
        {
            StartCoroutine(DisableOverTime());
            AudioManager.Instance.PlaySfx3D("shield_down", "items_common", ref _audioSource);
        }

        IEnumerator DisableOverTime()
        {
            float _startTime = 0f;
            float _endTime = _DisableCurve.keys[_DisableCurve.length - 1].time;
            Debug.Log("Length = " + _endTime);
            while (_startTime < _endTime)
            {
                float size = _DisableCurve.Evaluate(_startTime);
                transform.localScale = _shieldScale * size;
                _light.intensity = _intensity * size;
                _startTime += Time.smoothDeltaTime;
                yield return null;
            }
            foreach (ParticleSystem particle in _particleSystems)
            {
                particle.Stop();
            }
        }

        public void Enable(Activable activable, Trigger trigger)
        {
            AudioManager.Instance.PlaySfx3D("shield_up", "items_common", ref _audioSource);

            StartCoroutine(EnableOverTime());
            StartCoroutine(enableAfter());
        }

        public IEnumerator enableAfter()
        {
            yield return new WaitForSeconds(1000);
        }

        IEnumerator EnableOverTime()
        {
            float _startTime = 0f;
            float _endTime = _EnableCurve.keys[_EnableCurve.length - 1].time;

            while (_startTime < _endTime)
            {
                float size = _EnableCurve.Evaluate(_startTime);
                transform.localScale = _shieldScale * size;
                _light.intensity = _intensity * size;
                _startTime += Time.smoothDeltaTime;
                yield return null;
            }
            foreach (ParticleSystem particle in _particleSystems)
            {
                particle.Play();
            }
        }

    }
}