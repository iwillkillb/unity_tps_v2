using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    // Start is called before the first frame update
    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    // Update is called once per frame
    void OnEquipmentChanged(int slotIndex, Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier);
            Debug.Log("Plus  " + newItem.name + " -> armor : " + newItem.armorModifier + " / damage : " + newItem.damageModifier);
        }

        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);
            Debug.Log("Minus " + oldItem.name + " -> armor : " + oldItem.armorModifier + " / damage : " + oldItem.damageModifier);
        }
    }

    public override void Die()
    {
        base.Die();

        PlayerManager.instance.OnPlayerDied();

        //PlayerManager.instance.KillPlayer();
    }
}
