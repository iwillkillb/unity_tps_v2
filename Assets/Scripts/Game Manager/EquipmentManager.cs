using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singletone
    public static EquipmentManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Equipment Manager found.");
            return;
        }

        instance = this;
    }
    #endregion

    public delegate void OnEquipmentChanged(int slotIndex, Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    //public SkinnedMeshRenderer targetMesh;  // Equipments user(Player)'s modeling mesh.

    Equipment[] currentEquipment;   // Items i currently have equipped.
    //SkinnedMeshRenderer[] currentMeshes;

    Inventory inventory;



    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;

        // Initialize currentEquipment based on number of equipment slots.
        int numSlots = System.Enum.GetNames(typeof(EquipmentPart)).Length;
        currentEquipment = new Equipment[numSlots];
        //currentMeshes = new SkinnedMeshRenderer[numSlots];
    }



    // Used by Equipment.cs in Equipment item.
    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        Equipment oldItem = Unequip(slotIndex);

        // Insert the item into the slot.
        currentEquipment[slotIndex] = newItem;

        /*
        // Setting mesh
        if (newItem.mesh != null)
        {
            SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
            newMesh.transform.parent = targetMesh.transform;
            newMesh.bones = targetMesh.bones;
            newMesh.rootBone = targetMesh.rootBone;
            currentMeshes[slotIndex] = newMesh;
        }*/

        // Callback
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(slotIndex, newItem, null);
        }
    }



    public Equipment Unequip(int slotIndex)
    {
        // Is there empty slot in inventory?
        if (Inventory.instance.IsFull())
            return null;

        if (currentEquipment[slotIndex] != null)
        {
            /*
            // Mesh
            if (currentMeshes[slotIndex] != null)
            {
                Destroy(currentMeshes[slotIndex].gameObject);
            }*/

            // Save current equipment.
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            // Make empty slot.
            currentEquipment[slotIndex] = null;

            // Callback
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(slotIndex, null, oldItem);
            }

            return oldItem;
        }
        return null;
    }



    public void UnequipAll()
    {
        if (inventory.IsThereEnoughSpace(CountCurrentEquipment()))
        {
            for (int i = 0; i < currentEquipment.Length; i++)
            {
                Unequip(i);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }

    private int CountCurrentEquipment()
    {
        int countOfEquipments = 0;

        for (int i = 0; i < currentEquipment.Length; i++)
        {
            if (currentEquipment[i] != null)
            {
                countOfEquipments++;
            }
        }

        return countOfEquipments;
    }
}
