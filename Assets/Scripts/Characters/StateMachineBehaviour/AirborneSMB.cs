using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneLinkedSMB;

namespace GodsGame
{
    public class AirborneSMB : SceneLinkedSMB<PlayerBehaviour>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.GetInput();
            m_MonoBehaviour.Move();
            m_MonoBehaviour.CheckForGrounded();
        }
    }
}
