using UnityEngine;

public class DoctorBehaviour : BT_agent
{
    
    public GameObject[] patients;
    
    new void Start()
    {
        base.Start();
    
        RSelector selectObject = new RSelector("Select Object");
        for (int i = 0; i < patients.Length; i++)
        {
            Leaf gtt = new Leaf("Go to " + patients[i].name, i, GoToTech);
            selectObject.AddChild(gtt);
        }
        
        tree.AddChild(selectObject);
        //tree.PrintTree();
    }
    
    
    public Node.Status GoToTech(int i)
    {
        if(!patients[i].activeSelf)
        {
            return Node.Status.FAILURE;
        }
        Node.Status s = GoToLocation(patients[i].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            patients[i].SetActive(false);
        }
        return s;
    }
    
}
