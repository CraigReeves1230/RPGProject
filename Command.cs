using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Command : ScriptableObject
{
    private string[] stringParameters;
    private float[] floatParameters;
    private int[] intParameters;
    private bool[] boolParameters;
    private PlayerMovement[] playerParameters;
    private string name;
    private GameObject gameObjectParameter;
    private EventWorker eventWorkerParam;
    private EventSequence eventSequenceParam;
    private DialogManager dialogManagerParam;
    private Scene sceneParam;
    private EventSequence.PromptCallback callbackParam;

    public void setName(string name)
    {
        name = name;
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

    public void setPlayerParams(params PlayerMovement[] parameters)
    {
        playerParameters = new PlayerMovement[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            playerParameters[i] = parameters[i];
        }
    }

    public PlayerMovement[] getPlayerParams()
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

    public void setDialogManagerParam(DialogManager dm)
    {
        dialogManagerParam = dm;
    }

    public DialogManager getDialogManagerParam()
    {
        return dialogManagerParam;
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

    public void setGameObjectParam(GameObject gameObject)
    {
        gameObjectParameter = gameObject;
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

    public GameObject getGameObject()
    {
        return gameObjectParameter;
    }
}
