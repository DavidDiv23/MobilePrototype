using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;

public class NPC : MonoBehaviour
{
    public string name;
    public string greeting;
    public List<string> symptoms = new List<string>();
    public List<string> dialogue1 = new List<string>();
    public List<string> dialogue2 = new List<string>();
    public GameObject textBoxObject;

    // Start is called before the first frame update
    void Start()
    {
        textBoxObject = GameObject.FindGameObjectWithTag("Dialogue");
        DialogueSystem dialogueScript = textBoxObject.GetComponent<DialogueSystem>();
        dialogueScript.dialogue[0] = greeting +", I'm " + name;
        
        for (int i = 0; i < symptoms.Count; i++)
        {
            dialogueScript.dialogue[i+1] = dialogue1[i] + " " + symptoms[i] + " " + dialogue2[i];
        }

        if (dialogueScript != null)
        {
            dialogueScript.StartDialogue(); // Update text dynamically
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
