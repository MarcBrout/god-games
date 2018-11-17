using System.Collections.Generic;
using UnityEngine;
using SceneLinkedSMB;
using UnityEditor.Animations;

namespace GodsGame
{
    public class SpectatorManagerSMB : SceneLinkedSMB<SpectatorBehaviour>
    {
        private Dictionary<SpectatorBehaviour.STATES, int> m_states = new Dictionary<SpectatorBehaviour.STATES, int>();
        private float m_OffsetStart;
        
        public override void OnStart(Animator animator)
        {
            AnimatorController ac = animator.runtimeAnimatorController as AnimatorController;
            AnimatorStateMachine sm = ac.layers[0].stateMachine;
            ChildAnimatorStateMachine[] subStateMachines = sm.stateMachines;
            for (int i = 0; i < subStateMachines.Length; ++i)
            {
                AnimatorStateMachine subStateMachine = subStateMachines[i].stateMachine;
                m_states.Add(subStateMachine.name.ToEnum(SpectatorBehaviour.STATES.NULL), subStateMachine.states.Length);
            }
            m_OffsetStart = Random.Range(0f, 100f);
        }

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
                GoToNextState();
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (Time.time % m_OffsetStart == 0)
            {
                GoToNextState();
            }
        }

        private void GoToNextState()
        {
            SpectatorBehaviour.STATES nextState = m_MonoBehaviour.ChooseNextState();
            int animIndex = Random.Range(0, m_states[nextState]);
            m_MonoBehaviour.TransitionToNextState(nextState, animIndex);
        }
    }
}
