using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryStuff : MonoBehaviour
{
    public Transform holdPoint;

    public void PickUpItem(GameObject item) {
        item.transform.SetParent(gameObject.transform);
    }

    public void DropItem(GameObject item) {
        item.transform.SetParent(null);
    }
}
