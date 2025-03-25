using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SearchBarManager : MonoBehaviour
{
    public GameObject contentHolder;
    public List<GameObject> symptoms = new List<GameObject>();
    public GameObject searchbar;
    public int totalElements;

    public void GetSymptoms()
    {
        symptoms.Clear(); // Ensure the list is emptied before adding new items
        totalElements = contentHolder.transform.childCount;

        // Debugging to verify list is cleared and elements are counted correctly
        Debug.Log("Clearing symptoms list. Current count: " + symptoms.Count);

        for (int i = 0; i < totalElements; i++)
        {
            GameObject child = contentHolder.transform.GetChild(i).gameObject;
            symptoms.Add(child);
        }

        Debug.Log("New symptoms list count: " + symptoms.Count);
    }

    public void Search()
    {
        string searchText = searchbar.GetComponent<TMP_InputField>().text;
        int searchTextLength = searchText.Length;

        foreach (GameObject ele in symptoms)
        {
            if (ele.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Length >= searchTextLength)
            {
                if (searchText.ToLower() == ele.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Substring(0, searchTextLength).ToLower())
                {
                    ele.SetActive(true);
                }
                else
                {
                    ele.SetActive(false);
                }
            }
        }
    }
}

