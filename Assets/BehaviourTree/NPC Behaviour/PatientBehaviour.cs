using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PatientBehaviour : BTAgent
{
    public GameObject register;
    public GameObject[] rooms;
    public Transform hospitalExit;
    new void Start()
    {
        base.Start();
        register = GameObject.Find("Register");
        rooms = GameObject.FindGameObjectsWithTag("Room");
        hospitalExit = GameObject.Find("HospitalExit").transform;
        
        Sequence RootSeq = new Sequence("Root Sequence");
        
        //Sequence for patient to go to register
        Sequence registerSequence = new Sequence("Register Sequence");
        Leaf goingToRegister = new Leaf("Go to Register", GoToRegisterDesk);
        Leaf waitingForDiagnostic = new Leaf("Wait for Diagnostic", WaitForDiagnostic);
        registerSequence.AddChild(goingToRegister);
        registerSequence.AddChild(waitingForDiagnostic);
        
        //Sequence for patient to go to room
        Sequence gettingTreated = new Sequence("Getting Treated");
        RSelector randomRoomSelector = new RSelector("Random Room Selector");
        for (int i = 0; i < rooms.Length; i++)
        {
            Leaf goToRoom = new Leaf("Go to Room " + i, i, GoToRoom);
            randomRoomSelector.AddChild(goToRoom);
        }
        gettingTreated.AddChild(randomRoomSelector);
        Leaf goToExit = new Leaf("Go to Exit", GoToHospitalExit);
        gettingTreated.AddChild(goToExit);
        
        RootSeq.AddChild(registerSequence);
        RootSeq.AddChild(gettingTreated);
        tree.AddChild(RootSeq);
        tree.PrintTree();
    }


    private Node.Status WaitForDiagnostic()
    {
        Debug.Log("Wait for Diagnostic");
        return Node.Status.SUCCESS;
    }

    private Node.Status GoToRoom(int val)
    {
        if (val > rooms.Length)
        {
            return Node.Status.FAILURE;
        }
        Node.Status s = GoToLocation(rooms[val].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            Debug.Log("Go to Room " + val);
        }
        return s;
    }
    private Node.Status GoToHospitalExit()
    {
        Node.Status s = GoToLocation(hospitalExit.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            Destroy(this.gameObject);
        }
        return s;
    }

    private Node.Status GoToRegisterDesk()
    {
        Node.Status s = GoToLocation(register.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            Debug.Log("at register");
        }
        
        return s;
    }
}
