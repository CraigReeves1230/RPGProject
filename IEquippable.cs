using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquippable
{
    void equip(string slotHandle, string itemHandle, InventoryObject inventory = null, bool forRefresh = false);

    void unEquip(string slotHandle, InventoryObject inventory = null, bool forRefresh = false);
}
