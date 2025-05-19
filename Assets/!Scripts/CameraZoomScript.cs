using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoomScript : MonoBehaviour
{
    public CinemachineVirtualCamera defaultCam;
    public CinemachineVirtualCamera zoomCam;
    public CinemachineTargetGroup targetGroup;

    public Transform player;
    public Transform npc;

    public void FocusOnInteraction(Transform npcTransform)
    {
        targetGroup.m_Targets[0].target = player;
        targetGroup.m_Targets[1].target = npcTransform;

        defaultCam.gameObject.SetActive(false);
        zoomCam.gameObject.SetActive(true);
    }

    public void ResetCamera()
    {
        defaultCam.gameObject.SetActive(true);
        zoomCam.gameObject.SetActive(false);
    }
}
