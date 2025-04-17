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
    public GameObject enterButton;
    public GameObject interiorSpawnPoint;
    public GameObject houseEntrance;
    public GameObject exitButton;
    public Canvas exteriorWorldSpaceCanvas;
    public Canvas interiorWorldSpaceCanvas;
    public SimpleMovement simpleMovement;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void TeleportToInterior()
    {
        agent.Warp(interiorSpawnPoint.transform.position);
        interiorCamera.gameObject.SetActive(true);
        externalCamera.gameObject.SetActive(false);
        
        exteriorWorldSpaceCanvas.worldCamera = interiorCamera;
        interiorWorldSpaceCanvas.worldCamera = interiorCamera;
        simpleMovement.SetCamera(interiorCamera);
    }

    public void TeleportToExterior()
    {
        agent.Warp(houseEntrance.transform.position);
        interiorCamera.gameObject.SetActive(false);
        externalCamera.gameObject.SetActive(true);
        
        interiorWorldSpaceCanvas.worldCamera = externalCamera;
        exteriorWorldSpaceCanvas.worldCamera = externalCamera;
        simpleMovement.SetCamera(externalCamera);
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
