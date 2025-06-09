using UnityEngine;

public class ProximityCanvas : MonoBehaviour
{
    public Canvas interactCanvas; // Assign in Inspector
    public float activationRadius = 3f; // Adjust radius as needed

    private SphereCollider proximityCollider;
    private GameObject player;

    void Start()
    {
        // Set up the proximity collider
        proximityCollider = gameObject.AddComponent<SphereCollider>();
        proximityCollider.radius = activationRadius;
        proximityCollider.isTrigger = true;

        // Find player by tag (make sure your player has the "Player" tag)
        player = GameObject.FindGameObjectWithTag("Player");

        // Ensure canvas is hidden at start
        if (interactCanvas != null)
        {
            interactCanvas.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            ToggleCanvas(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            ToggleCanvas(false);
        }
    }

    void ToggleCanvas(bool state)
    {
        if (interactCanvas != null)
        {
            interactCanvas.gameObject.SetActive(state);
        }
    }

    // Optional: Visualize the radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}