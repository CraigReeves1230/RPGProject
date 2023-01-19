using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public abstract class EventSequence : MonoBehaviour
{    
    // Get NPC objects
    [SerializeField] protected MovingEntity[] npcs;
    
    // event worker instance
    protected EventWorker eventWorker;
    
    // get event collision box (if applicaable)
    protected Collider2D eventCollisionBox;

    // determines if event is activated by hitting fire or just stepping onto it
    [SerializeField]
    protected bool activatedOnButtonPress = true;
    
    // determines if event is activated only by being triggered by another event
    [SerializeField] protected bool activatedByEventOnly;
    
    // set to true once entire sequence is sent to the Event worker
    protected bool sequenceSendComplete;
    
    // set to true if the event worker is busy processing events
    protected bool workerBusy;    

    // for non-autotriggered events, this determines when the event is activated
    protected bool eventActivated;

    // determines if player is making contact with event's collision box
    protected bool withinEventZone;
        
    // determines if script runs only once or can be run more than once
    public bool runOnce;
    
    // determines if script loops. this automatically disables run once
    [SerializeField] 
    protected bool loop;
    
    // determines if auto trigger is on, automatically disabling run once and activate on return
    [SerializeField]
    protected bool autoTrigger;

    // function callback for prompt window
    public delegate void PromptCallback(int choice);
    
    // gets the player that is under our control
    protected ControllableEntity player()
    {
        return GameManager.instance.getControlTarget() == null ? GameManager.instance.partyLead() : GameManager.instance.getControlTarget();
    }

    protected ControllableEntity player(string playerName)
    {
        // go through and find correct player
        ControllableEntity foundPlayer = player();

        var allControllables = FindObjectsOfType<ControllableEntity>();
        
        foreach (var player in allControllables)
        {
            if (player.name != playerName) continue;

            foundPlayer = player;
        }

        return foundPlayer;
    }
    
    protected void Start()
    {
        // Initialize event worker
        eventWorker = gameObject.AddComponent<EventWorker>();

        // get trigger collision box of event
        var colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.isTrigger)
            {
                eventCollisionBox = collider;
            }
        }
        
        // make sure there aren't conflicts with settings
        if (autoTrigger)
        {
            runOnce = false;
            activatedOnButtonPress = false;
        }

        if (loop)
        {
            runOnce = false;
        }
    }
    
    // creates a new command instance
    Command newCom()
    {
        return ScriptableObject.CreateInstance<Command>();    
    }

    // handles sending commands to the event queue
    void handleEventLoop()
    {
        if (autoTrigger || eventActivated)
        {
            if (sequenceSendComplete) return;

            // still looping...
            if (loop && eventWorker.numberOfEventsInQueue() > 0) return;
            
            run();
        
            // once all commands have been completed, end sequence unless the event is to run on loop
            if (!loop)
            {
                sequenceSendComplete = true;
                eventActivated = false;
                withinEventZone = false;
            }
        }  
    }
    
    // handle collision with entities
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ControllableEntity>().CompareTag("Player"))
        {
            withinEventZone = true;

            // trigger event if activated on return is false
            if (!autoTrigger && !activatedOnButtonPress && !workerBusy && !activatedByEventOnly)
            {
                activateEvent();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<ControllableEntity>().CompareTag("Player") && !autoTrigger)
        {
            withinEventZone = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<ControllableEntity>().CompareTag("Player"))
        {
            withinEventZone = false; 
        }
    }

    void activateEvent()
    {
        if (!runOnce)
        {
            sequenceSendComplete = false;
        }
        
        eventActivated = true;   
    }

    void handleWithinZone()
    {
        if (!withinEventZone || workerBusy || autoTrigger) return;

        if (activatedOnButtonPress && !activatedByEventOnly && GameManager.instance.getMainFireKeyUp())
        {
            activateEvent();
        }
    }
    
    protected void Update()
    {   
        handleWithinZone();
        handleEventLoop();
        workerBusy = eventWorker.numberOfEventsInQueue() > 0;
    }

    // put event scripting here
    public abstract void run();
    
    public void updateWithinZone(GameObject player)
    {
        var playerCollisionBox = player.GetComponent<Collider2D>();
        if (playerCollisionBox.IsTouching(eventCollisionBox))
        {
            withinEventZone = true;
        }
        else
        {
            withinEventZone = false;
        }
    }
    
    //////////////////////////////////////////// GETTERS AND SETTERS ////////////////////////////////////////////

    public void setLoop(bool setting) => loop = setting;
    public bool getIsLoop() => loop;
    public bool getIsAutotrigger() => autoTrigger;
    public void setIsAutotrigger(bool setting) => autoTrigger = setting;

    //////////////////////////////////////////// CALLBACKS //////////////////////////////////////////////////////

    // facing commands

    protected void faceNorth(MovingEntity character)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("faceNorth");
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);  
    }
    
    protected void faceSouth(MovingEntity character)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("faceSouth");
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);  
    }
    
    protected void faceEast(MovingEntity character)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("faceEast");
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);  
    }
    
    protected void faceWest(MovingEntity character)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("faceWest");
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);  
    }
    
    // movement commands
    
    protected void walkEast(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkEast");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }

    protected void walkNW(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkNW");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkNE(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkNE");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkSE(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkSE");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkSW(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkSW");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }

    protected void stopAllFollowing()
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("stopAllFollowing");
        eventWorker.storeInQueue(command);
    }

    protected void followTheLeader(bool repositionPlayers = true)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("followTheLeader");
        command.setBoolParams(repositionPlayers);
        eventWorker.storeInQueue(command);
    }
    
    protected void runNW(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runNW");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runNE(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runNE");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runSE(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runSE");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runSW(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runSW");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkNorth(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkNorth");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkWest(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkWest");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkSouth(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkSouth");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
     }
    
    protected void runEast(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runEast");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runSouth(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runSouth");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runNorth(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runNorth");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runWest(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runWest");
        command.setFloatParams(distance);
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }

    protected void setExitsEnabled(bool setting)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("setExitsEnabled");
        command.setBoolParams(setting);
        eventWorker.storeInQueue(command);
    }

    protected void stealControl()
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("stealControl");
        eventWorker.storeInQueue(command); 
    }

    protected void returnControl()
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("returnControl");
        eventWorker.storeInQueue(command);
    }

    protected void promptWin(params string[] parameters)
    {
        var command = newCom();
        command.setName("promptWin");
        command.setStringParams(parameters);
        eventWorker.storeInQueue(command);
    }

    protected void giveItem(string name, int amount = 1, InventoryObject inventory = null)
    {
        var command = newCom();
        command.setName("giveItem");
        command.setStringParams(name);
        command.setIntParams(amount);
        command.setInventoryObjectParams(inventory);
        eventWorker.storeInQueue(command);
    }
    
    protected void takeItem(string name, int amount = 1, InventoryObject inventory = null)
    {
        var command = newCom();
        command.setName("takeItem");
        command.setStringParams(name);
        command.setIntParams(amount);
        command.setInventoryObjectParams(inventory);
        eventWorker.storeInQueue(command);
    }

    protected void equipItem(IEquippable target, string slotHandle, string itemHandle, InventoryObject inventory = null)
    {
        var command = newCom();
        command.setName("equipItem");
        command.setEquipTargetParam(target);
        command.setStringParams(slotHandle, itemHandle);
        command.setInventoryObjectParams(inventory);
        eventWorker.storeInQueue(command);
    }
    
    protected void unEquipItem(IEquippable target, string slotHandle, InventoryObject inventory = null)
    {
        var command = newCom();
        command.setName("unEquipItem");
        command.setEquipTargetParam(target);
        command.setStringParams(slotHandle);
        command.setInventoryObjectParams(inventory);
        eventWorker.storeInQueue(command);
    }

    public void addGameWorldInt(string name, int value)
    {
        var command = newCom();
        command.setName("addGameWorldInteger");
        command.setStringParams(name);
        command.setIntParams(value);
        eventWorker.storeInQueue(command);
    }
    
    public void addPlayerCustomInt(PlayerObject playerObject, string name, int value)
    {
        var command = newCom();
        command.setName("addPlayerCustomInteger");
        command.setPlayerObjectParam(playerObject);
        command.setStringParams(name);
        command.setIntParams(value);
        eventWorker.storeInQueue(command);
    }
    
    public int getGameWorldInt(string name)
    {
        return GameManager.instance.gameDatabase.GameWorldIntegerValue(name);
    }
    
    public int getPlayerCustomInt(PlayerObject playerObject, string name)
    {
        return playerObject.PlayerCustomIntegerValue(name);
    }
    
    public void setGameWorldInt(string name, int value)
    {
        var command = newCom();
        command.setName("setGameWorldInteger");
        command.setStringParams(name);
        command.setIntParams(value);
        eventWorker.storeInQueue(command);
    }
    
    public void setPlayerCustomInt(PlayerObject playerObject, string name, int value)
    {
        var command = newCom();
        command.setName("setPlayerCustomInteger");
        command.setPlayerObjectParam(playerObject);
        command.setStringParams(name);
        command.setIntParams(value);
        eventWorker.storeInQueue(command);
    }
    
    public void removeGameWorldInt(string name)
    {
        var command = newCom();
        command.setName("removeGameWorldInteger");
        command.setStringParams(name);
        eventWorker.storeInQueue(command);
    }
    
    public void removePlayerCustomInt(PlayerObject playerObject, string name)
    {
        var command = newCom();
        command.setPlayerObjectParam(playerObject);
        command.setName("removePlayerCustomInteger");
        command.setStringParams(name);
        eventWorker.storeInQueue(command);
    }
    
    public void addGameWorldString(string name, string value)
    {
        var command = newCom();
        command.setName("addGameWorldString");
        command.setStringParams(name, value);
        eventWorker.storeInQueue(command);
    }
    
    public void addPlayerCustomString(PlayerObject playerObject, string name, string value)
    {
        var command = newCom();
        command.setName("addPlayerCustomString");
        command.setPlayerObjectParam(playerObject);
        command.setStringParams(name, value);
        eventWorker.storeInQueue(command);
    }
    
    public string getGameWorldString(string name)
    {
        return GameManager.instance.gameDatabase.GameWorldStringValue(name);
    }
    
    public string getPlayerCustomString(PlayerObject playerObject, string name)
    {
        return playerObject.PlayerCustomStringValue(name);
    }
    
    public void setGameWorldString(string name, string value)
    {
        var command = newCom();
        command.setName("setGameWorldString");
        command.setStringParams(name, value);
        eventWorker.storeInQueue(command);
    }
    
    public void setPlayerCustomString(PlayerObject playerObject, string name, string value)
    {
        var command = newCom();
        command.setName("setPlayerCustomString");
        command.setPlayerObjectParam(playerObject);
        command.setStringParams(name, value);
        eventWorker.storeInQueue(command);
    }
    
    public void removeGameWorldString(string name)
    {
        var command = newCom();
        command.setName("removePlayerCustomString");
        command.setStringParams(name);
        eventWorker.storeInQueue(command);
    }
    
    public void removePlayerCustomString(PlayerObject playerObject, string name)
    {
        var command = newCom();
        command.setName("removePlayerCustomString");
        command.setPlayerObjectParam(playerObject);
        command.setStringParams(name);
        eventWorker.storeInQueue(command);
    }
    
    public void turnToFace(MovingEntity character, GameObject target)
    {
        var command = newCom();
        command.setName("turnToFace");
        command.setGameObjectParams(character.gameObject, target);
        eventWorker.storeInQueue(command);
    }

    public void positionCharacter(MovingEntity character, float x, float y)
    {
        var command = newCom();
        command.setName("positionCharacter");
        command.setGameObjectParams(character.gameObject);
        command.setFloatParams(x, y);
        eventWorker.storeInQueue(command);
    }

    public void triggerAnimation(MovingEntity character, string trigger)
    {
        var command = newCom();
        command.setName("triggerAnimation");
        command.setGameObjectParams(character.gameObject);
        command.setStringParams(trigger);
        eventWorker.storeInQueue(command);
    }

    
    // event flow commands. these are public so they can be controlled by other events
    
    public void delay(float time)
    {
        var command = newCom();
        command.setName("delay");
        command.setFloatParams(time);
        eventWorker.storeInQueue(command);
    }
    
    public void pause()
    {
        var command = newCom();
        command.setName("pause");
        command.setEventWorkerParam(eventWorker);
        eventWorker.storeInQueue(command);
    }

    public void pauseNow()
    {
        
        // clear directional buffer for all characters involved
        player().GetComponent<MovingEntity>().clearDirectionalBuffer();
        
        foreach (var npc in npcs)
        {
            npc.GetComponent<MovingEntity>().clearDirectionalBuffer();
        }
        
        eventWorker.pauseNow();
    }


    public void resumeSeq()
    {
        eventWorker.resume();
    }


    public void cancelSequence()
    {
        // clear directional buffer for all characters involved in script
        player().GetComponent<MovingEntity>().clearDirectionalBuffer();
        
        
        foreach (var npc in npcs)
        {
            npc.GetComponent<MovingEntity>().clearDirectionalBuffer();
        }
        
        eventWorker.cancelEventQueue();
    }
    
    protected void wait()
    {
        // clear directional buffer for all characters involved in script
        player().GetComponent<MovingEntity>().clearDirectionalBuffer();
        
        foreach (var npc in npcs)
        {
            npc.GetComponent<MovingEntity>().clearDirectionalBuffer();
        }
        
        Command command = newCom();
        command.setName("wait");
        command.setEventWorkerParam(eventWorker);
        eventWorker.storeInQueue(command);
    }

    protected void waitForPrompt(PromptCallback callback)
    {
        // clear directional buffer for all characters
        player().GetComponent<MovingEntity>().clearDirectionalBuffer();
        
        foreach (var npc in npcs)
        {
            npc.GetComponent<MovingEntity>().clearDirectionalBuffer();
        }

        Command command = newCom();
        command.setName("waitForPrompt");
        command.setCallbackParam(callback);
        eventWorker.storeInQueue(command);
    }
    
    // control the flow of other events
    
    protected void remoteResumeSeq(EventSequence es)
    {
        Command command = newCom();
        command.setName("remoteResumeSeq");
        command.setEventSequenceParam(es);
        eventWorker.storeInQueue(command);
    }
    
    protected void remotePause(EventSequence es)
    {
        Command command = newCom();
        command.setName("remotePause");
        command.setEventSequenceParam(es);
        eventWorker.storeInQueue(command);
    }

    protected void remoteCancelSeq(EventSequence es)
    {
        Command command = newCom();
        command.setName("remoteCancelSeq");
        command.setEventSequenceParam(es);
        eventWorker.storeInQueue(command);
    }

    protected void remoteRunSeq(EventSequence es)
    {
        Command command = newCom();
        command.setName("remoteRunSeq");
        command.setEventSequenceParam(es);
        eventWorker.storeInQueue(command);
    }
    
    
    // Message window events

    protected void msg(params string[] lines)
    {
        Command command = newCom();
        command.setName("msg");
        command.setStringParams(lines);
        eventWorker.storeInQueue(command);
    }
    
    protected void msgCls()
    {
        Command command = newCom();
        command.setName("msgCls");
        eventWorker.storeInQueue(command);
    }

    
    // fade
    
    protected void fadeOut()
    {
        Command command = newCom();
        command.setName("fadeOut");
        eventWorker.storeInQueue(command);
    }

    protected void fadeToBlack(float delayTime = 1f)
    {
        fadeOut();
        delay(delayTime);
    }
    
    
    protected void fadeIn()
    {
        Command command = newCom();
        command.setName("fadeIn");
        eventWorker.storeInQueue(command);
    }
    
    // go to a different scene
    protected void goToScene(string sceneName, bool partOfSequence, float x = 0, float y = 0)
    {
        Command command = newCom();
        command.setName("goToScene");
        command.setStringParams(sceneName);
        command.setFloatParams(x, y); 
        command.setBoolParams(partOfSequence);
        command.setEventSequenceParam(this);
        command.setEventWorkerParam(eventWorker);

        if (partOfSequence)
        {
            DontDestroyOnLoad(gameObject);
        }

        eventWorker.storeInQueue(command);
    }

    protected void fadeToScene(string sceneName, bool partOfSequence, float delayTime = 1f, float x = 0, float y = 0f)
    {
        fadeToBlack(delayTime);
        goToScene(sceneName, partOfSequence, x, y);
        fadeIn();
    }
    
    // hide and show characters
    
    protected void showCharacter(MovingEntity character)
    {
        Command command = newCom();
        command.setName("showCharacter");
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void hideCharacter(MovingEntity character)
    {
        Command command = newCom();
        command.setName("hideCharacter");
        command.setGameObjectParams(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    // camera commands
    protected void changeCamFollowTarget(GameObject entity)
    {
        Command command = newCom();
        command.setName("changeCameraFollowTarget");
        command.setGameObjectParams(entity);
        eventWorker.storeInQueue(command);
    }

    protected void changeCameraSpeed(float speed)
    {
        var command = newCom();
        command.setName("changeCameraSpeed");
        command.setFloatParams(speed);
        eventWorker.storeInQueue(command);
    }

    protected void setRain(bool setting, float rainTransitionSpeed = 10f, float rainIntensity = 550f)
    {
        Command command = newCom();
        command.setName("setRain");
        command.setBoolParams(setting);
        command.setFloatParams(rainTransitionSpeed, rainIntensity);
        eventWorker.storeInQueue(command);
    }

    protected void setDarkness(bool setting, float speed = 0.5f)
    {
        Command command = newCom();
        command.setName("setDarkness");
        command.setBoolParams(setting);
        command.setFloatParams(speed);
        eventWorker.storeInQueue(command);
    }
    
    protected void setFog(bool setting, float fogTransitionSpeed = 10f, float fogIntensity = 10f)
    {
        Command command = newCom();
        command.setName("setFog");
        command.setBoolParams(setting);   
        command.setFloatParams(fogTransitionSpeed, fogIntensity);
        eventWorker.storeInQueue(command);
    }
    
    protected void setSnow(bool setting, float snowTransitionSpeed = 10f, float snowIntensity = 300f)
    {
        Command command = newCom();
        command.setName("setSnow");
        command.setBoolParams(setting);   
        command.setFloatParams(snowTransitionSpeed, snowIntensity);
        eventWorker.storeInQueue(command);
    }
    
    protected void setSceneDefaultWeather(bool rain, bool fog, bool snow, bool darkness, string sceneName = null)
    {
        Command command = newCom();
        command.setName("setSceneDefaultWeather");
        command.setBoolParams(rain, fog, snow, darkness);   
        command.setStringParams(sceneName);
        eventWorker.storeInQueue(command);
    }
}
