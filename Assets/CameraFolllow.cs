using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraFolllow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 10f, -6f);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target);
    }
}
