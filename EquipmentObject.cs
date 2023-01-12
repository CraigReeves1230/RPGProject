using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    private ItemType type = ItemType.Equipment;
    public List<EquipmentCategory> equipmentCategory;
    public List<EquipmentSubType> equipmentSubType;
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

        numberOfSlotsFilled = 1;
    }
}
