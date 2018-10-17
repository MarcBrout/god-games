using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCameraScript : MonoBehaviour {

    public float transitionSpeed = 1;

    Transform _targetViewTransform;

    public void StartTransition(GameObject previouseCamera, Transform bossTransform) {
        transform.position = previouseCamera.transform.position;
        transform.eulerAngles = previouseCamera.transform.eulerAngles;

        _targetViewTransform = bossTransform;

        previouseCamera.SetActive(false);
        gameObject.SetActive(true);

    }

    public void Update()
    {
        //Lerp position
        transform.position = Vector3.Lerp(transform.position, _targetViewTransform.position, Time.deltaTime * transitionSpeed);

        Vector3 currentAngle = new Vector3(
         Mathf.LerpAngle(transform.rotation.eulerAngles.x, _targetViewTransform.transform.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
         Mathf.LerpAngle(transform.rotation.eulerAngles.y, _targetViewTransform.transform.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
         Mathf.LerpAngle(transform.rotation.eulerAngles.z, _targetViewTransform.transform.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

        transform.eulerAngles = currentAngle;
    }
}
