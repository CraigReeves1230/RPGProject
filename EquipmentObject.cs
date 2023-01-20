using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory System/Items/Equipment"), InlineEditor()]
public class EquipmentObject : ItemObject
{
    private ItemType type = ItemType.Equipment;
    
    [PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
    public List<EquipmentCategory> equipmentCategory;
    
    [PropertySpace(SpaceAfter = 10, SpaceBefore = 0)]
    public List<EquipmentSubType> equipmentSubType;
    
    [Range(1, 99)]
    public int numberOfSlotsFilled;
    
    public int attackBonus;
    public int defenseBonus;
    public int speedCost;
    
    private void Awake()
    {
        inventoryLimit = 99;

        if (equipmentCategory.Count < 1)
        {
            equipmentCategory.Add(EquipmentCategory.RightHand);
        }

        GameManager.instance.gameDatabase.allItems.Add(this);
    }
}
