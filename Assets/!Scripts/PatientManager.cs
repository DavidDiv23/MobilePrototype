using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
    public static PatientManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    private List<GameObject> patients = new List<GameObject>();
    
    public void AddPatient(GameObject patient)
    {
        patients.Add(patient);
    }
    
    public void RemovePatient(GameObject patient)
    {
        patients.Remove(patient);
    }

    public List<GameObject> GetPatients()
    {
        return patients;
    }
}
