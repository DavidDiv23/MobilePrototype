using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HouseEntrance : MonoBehaviour
{
    private NavMeshAgent agent;
    public Camera externalCamera;
    public Camera interiorCamera;
    public Camera storageCamera;
    public Camera patientCamera;
    public GameObject enterButton;
    public GameObject interiorSpawnPoint;
    public GameObject houseEntrance;
    public GameObject exitButton;
    public GameObject patientButton;
    public GameObject storageButton;
    public GameObject storageSpawnPoint;
    public GameObject patientSpawnPoint;
    public Canvas exteriorWorldSpaceCanvas;
    public Canvas interiorWorldSpaceCanvas;
    public SimpleMovement simpleMovement;

    public GameObject canvasForInterior;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        canvasForInterior.gameObject.SetActive(false);
    }

    public void TeleportToInterior()
    {
        agent.Warp(interiorSpawnPoint.transform.position);
        interiorCamera.gameObject.SetActive(true);
        externalCamera.gameObject.SetActive(false);
        storageCamera.gameObject.SetActive(false);
        patientCamera.gameObject.SetActive(false);
        
        exteriorWorldSpaceCanvas.worldCamera = interiorCamera;
        interiorWorldSpaceCanvas.worldCamera = interiorCamera;
        simpleMovement.SetCamera(interiorCamera);
        HideInteriorUI();
    }

    public void TeleportToExterior()
    {
        agent.Warp(houseEntrance.transform.position);
        interiorCamera.gameObject.SetActive(false);
        externalCamera.gameObject.SetActive(true);
        storageCamera.gameObject.SetActive(false);
        patientCamera.gameObject.SetActive(false);
        
        interiorWorldSpaceCanvas.worldCamera = externalCamera;
        exteriorWorldSpaceCanvas.worldCamera = externalCamera;
        simpleMovement.SetCamera(externalCamera);
        HideInteriorUI();
    }
    public void TeleportToStorage()
    {
        agent.Warp(storageSpawnPoint.transform.position);
        storageCamera.gameObject.SetActive(true);
        externalCamera.gameObject.SetActive(false);
        interiorCamera.gameObject.SetActive(false);
        patientCamera.gameObject.SetActive(false);
        
        exteriorWorldSpaceCanvas.worldCamera = storageCamera;
        interiorWorldSpaceCanvas.worldCamera = storageCamera;
        simpleMovement.SetCamera(storageCamera);
        HideInteriorUI();
    }
    public void TeleportToPatient()
    {
        agent.Warp(patientSpawnPoint.transform.position);
        patientCamera.gameObject.SetActive(true);
        externalCamera.gameObject.SetActive(false);
        interiorCamera.gameObject.SetActive(false);
        storageCamera.gameObject.SetActive(false);
        
        exteriorWorldSpaceCanvas.worldCamera = patientCamera;
        interiorWorldSpaceCanvas.worldCamera = patientCamera;
        simpleMovement.SetCamera(patientCamera);
        HideInteriorUI();
    }

    public void ShowInteriorUI()
    {
        canvasForInterior.gameObject.SetActive(true);
    }
    public void HideInteriorUI()
    {
        canvasForInterior.gameObject.SetActive(false);
        enterButton.SetActive(false);
        exitButton.SetActive(false);
        patientButton.SetActive(false);
        storageButton.SetActive(false);
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
        else if (other.gameObject == storageSpawnPoint)
        {
            storageButton.SetActive(true);
        }
        else if (other.gameObject == patientSpawnPoint)
        {
            patientButton.SetActive(true);
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
        else if (other.gameObject == storageSpawnPoint)
        {
            storageButton.SetActive(false);
        }
        else if (other.gameObject == patientSpawnPoint)
        {
            patientButton.SetActive(false);
        }
    }
}
