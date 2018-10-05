using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class DustEffectPool : ObjectPool<DustEffectPool, DustEffect, Vector3>
    {
        private bool _invokeStarted = false; 

        public void StartStepDust(float repeatDelay)
        {
            if (!_invokeStarted) {
                _invokeStarted = true;
                InvokeRepeating("DoStepDust", 0, repeatDelay);
            }
        }

        private void DoStepDust() {
            Pop(transform.position);
        }

        public void StopStepDust()
        {
            CancelInvoke("DoStepDust");
            _invokeStarted = false;
        }
    }

    public class DustEffect : PoolObject<DustEffectPool, DustEffect, Vector3>
    {
        private CFX_AutoDestructShuriken m_AutoDestructShuriken;
        private Transform originalParent;

        protected override void SetReferences()
        {
            originalParent = instance.transform.parent;
            m_AutoDestructShuriken = instance.GetComponent<CFX_AutoDestructShuriken>();
            m_AutoDestructShuriken.OnDestroy += ReturnToPool;
        }

        public override void Sleep()
        {
            instance.SetActive(false);
            instance.transform.SetParent(originalParent);
        }

        public override void WakeUp(Vector3 position)
        {
            instance.SetActive(true);
            instance.transform.position = position;
            instance.transform.SetParent(null);
        }
    }
}
