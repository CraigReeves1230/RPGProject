using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Single Use Item", menuName = "Inventory System/Items/Single Use Item"), InlineEditor()]
public class SingleUseObject : ItemObject
{
    private ItemType type = ItemType.SingleUse;
    
    public int healthRecovery;
    public int magicRecovery;

    private void Awake()
    {
        inventoryLimit = 99;
    }
}
