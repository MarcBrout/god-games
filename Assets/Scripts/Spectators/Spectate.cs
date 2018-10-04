﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class Spectate : MonoBehaviour
    {
        public int FrameUpdateInterval = 60;
        public int FrameStartingRange = 6;
        public Animator animator;
        public Transform target;
        public float RotationSpeed;
        

        static private Dictionary<CrowdManager.STATES, string[]> MapStateToAnimations = new Dictionary<CrowdManager.STATES, string[]>()
        {
            { CrowdManager.STATES.IDLE, new string[] { "IDLE_01" } },
            { CrowdManager.STATES.CHEER, new string[] { "CHEER_01", "CHEER_02", "CHEER_03" } },
            { CrowdManager.STATES.BOOH, new string[] { "BOOH_01", "BOOH_02" } },
            { CrowdManager.STATES.OOH, new string[] { "OOH_01" } }
        };

        private int StartingFrame;

        // Use this for initialization
        void Start()
        {
            StartingFrame = Random.Range(0, FrameStartingRange);
            animator.SetFloat("StartingIdleOffset", Random.Range(2f, 100f));
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
            target = objects[Random.Range(0, objects.Length)].transform;
        }

        CrowdManager.STATES GetMyState(CrowdManager.State state)
        {
            // TODO : rework this later to be more scalable
            int idle = state.IdlePercent;
            int cheer = idle + state.CheerPercent;
            int booh = cheer + state.BoohPercent;
            int ooh = booh + state.OohPercent;

            int me = Random.Range(0, 100);

            if (me >= idle && me < cheer)
                return CrowdManager.STATES.CHEER;

            if (me >= cheer && me < booh)
                return CrowdManager.STATES.BOOH;

            if (me >= booh && me < ooh)
                return CrowdManager.STATES.OOH;

            return CrowdManager.STATES.IDLE;
        }

        string GetAnimationOfState(CrowdManager.STATES state)
        {
            int len = Spectate.MapStateToAnimations[state].Length;

            return Spectate.MapStateToAnimations[state][Random.Range(0, len)];
        }

        void Update()
        {
            if (Time.frameCount % (FrameUpdateInterval + StartingFrame) == 0)
            {
                CrowdManager.STATES MyState = GetMyState(CrowdManager.instance.CurrentState);
                string animation = GetAnimationOfState(MyState);
                animator.SetTrigger(animation);
            }

            Vector3 targetTransform = target.position - transform.position;
            targetTransform.y = transform.position.y;
            Quaternion lookRotation = Quaternion.LookRotation(targetTransform, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, RotationSpeed * Time.deltaTime); ;
        }
    }
}