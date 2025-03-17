using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[Serializable] // Allows editing in the Inspector
public class Disease
{
    public string diseaseName;
    public List<string> symptoms = new List<string>();
    public DiseaseRarity rarity;
}

public enum DiseaseRarity
{
    Common,
    Uncommon,
    Rare,
    VeryRare
}

public class DeceaseManager : MonoBehaviour
{
    public NPCRandomizer npcRandomizer;

    [Header("UI References")]
    [SerializeField] private GameObject symptomPrefab;
    [SerializeField] private Transform contentPanel;

    [Header("Diseases List")]
    [SerializeField] private List<Disease> diseases = new List<Disease>();

    [Header("Rarity Percentages (Must Add to 100)")]
    [Range(0, 100)] public float commonChance = 50f;
    [Range(0, 100)] public float uncommonChance = 30f;
    [Range(0, 100)] public float rareChance = 15f;
    [Range(0, 100)] public float veryRareChance = 5f;

    private List<string> sortedSymptoms = new List<string>();

    public Disease selectedDisease;
    public List<string> resultLogs = new List<string>();
    public List<bool> correctSymptoms = new List<bool>();

    public SearchBarManager searchBarManager;

    void Start()
    {
        GenerateUniqueSymptomsList();
        PrintSymptoms();
        RandomizeNPC();
    }

    void Update()
    {
        if (correctSymptoms.All(b => b))
        {
            Console.WriteLine("All values are true!");
            DeleteSymptoms();
            StartCoroutine(WaitAndPrintDiseases());

            // Set all values back to false
            for (int i = 0; i < correctSymptoms.Count; i++)
            {
                correctSymptoms[i] = false;
            }
        }
    }

    private IEnumerator WaitAndPrintDiseases()
    {
        yield return new WaitForEndOfFrame(); // Waits until Unity updates the scene
        PrintDiseases();
    }

    private void GenerateUniqueSymptomsList()
    {
        HashSet<string> uniqueSymptoms = new HashSet<string>();
        foreach (var disease in diseases)
        {
            uniqueSymptoms.UnionWith(disease.symptoms);
        }
        sortedSymptoms = new List<string>(uniqueSymptoms);
        sortedSymptoms.Sort();
    }

    public void PrintSymptoms()
    {
        foreach (string symptom in sortedSymptoms)
        {
            GameObject newSymptomItem = Instantiate(symptomPrefab, contentPanel);
            TMP_Text textComponent = newSymptomItem.transform.Find("SymptomText").GetComponent<TMP_Text>();
            textComponent.text = symptom;
        }
        searchBarManager.GetSymptoms();
    }

