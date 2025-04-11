using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LookAtCameraUI : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private void LateUpdate() {
        transform.forward = virtualCamera.transform.forward;
    }
}
