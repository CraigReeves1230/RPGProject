using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Currency", menuName = "Inventory System/Items/Currency")]
public class CurrencyObject : ItemObject
{
    private void Awake()
    {
        inventoryLimit = 99999;
    }
}
