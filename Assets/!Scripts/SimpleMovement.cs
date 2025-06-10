using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Yarn;
using Yarn.Unity;

public class SimpleMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public Camera mainCam;
    public DialogueTrigger dialogueTrigger;
    public DialogueRunner dialogue;
    public CameraZoomScript cameraZoom;
    public Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
        agent.updateRotation = true;
    }
    private void Update()
    {
        if (dialogue.IsDialogueRunning)
        {
            return;
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
        Debug.Log("Agent Velocity: " + agent.velocity.magnitude);
    }
    public void SetCamera(Camera cam)
    {
        mainCam = cam;
    }
}