using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToScreenPosition : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
        }
    }
}
