using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientBehaviour : BTAgent
{
    public GameObject register;
    public GameObject[] rooms;
    new void Start()
    {
        base.Start();
        register = GameObject.Find("Register");
        rooms = GameObject.FindGameObjectsWithTag("Room");
        
        Sequence goToRegister = new Sequence("Register Sequence");
        Leaf goToRegisterDesk = new Leaf("Go to Register Desk", GoToRegisterDesk);
        goToRegister.AddChild(goToRegisterDesk);
        
        RSelector goToRandomRoom = new RSelector("RandomRoom");
        
        for (int i = 0; i < rooms.Length; i++)
        {
            Leaf goToRoom = new Leaf("Go to Room " + i, i, GoToRoom);
            goToRandomRoom.AddChild(goToRoom);
        }
        goToRegister.AddChild(goToRandomRoom);
        
        tree.AddChild(goToRegister);
        tree.PrintTree();
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
            Debug.Log("reached room");
        }
        return s;
    }

    private Node.Status GoToRegisterDesk()
    {
        Node.Status s = GoToLocation(register.transform.position);
        return s;
    }
}
