using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public sealed class HouseEntrance : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private Camera externalCamera;
    [SerializeField] private Camera interiorCamera;
    [SerializeField] private Camera storageCamera;
    [SerializeField] private Camera patientCamera;

    [SerializeField] private GameObject enterButton;
    [SerializeField] private GameObject interiorSpawnPoint;
    [SerializeField] private GameObject houseEntrance;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject patientButton;
    [SerializeField] private GameObject storageButton;
    [SerializeField] private GameObject storageSpawnPoint;
    [SerializeField] private GameObject patientSpawnPoint;

    [SerializeField] private Canvas exteriorWorldSpaceCanvas;
    [SerializeField] private Canvas interiorWorldSpaceCanvas;

    [SerializeField] private SimpleMovement simpleMovement;
    [SerializeField] private GameObject canvasForInterior;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        canvasForInterior.SetActive(false);
    }

    private void Teleport(Vector3 destination, Camera activeCamera)
    {
        agent.Warp(destination);

        externalCamera.gameObject.SetActive(false);
        interiorCamera.gameObject.SetActive(false);
        storageCamera.gameObject.SetActive(false);
        patientCamera.gameObject.SetActive(false);
        activeCamera.gameObject.SetActive(true);

        exteriorWorldSpaceCanvas.worldCamera = activeCamera;
        interiorWorldSpaceCanvas.worldCamera = activeCamera;
        simpleMovement.SetCamera(activeCamera);

        HideInteriorUI();
    }

    public void TeleportToInterior() => Teleport(interiorSpawnPoint.transform.position, interiorCamera);
    public void TeleportToExterior() => Teleport(houseEntrance.transform.position, externalCamera);
    public void TeleportToStorage() => Teleport(storageSpawnPoint.transform.position, storageCamera);
    public void TeleportToPatient() => Teleport(patientSpawnPoint.transform.position, patientCamera);

    public void ShowInteriorUI() => canvasForInterior.SetActive(true);

    public void HideInteriorUI()
    {
        canvasForInterior.SetActive(false);
        enterButton.SetActive(false);
        exitButton.SetActive(false);
        patientButton.SetActive(false);
        storageButton.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == houseEntrance) enterButton.SetActive(true);
        else if (other.gameObject == interiorSpawnPoint) exitButton.SetActive(true);
        else if (other.gameObject == storageSpawnPoint) storageButton.SetActive(true);
        else if (other.gameObject == patientSpawnPoint) patientButton.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == houseEntrance) enterButton.SetActive(false);
        else if (other.gameObject == interiorSpawnPoint) exitButton.SetActive(false);
        else if (other.gameObject == storageSpawnPoint) storageButton.SetActive(false);
        else if (other.gameObject == patientSpawnPoint) patientButton.SetActive(false);
    }
}
