using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawningPatients : MonoBehaviour
{
    public GameObject patient;
    public GameObject spawnPoint;
    
    void Start()
    {
        Debug.Log("Spawning Patient");
    }

    private void Update()
    {
        //SpawnPatient();
    }

    public void SpawnPatient()
    {
        GameObject patients = Instantiate(patient, spawnPoint.transform.position, spawnPoint.transform.rotation);
        Blackboard.Instance.RegisterPatient(patients);
        Debug.Log(Blackboard.Instance.patients.Count);
    }
}
