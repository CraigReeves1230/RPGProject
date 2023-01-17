using System.Collections;
using System.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Game Database", menuName = "Game Database")]
public class GameData : ScriptableObject
{    
    [Header("Message Window")]
    public DialogManager.DisplayStyleOptions messageWindowStyle;
    public Sprite[] faces;
    
    [Header("Items, Inventory and Currency")]
    public CurrencyObject mainCurrency;
    public InventoryObject defaultInventory;
    public List<ItemObject> allItems;   
    
    [Header("Game World")]
    public int maxLevel = 99;
    public List<GameWorldInteger> GameWorldIntegers;
    public Dictionary<string, int> GameWorldIntegerIndices = new Dictionary<string, int>();
    
    
    public void addGameWorldInteger(string _name, int _value)
    {
        // if game variable already exists, change existing value
        if (GameWorldIntegerIndices.ContainsKey(_name))
        {
            var i = GameWorldIntegerIndices[_name];
            GameWorldIntegers[i].value = _value;
            return;
        }
        
        var newVar = new GameWorldInteger(_name, _value);
        var currIndex = GameWorldIntegers.Count < 1 ? 0 : GameWorldIntegers.Count;
        Debug.Log("Current Index: " + currIndex);
        GameWorldIntegers.Add(newVar);
        GameWorldIntegerIndices.Add(_name, currIndex);
    }
    
    public void removeGameWorldInteger(string _name)
    {
        var gameVariableIdx = GameWorldIntegerIndices[_name];
        GameWorldIntegers.RemoveAt(gameVariableIdx);
        GameWorldIntegerIndices.Remove(_name);
    }

    public int GameWorldIntegerValue(string _name)
    {
        // if game variable doesn't exist, return 0
        if (!GameWorldIntegerIndices.ContainsKey(_name)) return 0;
        
        var gameVariableIdx = GameWorldIntegerIndices[_name];
        var theGameVar = GameWorldIntegers[gameVariableIdx];

        if (theGameVar == null)
        {
            return 0;
        }

        return theGameVar.value;
    }
    
    public void GameWorldIntegerValue(string _name, int _value)
    {
        // if variable doesn't exist, create it
        if (!GameWorldIntegerIndices.ContainsKey(_name))
        {
            addGameWorldInteger(_name, _value);
            return;
        }
        
        var gameWorldVarIdx = GameWorldIntegerIndices[_name];

        GameWorldIntegers[gameWorldVarIdx].value = _value;
    }
    
}

[System.Serializable]
public class GameWorldInteger
{
    public string name;
    public int value;

    // constructor
    public GameWorldInteger(string _name, int _value)
    {
        name = _name;
        value = _value;
        
    }
}


