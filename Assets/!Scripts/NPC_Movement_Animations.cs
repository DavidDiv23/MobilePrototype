using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Movement_Animations : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator animator;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }
    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
