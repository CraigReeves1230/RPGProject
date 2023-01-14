using System.Collections;
using System.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Database", menuName = "Game Database")]
public class GameData : ScriptableObject
{
    public Sprite[] faces;
    public CurrencyObject mainCurrency;
    public ItemObject[] allItems;    
    public int maxLevel = 99;
    public InventoryObject defaultInventory;
    public List<gameSwitch> switches;
    
}

[System.Serializable]
public class gameSwitch
{
    public string name;
    public int value;

    // constructor
    gameSwitch(string _name, int _value)
    {
        name = _name;
        value = _value;
    }
}


