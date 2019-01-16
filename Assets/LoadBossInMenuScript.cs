using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class LoadBossInMenuScript : MonoBehaviour
    {

        public GameObject[] bosses;

        // Use this for initialization
        void Start()
        {
            if (GameManager.Instance.previousLevel != 0)
            {
                Debug.Log(GameManager.Instance.previousLevel);
                bosses[GameManager.Instance.previousLevel - 1].SetActive(true);
            }
            else
            {
                bosses[0].SetActive(true);
            }
        }


    }
}
