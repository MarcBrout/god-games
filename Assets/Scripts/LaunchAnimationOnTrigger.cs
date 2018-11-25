using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchAnimationOnTrigger : MonoBehaviour {

    public int NumberTrigger = 0;
    private int _triggerCount = 0;

	public void AddTriggerCount()
    {
        _triggerCount++;
        Debug.Log(_triggerCount);
        if (_triggerCount >= NumberTrigger)
        {
            _triggerCount = 0;
            SceneManager.LoadScene(4, LoadSceneMode.Single);
        }
    }

    public void SubstractTriggerCount() 
    {
        _triggerCount--;
        Debug.Log(_triggerCount);
    }
}
