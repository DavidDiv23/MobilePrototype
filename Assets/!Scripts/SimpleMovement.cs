using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class SimpleMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public Camera mainCam;
    public DialogueTrigger dialogueTrigger;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if(dialogueTrigger.hasStartedDialogue)
            return;
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