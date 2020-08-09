using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentPart equipSlot;
    public SkinnedMeshRenderer mesh;
    public int armorModifier;
    public int damageModifier;

    public override void Use()
    {
        base.Use();

        // Equip the item and Remove from inventory.  2020-05-11 11:30
        RemoveFromInventory();
        EquipmentManager.instance.Equip(this);
        // I changed the order of two codes for fix a bug where
        // old equipment is deleted when changing equipments.
    }
}

public enum EquipmentPart {Weapon, Armor, Booster, Detector}
