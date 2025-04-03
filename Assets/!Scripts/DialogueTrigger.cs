using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class DialogueTrigger : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private DialogueRunner _dialogueRunner;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private BBParameter<bool> _isDialogueActive; // NodeCanvas Blackboard var

    private void Start()
    {
        _dialogueRunner.onDialogueStart.AddListener(OnDialogueStart);
        _dialogueRunner.onDialogueComplete.AddListener(OnDialogueEnd);
    }

    private void OnDialogueStart()
    {
        _isDialogueActive.value = true;
        _agent.isStopped = true; // Stop moving
    }

    private void OnDialogueEnd()
    {
        _isDialogueActive.value = false;
        _agent.isStopped = false; // Resume wandering
    }

    private void OnDestroy()
    {
        // Clean up listeners
        _dialogueRunner.onDialogueStart.RemoveListener(OnDialogueStart);
        _dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueEnd);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _dialogueRunner.StartDialogue("LionDialogue");
        _dialogueRunner.onDialogueStart.AddListener(OnDialogueStart);
    }
}
