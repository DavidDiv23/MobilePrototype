using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class SymptomChecker : MonoBehaviour
{
    public TMP_Text symptomText;
    public NPCRandomizer npcRandomizer;
    public DeceaseManager diseaseManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject npcRandomizerObject = GameObject.FindGameObjectWithTag("NPCRandomizer");
        GameObject diseaseManagerObject = GameObject.FindGameObjectWithTag("DiseaseManager");
        npcRandomizer = npcRandomizerObject.GetComponent<NPCRandomizer>();
        diseaseManager = diseaseManagerObject.GetComponent<DeceaseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SymptomCheck()
    {
        string correctDisease = diseaseManager.selectedDisease.diseaseName;

        if (symptomText != null)
        {
            for (int i = 0; i < npcRandomizer.symptoms.Count; i++)
            {
                if (symptomText.text == npcRandomizer.symptoms[i])
                {
                    diseaseManager.correctSymptoms[i] = true;
                    Debug.Log($"correct symptom");
                }

            }

            if (symptomText.text.Contains(correctDisease))
            {
                npcRandomizer.DestroyAllActiveNPCs();
                diseaseManager.RestartDiagnostics();
                Debug.Log($"correct disease");
            }
        }
    }
}
