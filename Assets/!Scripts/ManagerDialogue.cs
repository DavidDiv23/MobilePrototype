using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ManagerDialogue : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    public bool hasStartedDialogue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartDialogue()
    {
        dialogueRunner.StartDialogue("ManagerDialogue");
    }
}
