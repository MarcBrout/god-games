using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchAnimationOnTrigger : MonoBehaviour {

    public int numberTrigger = 0;
    public int sceneIndex = 0;

    private int _triggerCount = 0;

    public void AddTriggerCount()
    {
        _triggerCount++;
        Debug.Log(_triggerCount);
        if (_triggerCount >= numberTrigger)
        {
            _triggerCount = 0;
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        }
    }

    public void SubstractTriggerCount() 
    {
        _triggerCount--;
        Debug.Log(_triggerCount);
    }
}
