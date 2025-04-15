using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class HouseEntrance : MonoBehaviour
{
    public Transform interiorSpawnPoint;
    public CinemachineVirtualCamera externalCamera;
    public CinemachineVirtualCamera interiorCamera;
    public GameObject enterButton;
    public GameObject houseEntrance;

    
    public void TeleportToInterior()
    {
        transform.position = interiorSpawnPoint.position;
        interiorCamera.gameObject.SetActive(true);
        externalCamera.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == houseEntrance)
        {
            enterButton.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == houseEntrance)
        {
            enterButton.SetActive(false);
        }
    }
}
