using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquippable
{
    void equip(string slotHandle, string itemHandle, InventoryObject inventory = null);

    void unEquip(string slotHandle);

    void setAttribute(string attribute, int setting);
    int getAttribute(string attribute);
}
