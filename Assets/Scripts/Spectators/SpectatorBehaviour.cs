using SceneLinkedSMB;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    [RequireComponent(typeof(Animator))]
    public class SpectatorBehaviour : MonoBehaviour
    {
        #region Public Var
        public float rotationSpeed = 30;
        [HideInInspector]
        public enum STATES
        {
            NULL,
            IDLE,
            CHEER,
            DISAPROVAL,
            PLAYER_HITTEN
        }
        #endregion

        #region Private Var
        private Animator m_Animator;
        private Transform m_Target;
        private readonly int m_HashAnimIndexPara = Animator.StringToHash("AnimIndex");
        private readonly int m_HashIdlePara = Animator.StringToHash("Idle");
        private readonly int m_HashCheerPara = Animator.StringToHash("Cheer");
        private readonly int m_HashDisaprovePara = Animator.StringToHash("Disaprove");
        private readonly int m_HashPlayerHitPara = Animator.StringToHash("PlayerHitten");

        private Dictionary<STATES, StatePercentage> m_statesTransitionPrecentage = new Dictionary<STATES, StatePercentage>()
        {
            {STATES.IDLE, new StatePercentage(80, 10, 10, 0) },
            {STATES.CHEER, new StatePercentage(10, 70, 10, 10) },
            {STATES.DISAPROVAL, new StatePercentage(10, 20, 60, 10) },
            {STATES.PLAYER_HITTEN, new StatePercentage(5, 10, 5, 80) },
        };

        public struct StatePercentage
        {
            public int IdlePercent;
            public int CheerPercent;
            public int Disaproval;
            public int PlayerHitten;

            public StatePercentage(int IdlePercent, int CheerPercent, int Disaproval, int PlayerHitten)
            {
                this.IdlePercent = IdlePercent;
                this.CheerPercent = CheerPercent;
                this.Disaproval = Disaproval;
                this.PlayerHitten = PlayerHitten;
            }
        }
        #endregion

        #region Properties
        public STATES PreviousState { get; set; }
        #endregion

        void Start()
        {
            m_Animator = GetComponent<Animator>();
            SceneLinkedSMB<SpectatorBehaviour>.Initialise(m_Animator, this);
            PreviousState = STATES.IDLE;
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
            if (objects.Length > 0)
                m_Target = objects[Random.Range(0, objects.Length)].transform;
        }

        public STATES ChooseNextState()
        {
            StatePercentage state = m_statesTransitionPrecentage[PreviousState];
            // TODO : rework this later to be more scalable
            int idle = state.IdlePercent;
            int cheer = idle + state.CheerPercent;
            int disaproval = cheer + state.Disaproval;
            int playerHitten = disaproval + state.PlayerHitten;
            int me = Random.Range(0, 100);

            if (me >= idle && me < cheer)
                return STATES.CHEER;
            else if (me >= cheer && me < disaproval)
                return STATES.DISAPROVAL;
            if (me >= disaproval && me < playerHitten)
                return STATES.PLAYER_HITTEN;
            return STATES.IDLE;
        }

        public void TransitionToNextState(STATES state, int animIndex = 0)
        {
            switch (state)
            {
                case (STATES.CHEER):
                    GoToCheer(animIndex);
                    break;
                case (STATES.DISAPROVAL):
                    GotToDisaproval(animIndex);
                    break;
                case (STATES.PLAYER_HITTEN):
                    GoToPlayerHitten(animIndex);
                    break;
                default:
                    GotToIdle(animIndex);
                    break;
            }
        }

        public void GotToIdle(int animIndex = 0)
        {
            m_Animator.SetTrigger(m_HashIdlePara);
            m_Animator.SetInteger(m_HashAnimIndexPara, animIndex);
        }

        public void GoToCheer(int animIndex = 0)
        {
            m_Animator.SetTrigger(m_HashCheerPara);
            m_Animator.SetInteger(m_HashAnimIndexPara, animIndex);
        }

        public void GotToDisaproval(int animIndex = 0)
        {
            m_Animator.SetTrigger(m_HashDisaprovePara);
            m_Animator.SetInteger(m_HashAnimIndexPara, animIndex);
        }

        public void GoToPlayerHitten(int animIndex = 0)
        {
            m_Animator.SetTrigger(m_HashPlayerHitPara);
            m_Animator.SetInteger(m_HashAnimIndexPara, animIndex);
        }

        public void Update()
        {
            if (m_Target)
            {
                Vector3 targetTransform = m_Target.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(targetTransform, Vector3.up);
                lookRotation.x = lookRotation.z = 0;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
