using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UI_Handler : MonoBehaviour
{
    public GameObject uiCanvas;
    public TextMeshProUGUI happinessText;
    public GameObject exclamationElement;
    public GameObject DialogueButton;
    public GameObject TreatmentButton;
    public GameObject DragAndDropMenu;
    
    void Start()
    {
        uiCanvas.SetActive(false);
        exclamationElement.SetActive(false);
        DialogueButton.SetActive(false);
        TreatmentButton.SetActive(false);
        DragAndDropMenu.SetActive(false);
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
        Debug.Log("Clicked");
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
}
