using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class HouseEntrance : MonoBehaviour
{
    public Camera externalCamera;
    public Camera interiorCamera;
    public GameObject enterButton;
    public GameObject interiorSpawnPoint;
    public GameObject houseEntrance;
    public GameObject exitButton;
    public Canvas worldSpaceCanvas;
    
    public void TeleportToInterior()
    {
        transform.position = interiorSpawnPoint.transform.position;
        interiorCamera.gameObject.SetActive(true);
        externalCamera.gameObject.SetActive(false);
        
        worldSpaceCanvas.worldCamera = interiorCamera;
    }

    public void TeleportToExterior()
    {
        transform.position = houseEntrance.transform.position;
        interiorCamera.gameObject.SetActive(false);
        externalCamera.gameObject.SetActive(true);
        
        worldSpaceCanvas.worldCamera = externalCamera;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == houseEntrance)
        {
            enterButton.SetActive(true);
        }
        else if (other.gameObject == interiorSpawnPoint)
        {
            exitButton.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == houseEntrance)
        {
            enterButton.SetActive(false);
        }
        else if (other.gameObject == interiorSpawnPoint)
        {
            exitButton.SetActive(false);
        }
    }
}
