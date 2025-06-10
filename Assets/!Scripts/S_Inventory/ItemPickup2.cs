using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPickup2 : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemSO itemData;
    public int amount = 1;
    public float pickupDistance = 10f;
    public Vector3 canvasOffset = new Vector3(0, 2f, 0);
    public float respawnTime = 30f; // New: Time in seconds before item respawns

    [Header("UI Settings")]
    public Canvas pickupCanvas; // Assign child canvas in Inspector
    private Button pickupButton;
    private SphereCollider proximityCollider;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isRespawning = false;
    private float respawnTimer = 0f;

    private void Start()
    {
        // Save original transform
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Set up proximity collider
        proximityCollider = gameObject.AddComponent<SphereCollider>();
        proximityCollider.radius = pickupDistance;
        proximityCollider.isTrigger = true;

        // Canvas setup
        if (pickupCanvas != null)
        {
            pickupCanvas.transform.localPosition = canvasOffset;
            pickupCanvas.gameObject.SetActive(false);

            pickupButton = pickupCanvas.GetComponentInChildren<Button>();
            if (pickupButton != null)
            {
                pickupButton.onClick.AddListener(PickUp);
            }
            else
            {
                Debug.LogError("No button found in pickup canvas!", this);
            }
        }
        else
        {
            Debug.LogError("Pickup canvas not assigned!", this);
        }

        if (itemData == null)
        {
            Debug.LogError("No ItemSO assigned to this pickup!", this);
        }
    }

    private void Update()
    {
        if (isRespawning)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0f)
            {
                RespawnItem();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRespawning)
        {
            pickupCanvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupCanvas.gameObject.SetActive(false);
        }
    }

    public void PickUp()
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null)
        {
            Item item = new Item { itemData = itemData, amount = amount };
            inventoryManager.AddItem(item);
            StartRespawnTimer();
        }
        else
        {
            Debug.LogError("InventoryManager not found!");
        }
    }

    private void StartRespawnTimer()
    {
        // Disable all visible and interactive components
        SetItemActive(false);

        isRespawning = true;
        respawnTimer = respawnTime;
    }

    private void RespawnItem()
    {
        // Reset position and rotation
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // Enable all components
        SetItemActive(true);

        isRespawning = false;
    }

    private void SetItemActive(bool active)
    {
        // Disable/enable renderers and colliders
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = active;
        }

        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = active;
        }

        if (pickupCanvas != null)
        {
            pickupCanvas.gameObject.SetActive(false); // Always keep canvas hidden initially
        }
    }

    private void OnDestroy()
    {
        if (pickupButton != null)
        {
            pickupButton.onClick.RemoveListener(PickUp);
        }
    }

    // Visualize pickup radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }
}