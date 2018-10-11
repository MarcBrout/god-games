using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VikingCrewTools.UI;

namespace GodsGame
{
    //attack script to "spectators" object
    public class CrowdSpeech : MonoBehaviour
    {
        private GameObject[] randomObject;
        private Transform[] childs;

        void Start()
        {
            randomObject = new GameObject[10];
            childs = (Transform[]) gameObject.GetComponentsInChildren<Transform>();
        }

        public void CrowdSayThings(CrowdManager.STATES state)
        {
            switch (state)
            {
                case CrowdManager.STATES.BOOH:
                    CrowdSayThings(EnumAction.CROWD_BOOH);
                    break;
                case CrowdManager.STATES.CHEER:
                    break;
                case CrowdManager.STATES.OOH:
                    CrowdSayThings(EnumAction.CROWD_OOH);
                    break;
            }
        }
        //todo: add head transform to crowd
        private void CrowdSayThings(EnumAction action)
        {
            for (int i = 0; i < 10; ++i)
            {
                randomObject[i] = (GameObject)((Transform)childs[Random.Range(0, childs.Length)]).gameObject;
                SpeechBubbleManager.Instance.AddSpeechBubble(randomObject[i].transform, Speech.GetSpeech(action, EnumLevel.ANY));
            }
        }
    }
}
