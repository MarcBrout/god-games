using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class Telegprah : MonoBehaviour
    {
        public Projector projectorTimer;
        public Projector projectorBase;
        public ParticleSystem particleSyst;
        public Collider collider;
        public Damager damager;
        public float activationDelayOffset = 0.1f;

        float _StartTime;

        void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            collider.enabled = false;
            damager.activationDelay = particleSyst.main.startDelay.constant + activationDelayOffset;
            _StartTime = Time.time;
            projectorTimer.orthographicSize = 0;
            StartCoroutine(Tick());
        }

        public IEnumerator Tick()
        {
            while (projectorTimer.orthographicSize < projectorBase.orthographicSize)
            {
                projectorTimer.orthographicSize = ExtensionMethods.LerpOverTime(0, projectorBase.orthographicSize, _StartTime, damager.activationDelay);
                yield return new WaitForEndOfFrame();
            }
            projectorTimer.orthographicSize = projectorBase.orthographicSize;
        }
    }
}
