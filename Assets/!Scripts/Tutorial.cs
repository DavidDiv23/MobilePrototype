using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject panelUI;
    public NavMeshAgent playerAgent;
    public GameObject hospitalEntrance;
    public DialogueRunner dialogueRunner;
    public GameObject hospitalButton;
    
    private InMemoryVariableStorage yarnVariables;

    private void Start()
    {
        panelUI.SetActive(false);
        hospitalButton.SetActive(false);
        yarnVariables = dialogueRunner.VariableStorage as InMemoryVariableStorage;
        
        dialogueRunner.onDialogueComplete.AddListener(CheckIntroComplete);
        dialogueRunner.onDialogueComplete.AddListener(PatientData);
    }

    private void PatientData()
    {
        if (dialogueRunner.CurrentNodeName == "ManagerDialogueAfterAnya")
        {
            //open data ui
        }
    }


    private void CheckIntroComplete()
    {
        if (dialogueRunner.CurrentNodeName == "AnyaIntro" &&
            yarnVariables != null &&
            yarnVariables.TryGetValue("$hasFinishedIntro", out bool hasFinished) &&
            hasFinished)
        {
            panelUI.SetActive(true);
            hospitalButton.SetActive(true);
            Debug.Log("Intro Complete – Show tutorial panel");
        }
    }

    public void GoToEntrance()
    {
        playerAgent.SetDestination(hospitalEntrance.transform.position);
    }

    public void DisableButton()
    {
        hospitalButton.SetActive(false);
        panelUI.SetActive(false);
    }
}
