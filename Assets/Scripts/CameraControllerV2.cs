using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControllerV2 : MonoBehaviour
{

    public new CinemachineVirtualCamera camera;
    public List<Transform> targets;
    public Vector3 offset;
    public float smoothTime = 0.3f;

    public float zoomLimiter = 25f;
    public float minZoom = 40f;
    public float maxZoom = 10f;

    private Vector3 velocity;
    private Bounds bounds;

    private void LateUpdate()
    {
        if (targets.Count <= 0)
            throw new Exception("There is no targets");
        Move();
        Zoom();
    }

    private void Move()
    {
        bounds = GetBounds();
        Vector3 newPosition = GetCenterPoint() + offset;
        camera.transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, newZoom, Time.deltaTime);
    }


    float GetGreatestDistance()
    {
        return bounds.size.x > bounds.size.z ? bounds.size.x : bounds.size.z;
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
            return targets[0].position;
        return bounds.center;
    }

    Bounds GetBounds()
    {
        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; ++i)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds;
    }
}
