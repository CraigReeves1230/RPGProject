using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Command : ScriptableObject
{
    private string[] stringParameters;
    private float[] floatParameters;
    private int[] intParameters;
    private bool[] boolParameters;
    private MovingEntity[] playerParameters;
    private string name;
    private GameObject[] gameObjectParameters;
    private EventWorker eventWorkerParam;
    private EventSequence eventSequenceParam;
    private Scene sceneParam;
    private EventSequence.PromptCallback callbackParam;
    private InventoryObject[] inventoryObjectParams;

    public void setName(string inName)
    {
        name = inName;
    }

    public void setStringParams(params string[] parameters)
    {
        stringParameters = new string[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            stringParameters[i] = parameters[i];
        }
    }

    public void setBoolParams(params bool[] parameters)
    {
        boolParameters = new bool[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            boolParameters[i] = parameters[i];
        }
    }

    public void setInventoryObjectParams(params InventoryObject[] parameters)
    {
        inventoryObjectParams = new InventoryObject[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            inventoryObjectParams[i] = parameters[i];
        }
    }

    public void setPlayerParams(params MovingEntity[] parameters)
    {
        playerParameters = new MovingEntity[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            playerParameters[i] = parameters[i];
        }
    }

    public MovingEntity[] getPlayerParams()
    {
        return playerParameters;
    }

    public Scene getSceneParam()
    {
        return sceneParam;
    }

    public void setSceneParam(Scene scene)
    {
        sceneParam = scene;
    }

    public InventoryObject[] getInventoryObjectParams()
    {
        return inventoryObjectParams;
    }
    
    public EventSequence.PromptCallback getCallbackParam()
    {
        return callbackParam;
    }

    public void setCallbackParam(EventSequence.PromptCallback callback)
    {
        callbackParam = callback;
    }
    
    public void setFloatParams(params float[] parameters)
    {
        floatParameters = new float[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            floatParameters[i] = parameters[i];
        }
    }
    
    public void setIntParams(params int[] parameters)
    {
        intParameters = new int[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            intParameters[i] = parameters[i];
        }
    }

    public void setEventSequenceParam(EventSequence es)
    {
        eventSequenceParam = es;
    }

    public EventSequence getEventSequenceParam()
    {
        return eventSequenceParam;
    }

    public void setEventWorkerParam(EventWorker worker)
    {
        eventWorkerParam = worker;
    }

    public EventWorker getEventWorkerParameter()
    {
        return eventWorkerParam;
    }

    public void setGameObjectParams(params GameObject[] parameters)
    {
        gameObjectParameters = new GameObject[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            gameObjectParameters[i] = parameters[i];
        }
    }

    public string getName()
    {
        return name;
    }

    public string[] getStringParameters()
    {
        return stringParameters;
    }

    public int[] getIntParameters()
    {
        return intParameters;
    }

    public bool[] getBoolParameters()
    {
        return boolParameters;
    }

    public float[] getFloatParameters()
    {
        return floatParameters;
    }

    public GameObject[] getGameObjects()
    {
        return gameObjectParameters;
    }
}
