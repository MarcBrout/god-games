using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechEventManager : MonoBehaviour
{

    public GameObject Player1, Player2, Boss;

    private Speech _speech;

    void Awake()
    {
        _speech = new Speech();
    }

	void Start ()
	{
        StartCoroutine(Speak(Player1, _speech.playerDuringFight, 2));     // Testing
	    StartCoroutine(Speak(Player2, _speech.playerDuringFight, 4));     // Testing
	    StartCoroutine(Speak(Boss, _speech.playerDuringFight, 0));     // Testing
    }

    void Update ()
    {
        /* pseudocode for the speech logic

        if (player_1 finds object)
        {
            StartCoroutine(speak(Player_1, speech.playerFoundObject, 2));
        }

        if (player_2 finds object)
        {
            StartCoroutine(speak(Player_1, speech.playerFoundObject, 2));
        }

        if (player_1 dies)
        {
            StartCoroutine(speak(Player_1, speech.playerDeath, 1));
        }

        if (player_2 dies)
        {
            StartCoroutine(speak(Player_2, speech.playerDeath, 1));
        }

        if (player_1 dodges object)
        {
            StartCoroutine(speak(Player_1, speech.playerDodgeAttack, 1));
        }

        if (player_2 dodges object)
        {
            StartCoroutine(speak(Player_2, speech.playerDodgeAttack, 1));
        }

        if (player_1 dies)
        {
            StartCoroutine(speak(Player_2, speech.playerDeath, 1));
        }

        if (player_2 dies)
        {
            StartCoroutine(speak(Player_2, speech.playerDeath, 1));
        }

        if (boss dies)
        {
            StartCoroutine(speak(Player_2, speech.bossDeath, 1));
            StartCoroutine(speak(Player_1, speech.playerWon, 3));
            StartCoroutine(speak(Player_2, speech.playerFoundObject, 3));
        }

         */
    }

    IEnumerator Speak(GameObject entity, string[] phrase, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        VikingCrewTools.UI.SpeechBubbleManager.Instance.AddSpeechBubble(entity.transform
            , phrase[UnityEngine.Random.Range(0, 6)]);
    }
}
