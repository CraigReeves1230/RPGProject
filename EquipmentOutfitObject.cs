using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Outfit", menuName = "Inventory System/EquipmentOutfit")]
public class EquipmentOutfitObject : ScriptableObject
{
    public GameObject target;
    public string name;
    public List<OutfitSlot> container = new List<OutfitSlot>();
    
    public bool addItem(EquipmentObject item, string slotHandle, IEquippable target)
    {
        var ableToAdd = false;
        
        // make sure item isn't already equipped by someone else
        if (item.equippedBy == null)
        {
            foreach (var equipmentSlot in container)
            {
                if (slotHandle == equipmentSlot.handle)
                {
                    // make sure slot is compatible 
                    if (item.equipmentCategory.Contains(equipmentSlot.equipmentCategory))
                    {
                        // make sure item isn't equipped by someone else
                        if (item.equippedBy == null)
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
                                if (additionalAvailableSlots.Count > (item.numberOfSlotsFilled - 1))
                                ableToAdd = true;
                                
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
                                onEquip(item, target);
                                
                                item.equippedBy = target;
                            }
                        }
                    }
                }
            }
        }

        return ableToAdd;
    }

    void onEquip(EquipmentObject item, IEquippable target)
    {
        
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

