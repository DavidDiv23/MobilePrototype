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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPatient()
    {
        Instantiate(patient, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }
}
