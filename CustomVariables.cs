using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Custom Variables", menuName = "Custom Variables"), InlineEditor()]
public class CustomVariables : SerializedScriptableObject
{
    public List<CustomInteger> customIntegers;
    public List<CustomString> customStrings;
    public List<CustomFloat> customFloats;
    public List<CustomBoolean> customBooleans;
    
    public Dictionary<string, int> customIntegerIndices = new Dictionary<string, int>();
    public Dictionary<string, int> customStringIndices = new Dictionary<string, int>();
    public Dictionary<string, int> customFloatIndices = new Dictionary<string, int>();
    public Dictionary<string, int> customBooleanIndices = new Dictionary<string, int>();
    
    
    /*/////////////////////////////////////////// CUSTOM INTEGERS  ///////////////////////////////////////////*/
    
    public void addCustomInteger(string _name, int _value)
    {
        // if variable already exists, change existing value
        if (customIntegerIndices.ContainsKey(_name))
        {
            var i = customIntegerIndices[_name];
            customIntegers[i].value = _value;
            return;
        }
        
        var newVar = new CustomInteger(_name, _value);
        var currIndex = customIntegers.Count < 1 ? 0 : customIntegers.Count;
        customIntegers.Add(newVar);
        customIntegerIndices.Add(_name, currIndex);
    }
    
    public void removeCustomInteger(string _name)
    {
        var gameVariableIdx = customIntegerIndices[_name];
        customIntegers.RemoveAt(gameVariableIdx);
        customIntegerIndices.Remove(_name);
    }

    public int customIntegerValue(string in_name)
    {
        // if game variable doesn't exist, return 0
        
        if (!customIntegerIndices.ContainsKey(in_name)) return 0;
        
        var gameVariableIdx = customIntegerIndices[in_name];
        var theGameVar = customIntegers[gameVariableIdx];

        if (theGameVar == null)
        {
            return 0;
        }

        return theGameVar.value;
    }
    
    public void customIntegerValue(string _name, int _value)
    {
        // if game integer doesn't exist, create it
        if (!customIntegerIndices.ContainsKey(_name))
        {
            addCustomInteger(_name, _value);
            return;
        }
        
        var gameWorldVarIdx = customIntegerIndices[_name];

        customIntegers[gameWorldVarIdx].value = _value;
    }
    
    
    /*/////////////////////////////////////////// CUSTOM STRINGS  ///////////////////////////////////////////*/
    
    public void addCustomString(string _name, string _value)
    {
        // if variable already exists, change existing value
        if (customStringIndices.ContainsKey(_name))
        {
            var i = customStringIndices[_name];
            customStrings[i].value = _value;
            return;
        }
        
        var newVar = new CustomString(_name, _value);
        var currIndex = customStrings.Count < 1 ? 0 : customStrings.Count;
        customStrings.Add(newVar);
        customStringIndices.Add(_name, currIndex);
    }
    
    public void removeCustomString(string _name)
    {
        var gameVariableIdx = customStringIndices[_name];
        customStrings.RemoveAt(gameVariableIdx);
        customStringIndices.Remove(_name);
    }

    public string customStringValue(string _name)
    {
        // if game variable doesn't exist, return 0
        if (!customStringIndices.ContainsKey(_name)) return null;
        
        var gameVariableIdx = customStringIndices[_name];
        var theGameVar = customStrings[gameVariableIdx];

        if (theGameVar == null)
        {
            return null;
        }

        return theGameVar.value;
    }
    
    public void customStringValue(string _name, string _value)
    {
        // if game String doesn't exist, create it
        if (!customStringIndices.ContainsKey(_name))
        {
            addCustomString(_name, _value);
            return;
        }
        
        var gameWorldVarIdx = customStringIndices[_name];

        customStrings[gameWorldVarIdx].value = _value;
    }
    
    /*/////////////////////////////////////////// CUSTOM FLOATS  ///////////////////////////////////////////*/
    
    public void addCustomFloat(string _name, float _value)
    {
        // if variable already exists, change existing value
        if (customFloatIndices.ContainsKey(_name))
        {
            var i = customFloatIndices[_name];
            customFloats[i].value = _value;
            return;
        }
        
        var newVar = new CustomFloat(_name, _value);
        var currIndex = customFloats.Count < 1 ? 0 : customFloats.Count;
        customFloats.Add(newVar);
        customFloatIndices.Add(_name, currIndex);
    }
    
