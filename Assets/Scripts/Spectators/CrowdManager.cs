using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame {
    public class CrowdManager : MonoBehaviour {
        public State CurrentState
        {
            get { return this.States[currentState]; }
        }

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
            public float IdlePercent;
            public float CheerPercent;
            public float BoohPercent;
            public float OohPercent;

            public State(float IdlePercent, float CheerPercent, float BoohPercent, float OohPercent)
            {
                this.IdlePercent = IdlePercent;
                this.CheerPercent = CheerPercent;
                this.BoohPercent = BoohPercent;
                this.OohPercent = OohPercent;
            }
        }

        private STATES currentState = STATES.IDLE;
        private bool stateChanged = false;
        public Dictionary<STATES, State> States = new Dictionary<STATES, State>()
        {
            {STATES.IDLE, new State(80f, 15f, 0f, 0f) },
            {STATES.CHEER, new State(10f, 70f, 10f, 10f) },
            {STATES.BOOH, new State(10f, 20f, 60f, 10f) },
            {STATES.OOH, new State(5f, 20f, 5f, 70f) },
        };

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

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
                this.currentState = State;
                this.stateChanged = true;
            }
        }
    }
}