    public void DeleteSymptoms()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

    }

    public void PrintDiseases()
    {
        foreach (string result in resultLogs)
        {
            GameObject newResultChoice = Instantiate(symptomPrefab, contentPanel);
            TMP_Text textComponent = newResultChoice.transform.Find("SymptomText").GetComponent<TMP_Text>();
            textComponent.text = result;
        }
        searchBarManager.GetSymptoms();
    }

    public void RandomizeNPC()
    {
        resultLogs.Clear(); // Clear previous results

        selectedDisease = SelectDiseaseByRarity();
        if (selectedDisease == null) return;

        List<string> selectedSymptoms = SelectSymptomsForNPC(selectedDisease);
        Dictionary<Disease, float> diseaseProbabilities = CalculateDiseaseProbabilities(selectedSymptoms, selectedDisease);

        foreach (var entry in diseaseProbabilities)
        {
            resultLogs.Add($"{entry.Key.diseaseName}: {entry.Value:F2}%");
        }

        npcRandomizer.symptoms = selectedSymptoms;
        npcRandomizer.SpawnRandomNPC();

        foreach (string log in resultLogs)
        {
            Debug.Log(log);
        }
    }

    private Disease SelectDiseaseByRarity()
    {
        float randomValue = UnityEngine.Random.Range(0f, 100f);
        float cumulativeChance = 0;

        List<Disease> filteredDiseases = new List<Disease>();

        if (randomValue < (cumulativeChance += commonChance))
            filteredDiseases = diseases.FindAll(d => d.rarity == DiseaseRarity.Common);
        else if (randomValue < (cumulativeChance += uncommonChance))
            filteredDiseases = diseases.FindAll(d => d.rarity == DiseaseRarity.Uncommon);
        else if (randomValue < (cumulativeChance += rareChance))
            filteredDiseases = diseases.FindAll(d => d.rarity == DiseaseRarity.Rare);
        else
            filteredDiseases = diseases.FindAll(d => d.rarity == DiseaseRarity.VeryRare);

        if (filteredDiseases.Count == 0) return null;

        return filteredDiseases[UnityEngine.Random.Range(0, filteredDiseases.Count)];
    }

    private List<string> SelectSymptomsForNPC(Disease disease)
    {
        int symptomCountRoll = UnityEngine.Random.Range(0, 100);
        int symptomsToPick = symptomCountRoll < 70 ? 3 : (symptomCountRoll < 90 ? 2 : 1);

        List<string> chosenSymptoms = new List<string>();
        List<string> diseaseSymptoms = new List<string>(disease.symptoms);
        List<string> availableSymptoms = new List<string>(sortedSymptoms);
        availableSymptoms.RemoveAll(diseaseSymptoms.Contains);

        for (int i = 0; i < symptomsToPick; i++)
        {
            if (diseaseSymptoms.Count > 0 && (i == 0 || UnityEngine.Random.value < 0.75f))
            {
                int index = UnityEngine.Random.Range(0, diseaseSymptoms.Count);
                chosenSymptoms.Add(diseaseSymptoms[index]);
                diseaseSymptoms.RemoveAt(index);
            }
            else if (availableSymptoms.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, availableSymptoms.Count);
                chosenSymptoms.Add(availableSymptoms[index]);
                availableSymptoms.RemoveAt(index);
            }
        }

        return chosenSymptoms;
    }

    private Dictionary<Disease, float> CalculateDiseaseProbabilities(List<string> npcSymptoms, Disease actualDisease)
    {
        Dictionary<Disease, float> diseaseScores = new Dictionary<Disease, float>();

        foreach (var disease in diseases)
        {
            int matchingSymptoms = npcSymptoms
                .Select(s => s.ToLower().Trim())
                .Intersect(disease.symptoms.Select(s => s.ToLower().Trim()))
                .Count();

            if (matchingSymptoms == 0) continue;

            float rarityProbability = GetRarityProbability(disease.rarity);
            float score = matchingSymptoms * rarityProbability; // Use rarity chance instead of weight

            diseaseScores[disease] = score;
        }

        // Normalize scores to probabilities
        float totalScore = diseaseScores.Values.Sum();
        Dictionary<Disease, float> probabilityResults = new Dictionary<Disease, float>();

        if (totalScore > 0)
        {
            foreach (var entry in diseaseScores)
            {
                probabilityResults[entry.Key] = (entry.Value / totalScore) * 100f;
            }
        }

        // Sort alphabetically for fairness
        return probabilityResults
            .OrderBy(x => x.Key.diseaseName)
            .ToDictionary(x => x.Key, x => x.Value);
    }


    private float GetRarityProbability(DiseaseRarity rarity)
    {
        return rarity switch
        {
            DiseaseRarity.Common => commonChance,
            DiseaseRarity.Uncommon => uncommonChance,
            DiseaseRarity.Rare => rareChance,
            DiseaseRarity.VeryRare => veryRareChance,
            _ => 0f, // Default case (shouldn't happen)
        };
    }

    public void RestartDiagnostics()
    {
        DeleteSymptoms();
        StartCoroutine(WaitAndPrintSymptoms());
    }

    private IEnumerator WaitAndPrintSymptoms()
    {
        yield return new WaitForEndOfFrame(); // Waits until Unity updates the scene
        PrintSymptoms();
        RandomizeNPC();
    }

}
