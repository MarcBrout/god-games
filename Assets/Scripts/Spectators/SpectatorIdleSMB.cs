using UnityEngine;
using SceneLinkedSMB;

namespace GodsGame
{
    public class SpectatorIdleSMB : SceneLinkedSMB<SpectatorBehaviour>
    {
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.PreviousState = SpectatorBehaviour.STATES.IDLE;
        }
    }
}
