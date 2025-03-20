using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientBehaviour : BT_agent
{
    public GameObject register;
    public GameObject[] rooms;
    public Transform hospitalExit;
    public int timeToWait = 10;
    public bool isWaiting;

    new void Start()
    {
        base.Start();
        register = GameObject.Find("Register");
        rooms = GameObject.FindGameObjectsWithTag("Room");
        hospitalExit = GameObject.Find("HospitalExit").transform;

        Sequence RootSeq = new Sequence("Root Sequence");

        // Sequence for patient to go to register
        Sequence registerSequence = new Sequence("Register Sequence");
        Leaf goingToRegister = new Leaf("Go to Register", GoToRegisterDesk);
        Leaf waitingForDiagnostic = new Leaf("Wait for Diagnostic", WaitAtRegister);
        registerSequence.AddChild(goingToRegister);
        registerSequence.AddChild(waitingForDiagnostic);

        // Sequence for patient to go to room
        Sequence gettingTreated = new Sequence("Getting Treated");
        RSelector randomRoomSelector = new RSelector("Random Room Selector");
        for (int i = 0; i < rooms.Length; i++)
        {
            Leaf goToRoom = new Leaf("Go to Room " + i, i, GoToRoom);
            randomRoomSelector.AddChild(goToRoom);
        }

        Leaf waitingForDoctor = new Leaf("Wait for Treatment", WaitForDoctor);
        Leaf goToExit = new Leaf("Go to Exit", GoToHospitalExit);

        gettingTreated.AddChild(randomRoomSelector);
        gettingTreated.AddChild(waitingForDoctor);
        gettingTreated.AddChild(goToExit);

        RootSeq.AddChild(registerSequence);
        RootSeq.AddChild(gettingTreated);
        tree.AddChild(RootSeq);
        tree.PrintTree();
    }

    private Node.Status GoToRegisterDesk()
    {
        Node.Status s = GoToLocation(register.transform.position);
        isWaiting = true;
        return s;
    }

    private Node.Status WaitAtRegister()
    {
        if (isWaiting)
        {
            StartCoroutine(WaitCoroutine(timeToWait)); // Start coroutine only once
            return Node.Status.FAILURE;
        }
        return Node.Status.SUCCESS;
    }

    private Node.Status GoToRoom(int val)
    {
        if (val >= rooms.Length)
        {
            return Node.Status.FAILURE;
        }

        Node.Status s = GoToLocation(rooms[val].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            Debug.Log("Arrived at Room " + val);
        }
        return s;
    }

    private Node.Status WaitForDoctor()
    {
        if (!isWaiting)
        {
            StartCoroutine(WaitCoroutine(timeToWait));
            return Node.Status.RUNNING;
        }
        return isWaiting ? Node.Status.RUNNING : Node.Status.SUCCESS;
    }

    private Node.Status GoToHospitalExit()
    {
        Node.Status s = GoToLocation(hospitalExit.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            Debug.Log("Patient leaving hospital.");
            Destroy(this.gameObject);
        }
        return s;
    }

    private IEnumerator WaitCoroutine(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isWaiting = false; 
    }
}