    public void removeCustomFloat(string _name)
    {
        var gameVariableIdx = customFloatIndices[_name];
        customFloats.RemoveAt(gameVariableIdx);
        customFloatIndices.Remove(_name);
    }

    public float customFloatValue(string _name)
    {
        // if game variable doesn't exist, return 0
        if (!customFloatIndices.ContainsKey(_name)) return 0;
        
        var gameVariableIdx = customFloatIndices[_name];
        var theGameVar = customFloats[gameVariableIdx];

        if (theGameVar == null)
        {
            return 0;
        }

        return theGameVar.value;
    }
    
    public void customFloatValue(string _name, float _value)
    {
        // if game Float doesn't exist, create it
        if (!customFloatIndices.ContainsKey(_name))
        {
            addCustomFloat(_name, _value);
            return;
        }
        
        var gameWorldVarIdx = customFloatIndices[_name];

        customFloats[gameWorldVarIdx].value = _value;
    }
    
    /*/////////////////////////////////////////// CUSTOM BOOLEANS ///////////////////////////////////////////*/
    
    public void addCustomBoolean(string _name, bool _value)
    {
        // if variable already exists, change existing value
        if (customBooleanIndices.ContainsKey(_name))
        {
            var i = customBooleanIndices[_name];
            customBooleans[i].value = _value;
            return;
        }
        
        var newVar = new CustomBoolean(_name, _value);
        var currIndex = customBooleans.Count < 1 ? 0 : customBooleans.Count;
        customBooleans.Add(newVar);
        customBooleanIndices.Add(_name, currIndex);
    }
    
    public void removeCustomBoolean(string _name)
    {
        var gameVariableIdx = customBooleanIndices[_name];
        customBooleans.RemoveAt(gameVariableIdx);
        customBooleanIndices.Remove(_name);
    }

    public bool customBooleanValue(string _name)
    {
        // if game variable doesn't exist, return 0
        if (!customBooleanIndices.ContainsKey(_name)) return false;
        
        var gameVariableIdx = customBooleanIndices[_name];
        var theGameVar = customBooleans[gameVariableIdx];

        if (theGameVar == null)
        {
            return false;
        }

        return theGameVar.value;
    }
    
    public void customBooleanValue(string _name, bool _value)
    {
        // if game Boolean doesn't exist, create it
        if (!customBooleanIndices.ContainsKey(_name))
        {
            addCustomBoolean(_name, _value);
            return;
        }
        
        var gameWorldVarIdx = customBooleanIndices[_name];

        customBooleans[gameWorldVarIdx].value = _value;
    }

    public static void hydrateDictionaries(CustomVariables cv)
    {        
        for (int i = 0; i < cv.customIntegers.Count; i++)
        {
            cv.customIntegerIndices[cv.customIntegers[i].name] = i;
        }
        
        for (int i = 0; i < cv.customStrings.Count; i++)
        {
            cv.customStringIndices[cv.customStrings[i].name] = i;
        }
        
        for (int i = 0; i < cv.customFloats.Count; i++)
        {
            cv.customFloatIndices[cv.customFloats[i].name] = i;
        }
        
        for (int i = 0; i < cv.customBooleans.Count; i++)
        {
            cv.customBooleanIndices[cv.customBooleans[i].name] = i;
        }
    }
}


[System.Serializable]
public class CustomInteger
{
    public string name;
    public int value;

    public CustomInteger(string _name, int _value)
    {
        name = _name;
        value = _value;
    }
}


[System.Serializable]
public class CustomString
{
    public string name;
    public string value;

    public CustomString(string _name, string _value)
    {
        name = _name;
        value = _value;
    }
}


[System.Serializable]
public class CustomFloat
{
    public string name;
    public float value;

    public CustomFloat(string _name, float _value)
    {
        name = _name;
        value = _value;
    }
}


[System.Serializable]
public class CustomBoolean
{
    public string name;
    public bool value;

    public CustomBoolean(string _name, bool _value)
    {
        name = _name;
        value = _value;
    }
}