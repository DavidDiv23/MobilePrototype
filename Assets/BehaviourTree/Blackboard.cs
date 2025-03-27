using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackboard : MonoBehaviour
{
    public int timeOfDay; // in minutes
    public Text clock;
    public Stack<GameObject> patients = new Stack<GameObject>();
    public int openTime = 6 * 60;  // 6:00 AM in minutes
    public int closeTime = 14 * 60; // 2:00 PM (8-hour shift)
    public SpawningPatients spawningPatients;

    static Blackboard instance;
    public static Blackboard Instance
    {
        get
        {
            if (!instance)
            {
                Blackboard[] blackboards = GameObject.FindObjectsOfType<Blackboard>();
                if (blackboards != null && blackboards.Length == 1)
                {
                    instance = blackboards[0];
                }
                else
                {
                    GameObject go = new GameObject("Blackboard", typeof(Blackboard));
                    instance = go.GetComponent<Blackboard>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
        set
        {
            instance = value as Blackboard;
        }
    }

    void Start()
    {
        timeOfDay = openTime;
        StartCoroutine(UpdateClock());
    }

    IEnumerator UpdateClock()
    {
        while (timeOfDay < closeTime)
        {
            UpdateClockDisplay();
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            timeOfDay++;
            spawningPatients.SpawnPatient();
        }

        // End of day
        patients.Clear();
        UpdateClockDisplay(); // final time
    }

    void UpdateClockDisplay()
    {
        int hours = timeOfDay / 60;
        int minutes = timeOfDay % 60;
        clock.text = $"{hours:D2}:{minutes:D2}";
    }

    public bool RegisterPatient(GameObject p)
    {
        patients.Push(p);
        return true;
    }
}