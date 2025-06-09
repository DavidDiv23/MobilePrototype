using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup2 : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemSO itemData;
    public int amount = 1;
    public float pickupDistance = 5f;
    public Vector3 buttonOffset = new Vector3(0, 2f, 0); // Offset above item

    [Header("UI Settings")]
    public GameObject pickupButtonPrefab; // Assign a UI Button prefab
    private GameObject currentButton;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player not found! Tag your player as 'Player'");
        }

        if (itemData == null)
        {
            Debug.LogError("No ItemSO assigned to this pickup!", gameObject);
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool inRange = distance <= pickupDistance;

        HandleButtonVisibility(inRange);
    }

    private void HandleButtonVisibility(bool inRange)
    {
        if (inRange)
        {
            if (currentButton == null)
            {
                // Create button if it doesn't exist
                currentButton = Instantiate(pickupButtonPrefab, FindObjectOfType<Canvas>().transform);
                currentButton.GetComponent<Button>().onClick.AddListener(PickUp);
            }

            // Position button above item
            if (currentButton != null)
            {
                currentButton.transform.position = Camera.main.WorldToScreenPoint(transform.position + buttonOffset);
            }
        }
        else
        {
            // Remove button when out of range
            if (currentButton != null)
            {
                Destroy(currentButton);
                currentButton = null;
            }
        }
    }

    public void PickUp()
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null)
        {
            Item item = new Item { itemData = itemData, amount = amount };
            inventoryManager.AddItem(item);

            // Clean up before destroying
            if (currentButton != null)
            {
                Destroy(currentButton);
            }
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("InventoryManager not found!");
        }
    }

    private void OnDestroy()
    {
        if (currentButton != null)
        {
            Destroy(currentButton);
        }
    }
}