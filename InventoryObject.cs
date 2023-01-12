using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> container = new List<InventorySlot>();

    public void addItem(ItemObject _item, int _amount)
    {
        // 0 does nothing
        if (_amount == 0) return;

        // negative numbers do the opposite
        if (_amount < 0)
        {
            removeItem(_item, (_amount * -1));
            return;
        }
        
        // cap amount at inventory limit
        if (_amount > _item.inventoryLimit) _amount = _item.inventoryLimit;
                
        var hasItem = false;
        for (int i = 0; i < container.Count; i++)
        {
            if (container[i].item.Equals(_item))
            {
                // just simply add to amount but don't let exeed inventory limit
                var futureTotal = _amount + container[i].amount;
                if (futureTotal >= _item.inventoryLimit)
                {
                    container[i].amount = _item.inventoryLimit;
                }
                else
                {
                    container[i].amount = futureTotal;
                }
                
                hasItem = true;
                break;
            }
        }

        if (!hasItem)
        {
            // add new slot
            container.Add(new InventorySlot(_item, _amount));
        }
    }
    
    public void removeItem(ItemObject _item, int _amount)
    {
        // 0 does nothing
        if (_amount == 0) return;

        // negative numbers do the opposite
        if (_amount < 0)
        {
            addItem(_item, (_amount * -1));
            return;
        }
            
        for (int i = 0; i < container.Count; i++)
        {
            if (container[i].item.Equals(_item))
            {
                if ((container[i].amount - _amount) > 0)
                {
                    container[i].amount -= _amount;
                }
                else
                {
                    if (container[i].item.GetType() != typeof(CurrencyObject))
                    {
                        container.Remove(container[i]);
                    }
                    else
                    {
                        container[i].amount = 0;
                    }                  
                }

                break;
            }
        }
    }

    public ItemObject getItemByHandle(string handle)
    {
        ItemObject foundObject = null;
        foreach (var slot in container)
        {
            if (slot.item.handle == handle)
            {
                foundObject = slot.item;
            }
        }

        return foundObject;
    }

    public int amountInInventory(ItemObject item)
    {
        int amount = 0;
        foreach (var slot in container)
        {
            if (slot.item = item)
            {
                amount = slot.amount;
                break;
            }
        }

        return amount;
    }
    
    public int amountEquipped(ItemObject item)
    {
        int amount = 0;
        foreach (var slot in container)
        {
            if (slot.item = item)
            {
                amount = slot.equippedBy.Count;
                break;
            }
        }

        return amount;
    }
    
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;
    public List<EquipmentOutfitObject> equippedBy;
    
    // constructor
    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
        equippedBy = new List<EquipmentOutfitObject>();
    }

}
