using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemSO itemData; // Reference to ScriptableObject item
    public int amount = 1;
    public float pickupDistance = 2f;
    public float touchRadius = 0.5f;

    [Header("Visual Feedback")]
    public Material normalMaterial;
    public Material highlightMaterial;
    public float highlightIntensity = 1.5f;
    public bool useEmission = true;

    [Header("Debug")]
    public bool showDebugInfo = true;
    public Color debugGizmoColor = Color.green;

    private Transform playerTransform;
    private Renderer itemRenderer;
    private Material originalMaterial;
    private Color originalEmissionColor;
    private bool isHighlighted = false;
    private Collider itemCollider;

    private void Start()
    {
        // Find player
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player not found! Tag your player as 'Player'");
        }

        // Get renderer and collider components
        itemRenderer = GetComponent<Renderer>();
        itemCollider = GetComponent<Collider>();

        if (itemRenderer != null)
        {
            originalMaterial = itemRenderer.material;
            if (useEmission && originalMaterial.HasProperty("_EmissionColor"))
            {
                originalEmissionColor = originalMaterial.GetColor("_EmissionColor");
            }
        }
        else
        {
            Debug.LogWarning("No Renderer found on pickup item - visual feedback won't work");
        }

        if (itemCollider == null)
        {
            Debug.LogError("No Collider found on pickup item - item won't be pickable!");
        }

        // Validate item data
        if (itemData == null)
        {
            Debug.LogError("No ItemSO assigned to this pickup!", gameObject);
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool nowInRange = distance <= pickupDistance;

        if (nowInRange != isHighlighted)
        {
            isHighlighted = nowInRange;
            UpdateVisualFeedback();
        }

        ProcessTouchInput();
    }

    private void ProcessTouchInput()
    {
        if (!isHighlighted || Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            // Debug raycast in editor
            if (showDebugInfo)
            {
                Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);
            }

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Check if we hit this object or nearby (using touchRadius)
                float hitDistance = Vector3.Distance(hit.point, transform.position);
                bool isDirectHit = hit.transform == transform;
                bool isNearbyHit = hitDistance <= touchRadius;

                if (showDebugInfo)
                {
                    Debug.Log($"Touch detected on {hit.transform.name}. Direct: {isDirectHit}, Nearby: {isNearbyHit}, Distance: {hitDistance}");
                }

                if (isDirectHit || isNearbyHit)
                {
                    PickUp();
                }
            }
        }
    }

    private void UpdateVisualFeedback()
    {
        if (itemRenderer == null) return;

        if (highlightMaterial != null)
        {
            itemRenderer.material = isHighlighted ? highlightMaterial : originalMaterial;
        }
        else if (useEmission && itemRenderer.material.HasProperty("_EmissionColor"))
        {
            if (isHighlighted)
            {
                itemRenderer.material.SetColor("_EmissionColor", originalEmissionColor * highlightIntensity);
                itemRenderer.material.EnableKeyword("_EMISSION");
            }
            else
            {
                itemRenderer.material.SetColor("_EmissionColor", originalEmissionColor);
            }
        }
    }

    public void PickUp()
    {
        if (showDebugInfo) Debug.Log($"Attempting to pick up {itemData?.itemName}");

        if (itemData == null)
        {
            Debug.LogError("Cannot pick up - no item data assigned!", gameObject);
            return;
        }

        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null)
        {
            Item item = new Item { itemData = itemData, amount = amount };
            inventoryManager.AddItem(item);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("InventoryManager not found!");
        }
    }

    private void OnDestroy()
    {
        if (itemRenderer != null && originalMaterial != null)
        {
            itemRenderer.material = originalMaterial;
        }
    }

    // Visualize pickup radius in editor
    private void OnDrawGizmosSelected()
    {
        if (showDebugInfo)
        {
            Gizmos.color = debugGizmoColor;
            Gizmos.DrawWireSphere(transform.position, pickupDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, touchRadius);
        }
    }

    // Auto-assign icon if available
    private void OnValidate()
    {
        if (itemData != null && itemData.icon != null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = itemData.icon;
            }
        }
    }



    /*
    [Header("Item Settings")]
    public Item.ItemType itemType;
    public int amount = 1;
    public float pickupDistance = 2f;
    public float touchRadius = 0.5f;

    [Header("Visual Feedback")]
    public Material normalMaterial;
    public Material highlightMaterial;
    public float highlightIntensity = 1.5f;
    public bool useEmission = true;

    [Header("Debug")]
    public bool showDebugInfo = true;
    public Color debugGizmoColor = Color.green;

    private Transform playerTransform;
    private Renderer itemRenderer;
    private Material originalMaterial;
    private Color originalEmissionColor;
    private bool isHighlighted = false;
    private Collider itemCollider;

    private void Start()
    {
        // Find player
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player not found! Tag your player as 'Player'");
        }

        // Get renderer and collider components
        itemRenderer = GetComponent<Renderer>();
        itemCollider = GetComponent<Collider>();

        if (itemRenderer != null)
        {
            originalMaterial = itemRenderer.material;
            if (useEmission && originalMaterial.HasProperty("_EmissionColor"))
            {
                originalEmissionColor = originalMaterial.GetColor("_EmissionColor");
            }
        }
        else
        {
            Debug.LogWarning("No Renderer found on pickup item - visual feedback won't work");
        }

        if (itemCollider == null)
        {
            Debug.LogError("No Collider found on pickup item - item won't be pickable!");
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool nowInRange = distance <= pickupDistance;

        if (nowInRange != isHighlighted)
        {
            isHighlighted = nowInRange;
            UpdateVisualFeedback();
        }

        ProcessTouchInput();
    }

    private void ProcessTouchInput()
    {
        if (!isHighlighted || Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            // Debug raycast in editor
            if (showDebugInfo)
            {
                Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);
            }

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Check if we hit this object or nearby (using touchRadius)
                float hitDistance = Vector3.Distance(hit.point, transform.position);
                bool isDirectHit = hit.transform == transform;
                bool isNearbyHit = hitDistance <= touchRadius;

                if (showDebugInfo)
                {
                    Debug.Log($"Touch detected on {hit.transform.name}. Direct: {isDirectHit}, Nearby: {isNearbyHit}, Distance: {hitDistance}");
                }

                if (isDirectHit || isNearbyHit)
                {
                    PickUp();
                }
            }
        }
    }

    private void UpdateVisualFeedback()
    {
        if (itemRenderer == null) return;

        if (highlightMaterial != null)
        {
            itemRenderer.material = isHighlighted ? highlightMaterial : originalMaterial;
        }
        else if (useEmission && itemRenderer.material.HasProperty("_EmissionColor"))
        {
            if (isHighlighted)
            {
                itemRenderer.material.SetColor("_EmissionColor", originalEmissionColor * highlightIntensity);
                itemRenderer.material.EnableKeyword("_EMISSION");
            }
            else
            {
                itemRenderer.material.SetColor("_EmissionColor", originalEmissionColor);
            }
        }
    }

    public void PickUp()
    {
        if (showDebugInfo) Debug.Log($"Attempting to pick up {itemType}");

        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null)
        {
            Item item = new Item { itemtype = itemType, amount = amount };
            inventoryManager.AddItem(item);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("InventoryManager not found!");
        }
    }

    private void OnDestroy()
    {
        if (itemRenderer != null && originalMaterial != null)
        {
            itemRenderer.material = originalMaterial;
        }
    }

    // Visualize pickup radius in editor
    private void OnDrawGizmosSelected()
    {
        if (showDebugInfo)
        {
            Gizmos.color = debugGizmoColor;
            Gizmos.DrawWireSphere(transform.position, pickupDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, touchRadius);
        }
    }
    */
}