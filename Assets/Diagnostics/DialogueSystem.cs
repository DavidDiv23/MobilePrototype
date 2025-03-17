using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public List<string> dialogue = new List<string>();
    private int line;
    private int dialogueMax;
    public NPCRandomizer npcRandomizer;

    [SerializeField] private TMP_Text textBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
         
    }

    public void StartDialogue()
    {
        if (textBox != null)
        {
            textBox.text = dialogue[0];
        }
        dialogueMax = dialogue.Count;
        line = 0;
    }

    public void NextDialogue()
    {
        line++;

            if (line < dialogueMax)
            {
             textBox.text = dialogue[line];
            }
            else
            {
            textBox.text = "";
            npcRandomizer.DestroyAllActiveNPCs();
            }
        
    }
}
