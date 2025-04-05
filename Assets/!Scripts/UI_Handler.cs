using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Handler : MonoBehaviour
{
    public GameObject uiCanvas;
    public TextMeshProUGUI happinessText;
    void Start()
    {
        uiCanvas.SetActive(false);
    }

    public IEnumerator ShowCanvas()
    {
        uiCanvas.SetActive(true);
        happinessText.text = "Happiness: " + '+' + GetComponent<NPC_Personality_Handler>().happiness.ToString();
        yield return new WaitForSeconds(2f);
        uiCanvas.SetActive(false);
    }
}
