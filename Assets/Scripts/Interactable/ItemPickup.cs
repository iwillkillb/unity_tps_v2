using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    protected override void Interact()
    {
        base.Interact();

        Debug.Log("Picking up " + item.name);
        bool wasPickup = Inventory.instance.Add(item);
        if (wasPickup)
        {
            Destroy(gameObject);
        }
    }
}
