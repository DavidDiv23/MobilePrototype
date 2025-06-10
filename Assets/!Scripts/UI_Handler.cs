using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_Handler : MonoBehaviour
{
    public GameObject uiCanvas;
    public TextMeshProUGUI happinessText;
    public GameObject exclamationElement;
    public GameObject DialogueButton;
    public GameObject TreatmentButton;
    public GameObject DragAndDropMenu;
    public Image staminaBar;

    public GameObject patientLog;
    
    public bool isInDistance;
    public SphereCollider sphereCollider;

    [SerializeField] private GameObject blueprintsCanva;
    void Start()
    {
        uiCanvas.SetActive(false);
        exclamationElement.SetActive(false);
        DialogueButton.SetActive(false);
        TreatmentButton.SetActive(false);
        DragAndDropMenu.SetActive(false);
        patientLog.SetActive(false);
        sphereCollider = GetComponent<SphereCollider>();
    }
    
    public IEnumerator ShowCanvas()
    {
        
        uiCanvas.SetActive(true);
        happinessText.text = "Happiness: " + '+' + GetComponent<NPC_Personality_Handler>().happiness.ToString();
        yield return new WaitForSeconds(2f);
        uiCanvas.SetActive(false);
    }
    public void HideCanvas()
    {
        exclamationElement.SetActive(false);
    }
    
    public void ShowExclamation()
    {
        exclamationElement.SetActive(true);
    }
    
    public void ShowDialogueAndTreatmentButtons()
    {
        exclamationElement.SetActive(false);
        DialogueButton.SetActive(true);
        TreatmentButton.SetActive(true);
    }
    public void HideDialogueAndTreatmentButtons()
    {
        DialogueButton.SetActive(false);
        TreatmentButton.SetActive(false);
    }
    public void ShowDragAndDropButton()
    {
        DragAndDropMenu.SetActive(true);
    }
    public void HideDragAndDropButton()
    {
        DragAndDropMenu.SetActive(false);
    }
    public void ReduceStamina()
    {
        staminaBar.fillAmount = staminaBar.fillAmount - 0.1f;
    }
    
    public void ShowPatientLog()
    {
        patientLog.SetActive(true);
    }
    public void HidePatientLog()
    {
        patientLog.SetActive(false);
    }
    public void HideBlueprints()
    {
        blueprintsCanva.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ShowExclamation();
            isInDistance = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HideDialogueAndTreatmentButtons();
            isInDistance = false;
            HideCanvas();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInDistance = true;
        }
    }
}
