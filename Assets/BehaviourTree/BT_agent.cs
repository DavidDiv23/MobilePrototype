using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BT_agent : MonoBehaviour
{
    public enum ActionState
    {
        IDLE,
        WORKING
    }
    public ActionState state = ActionState.IDLE;
    public Node.Status treeStatus = Node.Status.RUNNING;

    public BehaviourTree tree;
    public NavMeshAgent agent;
    
    WaitForSeconds waitForSeconds;

    private Vector3 rememberLocation;
    
    public void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        tree = new BehaviourTree("Tree");
        waitForSeconds = new WaitForSeconds(Random.Range(0.1f, 1f));

        StartCoroutine(Behave());
    }
    
    
    public Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);

        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2 && !agent.pathPending)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }

    IEnumerator Behave()
    {
        while (true)
        {
            treeStatus = tree.Process();
            yield return waitForSeconds;
        }
    }
}
