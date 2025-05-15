using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public NPC_Drop_Manager dropManager;

    public string acceptedItemID;

    public void OnDrop(PointerEventData eventData)
    {
        var draggableItem = eventData.pointerDrag?.GetComponent<DraggableItem>();
        if (draggableItem != null)
        {
            if (draggableItem.itemID == acceptedItemID)
            {
                draggableItem.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

                dropManager.RegisterDragAndDrop();
            }
            else
            {
                draggableItem.ResetPosition();
            }
        }
    }
}
