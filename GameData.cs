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
    public List<GameWorldString> GameWorldStrings;
    public Dictionary<string, int> GameWorldIntegerIndices = new Dictionary<string, int>();
    public Dictionary<string, int> GameWorldStringIndices = new Dictionary<string, int>();
    
    
    // add, get and set game world variables
    
    /*/////////////////////////////////////////// GAME WORLD INTEGERS  ///////////////////////////////////////////*/
    
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
        // if game integer doesn't exist, create it
        if (!GameWorldIntegerIndices.ContainsKey(_name))
        {
            addGameWorldInteger(_name, _value);
            return;
        }
        
        var gameWorldVarIdx = GameWorldIntegerIndices[_name];

        GameWorldIntegers[gameWorldVarIdx].value = _value;
    }
    
  
    
    /*/////////////////////////////////////////// GAME WORLD STRINGS  ///////////////////////////////////////////*/
    
    public void addGameWorldString(string _name, string _value)
    {
        // if game string already exists, change existing value
        if (GameWorldStringIndices.ContainsKey(_name))
        {
            var i = GameWorldStringIndices[_name];
            GameWorldStrings[i].value = _value;
            return;
        }
        
        var newVar = new GameWorldString(_name, _value);
        var currIndex = GameWorldStrings.Count < 1 ? 0 : GameWorldStrings.Count;
        GameWorldStrings.Add(newVar);
        GameWorldStringIndices.Add(_name, currIndex);
    }
    
    public void removeGameWorldString(string _name)
    {
        var gameVariableIdx = GameWorldStringIndices[_name];
        GameWorldStrings.RemoveAt(gameVariableIdx);
        GameWorldStringIndices.Remove(_name);
    }
    
    public string GameWorldStringValue(string _name)
    {
        // if game variable doesn't exist, return 0
        if (!GameWorldStringIndices.ContainsKey(_name)) return null;
        
        var gameVariableIdx = GameWorldStringIndices[_name];
        var theGameVar = GameWorldStrings[gameVariableIdx];

        
        return theGameVar.value;
    }
    
    public void GameWorldStringValue(string _name, string _value)
    {
        // if game integer doesn't exist, create it
        if (!GameWorldStringIndices.ContainsKey(_name))
        {
            addGameWorldString(_name, _value);
            return;
        }
        
        var gameWorldVarIdx = GameWorldStringIndices[_name];

        GameWorldStrings[gameWorldVarIdx].value = _value;
    }
    
    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    
    
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

[System.Serializable]
public class GameWorldString
{
    public string name;
    public string value;

    // constructor
    public GameWorldString(string _name, string _value)
    {
        name = _name;
        value = _value;
        
    }
}


