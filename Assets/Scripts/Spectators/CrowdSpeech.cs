using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VikingCrewTools.UI;

namespace GodsGame
{
    //attach script to "spectators" object
    public class CrowdSpeech : MonoBehaviour
    {
        private GameObject[] randomObject;
        private Transform[] childs;

        void Start()
        {
            randomObject = new GameObject[10];
            childs = gameObject.GetComponentsInChildren<Transform>();
        }
        
        public void CrowdSayThings(CrowdManager.STATES state)
        {
            switch (state)
            {
                case CrowdManager.STATES.BOOH:
                    StartCoroutine(IECrowdSayThings(EnumAction.CROWD_BOOH));
                    break;
                case CrowdManager.STATES.OOH:
                    StartCoroutine(IECrowdSayThings(EnumAction.CROWD_OOH));
                    break;
                default:
                    Debug.Log("State not found");
                    break;
            }
        }

        //todo: add head transform to crowd
        private IEnumerator IECrowdSayThings(EnumAction action)
        {
            for (int i = 0; i < 10; ++i)
            {
                randomObject[i] = childs[Random.Range(0, childs.Length)].gameObject;
                SpeechBubbleManager.Instance.AddSpeechBubble(randomObject[i].transform, Speech.GetSpeech(action, EnumLevel.ANY));
                yield return new WaitForSeconds(.15f);
            }
        }
    }
}
