using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCRandomizer : MonoBehaviour
{
    public List<string> symptoms = new List<string>();

    public List<GameObject> npcs = new List<GameObject>();
    [SerializeField] private Transform spawnPoint;

    private List<GameObject> activeNPCs = new List<GameObject>(); // To track spawned NPCs

    // Start is called before the first frame update
   

    public void SpawnRandomNPC()
    {
        if (npcs.Count == 0) return; // Prevent errors if the list is empty

        int randomIndex = UnityEngine.Random.Range(0, npcs.Count); // Pick a random prefab
        GameObject randomObject = npcs[randomIndex];

        // Instantiate the object and add it to the activeNPCs list
        GameObject npcInstance = Instantiate(randomObject, spawnPoint.position, Quaternion.identity);
        activeNPCs.Add(npcInstance); // Track the spawned NPC
        NPC npcScript = npcInstance.GetComponent<NPC>();
        npcScript.symptoms = symptoms;
    }

    // Call this method when an NPC is destroyed
    public void OnNPCDestroyed(GameObject npc)
    {
        activeNPCs.Remove(npc); // Remove from the active list when an NPC is destroyed
    }

    public void DestroyAllActiveNPCs()
    {
        foreach (GameObject npc in activeNPCs)
        {
            Destroy(npc); // Destroy each NPC in the list
        }

        activeNPCs.Clear(); // Clear the list after destruction
    }
}
