using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private NPC_Drop_Manager dropManager;
    [SerializeField] private string acceptedItemID;

    public enum SlotType
    {
        PatientLog,
        TreatCanvas
    }

    [SerializeField] private SlotType slotType;

    public void OnDrop(PointerEventData eventData)
    {
        var draggableItem = eventData.pointerDrag?.GetComponent<DraggableItem>();
        if (draggableItem == null) return;

        if (draggableItem.itemID == acceptedItemID)
        {
            draggableItem.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
            HandleCorrectDrop();
        }
        else
        {
            draggableItem.ResetPosition();
        }
    }

    private void HandleCorrectDrop()
    {
        switch (slotType)
        {
            case SlotType.PatientLog:
                dropManager.RegisterWords();
                break;
            case SlotType.TreatCanvas:
                dropManager.RegisterPills();
                break;
        }
    }
}