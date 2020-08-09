using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    protected Item item;
    public Image icon;
    public Button removeButton;

    public void AddItem(Item newItem)
    {
        // Draw item in inventory slot.
        item = newItem;
        icon.sprite = newItem.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        // Delete item in inventory slot.
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public Item GetItem()
    {
        return item;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }
}
