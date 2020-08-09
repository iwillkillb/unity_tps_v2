using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This code manages inventory data.

public class Inventory : MonoBehaviour
{
    #region Singletone
    public static Inventory instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found.");
            return;
        }

        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallBack;

    public int space = 10;

    public List<Item> items = new List<Item>();

    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enough inventory.");
                return false;
            }
            items.Add(item);

            if (onItemChangedCallBack != null)
            {
                onItemChangedCallBack.Invoke();
            }
        }
        return true;
    }

    public bool IsFull()
    {
        return (items.Count >= space) ? true : false;
    }

    public bool IsThereEnoughSpace(int required)
    {
        return (items.Count + required <= space) ? true : false;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallBack != null)
        {
            onItemChangedCallBack.Invoke();
        }
    }
}
