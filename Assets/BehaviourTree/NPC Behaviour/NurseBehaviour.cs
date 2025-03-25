using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NurseBehaviour : BTAgent
{
    //public GameObject patient;
    public GameObject[] tech;
    
    new void Start()
    {
        base.Start();
    
        RSelector selectObject = new RSelector("Select Object");
        for (int i = 0; i < tech.Length; i++)
        {
            Leaf gtt = new Leaf("Go to " + tech[i].name, i, GoToTech);
            selectObject.AddChild(gtt);
        }
        
        tree.AddChild(selectObject);
        //tree.PrintTree();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Node.Status GoToTech(int i)
    {
        if(!tech[i].activeSelf)
        {
            return Node.Status.FAILURE;
        }
        Node.Status s = GoToLocation(tech[i].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            tech[i].SetActive(false);
        }
        return s;
    }
    
}