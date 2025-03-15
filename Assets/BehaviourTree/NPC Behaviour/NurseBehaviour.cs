using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NurseBehaviour : BT_agent
{
    public GameObject patient;
    public GameObject[] tech;
    public GameObject[] patients;
    
    new void Start()
    {
        base.Start();
        
        Leaf goToTech = new Leaf("Go to Tech", GoToTech);
        Leaf goToPatient = new Leaf("Go to Patient", GoToPatient);

        for (int i = 0; i < patients.Length; i++)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Node.Status GoToPatient(int i)
    {
        if(!patients[i].activeSelf)
        {
            return Node.Status.FAILURE;
        }
    }
    
}
