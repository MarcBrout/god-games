using UnityEngine;
using SceneLinkedSMB;

namespace GodsGame
{
    public class SpectatorPlayerHittenSMB : SceneLinkedSMB<SpectatorBehaviour>
    { 
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.PreviousState = SpectatorBehaviour.STATES.PLAYER_HITTEN;
        }
    }
}
