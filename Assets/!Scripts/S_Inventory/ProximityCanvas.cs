using UnityEngine;

public class ProximityCanvas : MonoBehaviour
{
    public Canvas interactCanvas; // Assign in Inspector
    public float activationRadius = 3f;

    private GameObject player;
    private bool playerInRange;

    void Start()
    {
        // Find player by tag
        player = GameObject.FindGameObjectWithTag("Player");

        // Ensure canvas is hidden at start
        if (interactCanvas != null)
        {
            interactCanvas.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Interact Canvas not assigned in ProximityCanvas!", this);
        }
    }

    void Update()
    {
        if (player == null) return;

        // Calculate distance to player
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Toggle canvas based on distance
        if (distance <= activationRadius)
        {
            if (!playerInRange)
            {
                playerInRange = true;
                ToggleCanvas(true);
            }
        }
        else
        {
            if (playerInRange)
            {
                playerInRange = false;
                ToggleCanvas(false);
            }
        }
    }

    void ToggleCanvas(bool state)
    {
        if (interactCanvas != null)
        {
            interactCanvas.gameObject.SetActive(state);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}