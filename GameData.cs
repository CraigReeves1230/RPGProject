using System.Collections;
using System.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Game Database", menuName = "Game Database")]
public class GameData : ScriptableObject
{    
    public Sprite[] faces;
    public CurrencyObject mainCurrency;
    public ItemObject[] allItems;    
    public int maxLevel = 99;
    public DialogManager.DisplayStyleOptions messageWindowStyle;
    public InventoryObject defaultInventory;
    public List<gameWorldVariable> gameWorldVariables;
    public Dictionary<string, int> gameWorldVariableIndices = new Dictionary<string, int>();

    
    public void addGameWorldVariable(string _name, int _value)
    {
        // if game variable already exists, change existing value
        if (gameWorldVariableIndices.ContainsKey(_name))
        {
            var i = gameWorldVariableIndices[_name];
            gameWorldVariables[i].value = _value;
            return;
        }
        
        var newVar = new gameWorldVariable(_name, _value);
        var currIndex = gameWorldVariables.Count < 1 ? 0 : gameWorldVariables.Count;
        Debug.Log("Current Index: " + currIndex);
        gameWorldVariables.Add(newVar);
        gameWorldVariableIndices.Add(_name, currIndex);
    }
    
    public void removeGameWorldVariable(string _name)
    {
        var gameVariableIdx = gameWorldVariableIndices[_name];
        gameWorldVariables.RemoveAt(gameVariableIdx);
        gameWorldVariableIndices.Remove(_name);
    }

    public int gameWorldVariableValue(string _name)
    {
        // if game variable doesn't exist, return 0
        if (!gameWorldVariableIndices.ContainsKey(_name)) return 0;
        
        var gameVariableIdx = gameWorldVariableIndices[_name];
        var theGameVar = gameWorldVariables[gameVariableIdx];

        if (theGameVar == null)
        {
            return 0;
        }

        return theGameVar.value;
    }
    
    public void gameWorldVariableValue(string _name, int _value)
    {
        // if variable doesn't exist, create it
        if (!gameWorldVariableIndices.ContainsKey(_name))
        {
            addGameWorldVariable(_name, _value);
            return;
        }
        
        var gameWorldVarIdx = gameWorldVariableIndices[_name];

        gameWorldVariables[gameWorldVarIdx].value = _value;
    }
    
}

[System.Serializable]
public class gameWorldVariable
{
    public string name;
    public int value;

    // constructor
    public gameWorldVariable(string _name, int _value)
    {
        name = _name;
        value = _value;
        
    }
}


