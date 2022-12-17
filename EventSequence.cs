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
    private EventWorker eventWorker;
    
    // get event collision box (if applicaable)
    private Collider2D eventCollisionBox;
    
    // get scene
    [SerializeField] private Scene scene;
    
    // get dialog manager
    private DialogManager dialogManager;
    
    // get dialog activator
    private DialogActivator dialogActivator;

    // determines if event is activated by hitting return or just stepping onto it
    [SerializeField]
    private bool activatedOnReturn = true;
    
    // determines if event is activated only by being triggered by another event
    [SerializeField] private bool activatedByEventOnly;
    
    // set to true once entire sequence is sent to the Event worker
    private bool sequenceSendComplete;
    
    // set to true if the event worker is busy processing events
    private bool workerBusy;    

    // for non-autotriggered events, this determines when the event is activated
    private bool eventActivated;

    // determines if player is making contact with event's collision box
    private bool withinEventZone;
        
    // determines if script runs only once or can be run more than once
    public bool runOnce;
    
    // determines if script loops. this automatically disables run once
    [SerializeField] 
    private bool loop;
    
    // determines if auto trigger is on, automatically disabling run once and activate on return
    [SerializeField]
    private bool autoTrigger;

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
        
        // Find dialog manager
        dialogManager = FindObjectOfType<DialogManager>();
        
        // Add dialog Activator
        dialogActivator = gameObject.AddComponent<DialogActivator>();

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
            activatedOnReturn = false;
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
            if (!autoTrigger && !activatedOnReturn && !workerBusy && !activatedByEventOnly)
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

        if (activatedOnReturn && !activatedByEventOnly && Input.GetButtonUp("Fire1"))
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

    //////////////////////////////////////////// CALLBACKS //////////////////////////////////////////////////////

    // facing commands

    protected void faceNorth(MovingEntity character)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("faceNorth");
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);  
    }
    
    protected void faceSouth(MovingEntity character)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("faceSouth");
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);  
    }
    
    protected void faceEast(MovingEntity character)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("faceEast");
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);  
    }
    
    protected void faceWest(MovingEntity character)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("faceWest");
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);  
    }
    
    // movement commands
    
    protected void walkEast(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkEast");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }

    protected void walkNW(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkNW");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkNE(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkNE");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkSE(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkSE");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkSW(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkSW");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }

    protected void stopAllFollowing()
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("stopAllFollowing");
        eventWorker.storeInQueue(command);
    }

    protected void followTheLeader()
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("followTheLeader");
        eventWorker.storeInQueue(command);
    }
    
    protected void runNW(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runNW");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runNE(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runNE");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runSE(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runSE");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runSW(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runSW");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkNorth(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkNorth");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkWest(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkWest");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkSouth(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkSouth");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
     }
    
    protected void runEast(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runEast");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runSouth(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runSouth");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runNorth(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runNorth");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runWest(MovingEntity character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runWest");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
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

    protected void giveItem(string itemName)
    {
        var command = newCom();
        command.setName("giveItem");
        command.setStringParams(itemName);
        eventWorker.storeInQueue(command);
    }
    
    protected void turnToFace(MovingEntity character, MovingEntity reference)
    {
        MovingEntity cm = character.GetComponent<MovingEntity>();
        var playerPos = new Vector2(reference.transform.position.x, reference.transform.position.y);
        var npcPos = new Vector2(character.transform.position.x, character.transform.position.y);
        var angle = Math.Abs(Math.Atan2(npcPos.y - playerPos.y, npcPos.x - playerPos.x) * 180f / Math.PI);
        
        if (angle >= 45f && angle <= 120f)
        {
            // character will either look north or south
            if (reference.transform.position.y > character.transform.position.y)
            {
                faceNorth(character);
            }
            else
            {
                faceSouth(character);
            }
        }
        else
        {
            // character will either look east or west
            if (playerPos.x > npcPos.x)
            {
                faceEast(character);
            }
            else
            {
                faceWest(character);
            }
        }
    }

    public void positionCharacter(MovingEntity character, float x, float y)
    {
        var command = newCom();
        command.setName("positionCharacter");
        command.setGameObjectParam(character.gameObject);
        command.setFloatParams(x, y);
        eventWorker.storeInQueue(command);
    }

    public void triggerAnimation(MovingEntity character, string trigger)
    {
        var command = newCom();
        command.setName("triggerAnimation");
        command.setGameObjectParam(character.gameObject);
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

    public void setScene(Scene scene)
    {
        this.scene = scene;
    }

    public Scene getScene()
    {
        return scene;
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
        command.setDialogActivatorParam(dialogActivator);
        command.setEventWorkerParam(eventWorker);
        eventWorker.storeInQueue(command);
    }

    // go to a different scene
    protected void goToScene(float x, float y, MovingEntity inPlayer, bool partOfSequence)
    {
        Command command = newCom();
        command.setName("goToScene");
        command.setSceneParam(scene);
        command.setFloatParams(x, y);
        command.setBoolParams(partOfSequence);
        command.setGameObjectParam(inPlayer.gameObject);
        eventWorker.storeInQueue(command);
    }

    protected void setNextWeather(bool rain, bool snow, bool fog, bool darkness)
    {      
        Command command = newCom();
        command.setName("setNextWeather");
        command.setBoolParams(rain, snow, fog, darkness);
        eventWorker.storeInQueue(command);
    }
    
    // hide and show characters
    
    protected void showCharacter(MovingEntity character)
    {
        Command command = newCom();
        command.setName("showCharacter");
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void hideCharacter(MovingEntity character)
    {
        Command command = newCom();
        command.setName("hideCharacter");
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    // camera commands
    protected void changeCamFollowTarget(GameObject entity)
    {
        Command command = newCom();
        command.setName("changeCameraFollowTarget");
        command.setGameObjectParam(entity);
        eventWorker.storeInQueue(command);
    }

    protected void changeCameraSpeed(float speed)
    {
        var command = newCom();
        command.setName("changeCameraSpeed");
        command.setFloatParams(speed);
        eventWorker.storeInQueue(command);
    }

    protected void setRain(bool setting, bool darkenScene)
    {
        Command command = newCom();
        command.setName("setRain");
        command.setBoolParams(setting, darkenScene);
        eventWorker.storeInQueue(command);
    }
    
    protected void setFog(bool setting)
    {
        Command command = newCom();
        command.setName("setFog");
        command.setBoolParams(setting);
        eventWorker.storeInQueue(command);
    }
    
    protected void setSnow(bool setting)
    {
        Command command = newCom();
        command.setName("setSnow");
        command.setBoolParams(setting);
        eventWorker.storeInQueue(command);
    }
}
