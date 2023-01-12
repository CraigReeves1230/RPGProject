using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Outfit", menuName = "Inventory System/EquipmentOutfit")]
public class EquipmentOutfitObject : ScriptableObject
{
    public string name;
    public List<OutfitSlot> container = new List<OutfitSlot>();
    
    public bool addItem(EquipmentObject item, OutfitSlot slot, IEquippable equipTarget, InventoryObject inventory = null)
    {
        // the inventory is the party inventory by default
        if (inventory == null)
            inventory = GameManager.instance.partyInventory;
        
        var ableToAdd = false;
        
        InventorySlot invSlot = null;
        
        // make sure item exists
        if (item == null)
        {
            Debug.Log("ERROR: Item does not exist");
        }
        else
        {
            // find item in inventory
            foreach (var containerSlot in inventory.container)
            {
                if (containerSlot.item == item)
                {
                    invSlot = containerSlot;
                    break;
                }
            }
            
            // make sure item is in inventory
            if (invSlot == null)
            {
                Debug.Log("ERROR: Item does not exist in inventory.");
                return false;
            }
            
            // make sure item is free to be equipped. The amount equipped already must be lower than the amount in inventory.
            if (invSlot.amount > invSlot.equippedBy.Count)
            {
                foreach (var equipmentSlot in container)
                {
                    if (slot == equipmentSlot)
                    {                        
                        // make sure slot is compatible 
                        if (item.equipmentCategory.Contains(equipmentSlot.equipmentCategory))
                        {                            
                            // if item takes up more than one slot, fill then block it
                            if (item.numberOfSlotsFilled > 1)
                            {
                                // get available slots
                                var additionalAvailableSlots = new List<OutfitSlot>();
                               
                                foreach (var slotToBlock in container)
                                {
                                    // we don't want to add the slot we're equipping on
                                    if (slotToBlock == equipmentSlot) continue;
                                    
                                    // find a free slot that is compatible
                                    if (item.equipmentCategory.Contains(slotToBlock.equipmentCategory) &&
                                        slotToBlock.item == null)
                                    {
                                        additionalAvailableSlots.Add(slotToBlock);
                                    }
                                }
    
                                // if there aren't enough additional slots, do not equip item at all
                                if ((additionalAvailableSlots.Count + 1) < item.numberOfSlotsFilled) 
                                {
                                    Debug.Log("ERROR: Not enough compatible free slots for this item in outfit.");
                                }
                                else
                                {
                                    ableToAdd = true;
                                }
                                         
                                // fill and block additional slots
                                if (ableToAdd)
                                {
                                    foreach (var slotToBlock in container)
                                    {
                                        // we don't want to fill or block the slot we're equipping on
                                        if (slotToBlock == equipmentSlot) continue;
                                    
                                        // find a free slot that is compatible
                                        if (item.equipmentCategory.Contains(slotToBlock.equipmentCategory) &&
                                            slotToBlock.item == null)
                                        {
                                            slotToBlock.item = item;
                                            slotToBlock.isBlocked = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ableToAdd = true;
                            }  
                            
                            // equip item
                            if (ableToAdd)
                            {
                                equipmentSlot.item = item;
                                
                                // run onEquip
                                onEquip(item, equipTarget);
                                                               
                                // add to equipped
                                invSlot.equippedBy.Add(this);                                
                            }
                        }
                        else
                        {
                            Debug.Log("ERROR: Slot is incompatible with this item.");
                        }
                    }
                }
            }
            else
            {
                Debug.Log("ERROR: Item is already equipped by someone else.");
            }
        }
        return ableToAdd;
    }

    void onEquip(EquipmentObject item, IEquippable equipTarget)
    {
        
    }

    public OutfitSlot getSlotByHandle(string slotHandle)
    {
        OutfitSlot foundSlot = null;
        foreach (var slot in container)
        {
            if (slot.handle == slotHandle)
            {
                foundSlot = slot;
            }
        }

        return foundSlot;
    }
}

[System.Serializable]
public class OutfitSlot
{
    public string handle;
    public EquipmentCategory equipmentCategory;
    public EquipmentObject item;
    public bool isBlocked;
}

