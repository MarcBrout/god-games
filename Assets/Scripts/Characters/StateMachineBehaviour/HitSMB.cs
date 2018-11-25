using UnityEngine;
using SceneLinkedSMB;

namespace GodsGame
{
    public class HitSMB : SceneLinkedSMB<PlayerBehaviour>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.GroundedHorizontalMovement(false);
            m_MonoBehaviour.Damageable.EnableInvulnerability(true);
            GameManager.Instance.StatsManager.AddHitCountFor(animator.name);
            Debug.Log("INVU");
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Damageable.DisableInvulnerability();
            Debug.Log("NOT INVU");
        }
    }
}
