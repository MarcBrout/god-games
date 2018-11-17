using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame {
    public class CrowdManager : MonoBehaviour
    {
        public static CrowdManager instance;
        private CrowdSpeech crowdSpeech;

        public State CurrentState
        {
            get { return this.States[currentState]; }
        }

        public Dictionary<STATES, State> States = new Dictionary<STATES, State>()
        {
            {STATES.IDLE, new State(90, 5, 5, 0) },
            {STATES.CHEER, new State(10, 70, 10, 10) },
            {STATES.BOOH, new State(10, 20, 60, 10) },
            {STATES.OOH, new State(5, 10, 5, 80) },
        };

        [HideInInspector]
        public enum STATES
        {
            IDLE,
            CHEER,
            BOOH,
            OOH
        }

        public struct State
        {
            public int IdlePercent;
            public int CheerPercent;
            public int BoohPercent;
            public int OohPercent;

            public State(int IdlePercent, int CheerPercent, int BoohPercent, int OohPercent)
            {
                this.IdlePercent = IdlePercent;
                this.CheerPercent = CheerPercent;
                this.BoohPercent = BoohPercent;
                this.OohPercent = OohPercent;
            }
        }

        private STATES currentState = STATES.IDLE;
        private bool stateChanged = false;

        private IEnumerator ChangeState(STATES State, long DurationInMilliseconds)
        {
            yield return new WaitForSecondsRealtime(DurationInMilliseconds / 1000f);

            this.stateChanged = false;
            this.currentState = STATES.IDLE;
        }

        public void SetState(STATES State, long DurationInMilliseconds)
        {
            if (!stateChanged)
            {
                StartCoroutine(ChangeState(State, DurationInMilliseconds));
                crowdSpeech.CrowdSayThings(State);
                this.currentState = State;
                this.stateChanged = true;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            crowdSpeech = GameObject.Find("Spectators").GetComponent<CrowdSpeech>();
        }
    }
}