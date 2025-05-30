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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (dialogue.IsDialogueRunning)
        {
            return;
        }
            
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
    public void SetCamera(Camera cam)
    {
        mainCam = cam;
    }
}