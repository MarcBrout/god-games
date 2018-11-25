using System.Collections;
using UnityEngine;
using VikingCrewTools.UI;

namespace GodsGame
{
    public class CrowdManager : Singleton<CrowdManager>
    {
        #region Public Var
        public int nbOfOverridenSpectators = 50;
        public int nbOfSpeakingSpectators = 10;
        #endregion

        #region Private Var
        private SpectatorBehaviour[] m_spectators;
        private Transform[] m_RandomSpeaker;
        private SpectatorBehaviour[] m_RandomSpectator;
        private bool m_StateOverriden = false;
        #endregion

        protected override void OnAwakeEarly()
        {
            dontDestroyOnLoad = false;
        }

        protected override void OnAwake()
        {
            m_spectators = GetComponentsInChildren<SpectatorBehaviour>();
            m_RandomSpeaker = new Transform[nbOfSpeakingSpectators];
            m_RandomSpectator = new SpectatorBehaviour[nbOfOverridenSpectators];
        }

        public void SetState(SpectatorBehaviour.STATES state, float durationInSeconds)
        {
            if (!m_StateOverriden)
            {
                m_StateOverriden = true;
                for (int i = 0; i < m_RandomSpectator.Length; ++i)
                {
                    m_RandomSpectator[i] = m_spectators[Random.Range(0, m_spectators.Length)];
                    m_RandomSpectator[i].TransitionToNextState(state);

                }
                this.DelayAction(durationInSeconds, ResetSate);
                CrowdSayThings(state);
            }
        }

        private void ResetSate()
        {
            for (int i = 0; i < m_RandomSpectator.Length; ++i)
            {
                m_StateOverriden = false;
                m_RandomSpectator[i].TransitionToNextState(SpectatorBehaviour.STATES.IDLE);
            }
        }

        public void CrowdSayThings(SpectatorBehaviour.STATES state)
        {
            switch (state)
            {
                case SpectatorBehaviour.STATES.DISAPROVAL:
                    StartCoroutine(IECrowdSayThings(EnumAction.CROWD_DISAPROVAL));
                    break;
                case SpectatorBehaviour.STATES.PLAYER_HITTEN:
                    StartCoroutine(IECrowdSayThings(EnumAction.CROWD_PLAYER_HITTEN));
                    break;
                default:
                    Debug.Log("State not found");
                    break;
            }
        }

        //todo: add head transform to crowd
        private IEnumerator IECrowdSayThings(EnumAction action)
        {
            for (int i = 0; i < m_RandomSpeaker.Length; ++i)
            {
                m_RandomSpeaker[i] = m_spectators[Random.Range(0, m_spectators.Length)].transform;
                SpeechBubbleManager.Instance.AddSpeechBubble(m_RandomSpeaker[i], Speech.GetSpeech(action, EnumLevel.ANY));
                yield return new WaitForSeconds(.15f);
            }
        }
    }
}