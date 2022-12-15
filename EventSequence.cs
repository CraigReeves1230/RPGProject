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
    [SerializeField] protected CharacterMovement[] npcs;
    
    // event worker instance
    private EventWorker eventWorker;
    
    // get event collision box (if applicaable)
    private Collider2D eventCollisionBox;
    
    // get scene
    [SerializeField] private Scene scene;
    
    // get dialog manager
    private DialogManager dialogManager;
    
    // get game world
    public GameWorld gameWorld;
    
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
    protected PlayerMovement player()
    {
        return gameWorld.partyLeader();
    }

    protected PlayerMovement player(string playerName)
    {
        // go through party and find correct player
        PlayerMovement foundPlayer = player();
        
        foreach (var player in gameWorld.party)
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
        
        // Find Game world if one wasn't assigned
        if (!gameWorld)
        {
            gameWorld = FindObjectOfType<GameWorld>();
        }
        
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
        if (other.gameObject.GetComponent<PlayerMovement>() == null) return;

        if (other.GetComponent<PlayerMovement>().isUnderPlayerControl())
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
        
        if (other.gameObject.GetComponent<PlayerMovement>() == null) return;
        
        if (other.GetComponent<CharacterMovement>().isUnderPlayerControl() && !autoTrigger)
        {
            withinEventZone = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() == null) return;
        
        if (other.GetComponent<CharacterMovement>().isUnderPlayerControl())
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

        if (activatedOnReturn && !activatedByEventOnly && Input.GetKeyUp(KeyCode.Return))
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

    protected void faceNorth(CharacterMovement character)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("faceNorth");
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);  
    }
    
    protected void faceSouth(CharacterMovement character)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("faceSouth");
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);  
    }
    
    protected void faceEast(CharacterMovement character)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("faceEast");
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);  
    }
    
    protected void faceWest(CharacterMovement character)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("faceWest");
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);  
    }
    
    // movement commands
    
    protected void walkEast(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkEast");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }

    protected void walkNW(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkNW");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkNE(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkNE");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkSE(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkSE");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkSW(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkSW");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runNW(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runNW");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runNE(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runNE");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runSE(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runSE");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runSW(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runSW");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkNorth(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkNorth");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkWest(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkWest");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void walkSouth(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("walkSouth");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
     }
    
    protected void runEast(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runEast");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runSouth(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runSouth");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runNorth(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runNorth");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void runWest(CharacterMovement character, float distance)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("runWest");
        command.setFloatParams(distance);
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }

    protected void stealControl(CharacterMovement character)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("stealControl");
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command); 
    }

    protected void returnControl(CharacterMovement character)
    {
        var command = ScriptableObject.CreateInstance<Command>();
        command.setName("returnControl");
        command.setGameObjectParam(character.gameObject);
        command.setEventSequenceParam(this);
        eventWorker.storeInQueue(command);
    }

    protected void promptWin(params string[] parameters)
    {
        var command = newCom();
        command.setName("promptWin");
        command.setStringParams(parameters);
        command.setDialogManagerParam(dialogManager);
        eventWorker.storeInQueue(command);
    }

    protected void giveItem(string itemName)
    {
        var command = newCom();
        command.setName("giveItem");
        command.setStringParams(itemName);
        eventWorker.storeInQueue(command);
    }
    
    // shortcut msg function
    protected void msgNext()
    {
        wait();
        msgClose();
    }

    protected void turnToFace(CharacterMovement character, CharacterMovement reference)
    {
        CharacterMovement cm = character.GetComponent<CharacterMovement>();
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

    public void positionCharacter(CharacterMovement character, float x, float y)
    {
        var command = newCom();
        command.setName("positionCharacter");
        command.setGameObjectParam(character.gameObject);
        command.setFloatParams(x, y);
        eventWorker.storeInQueue(command);
    }

    public void triggerAnimation(CharacterMovement character, string trigger)
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
        player().GetComponent<PlayerMovement>().clearDirectionalBuffer();
        
        foreach (var npc in npcs)
        {
            npc.GetComponent<NPCMovement>().clearDirectionalBuffer();
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
        player().GetComponent<PlayerMovement>().clearDirectionalBuffer();
        
        
        foreach (var npc in npcs)
        {
            npc.GetComponent<NPCMovement>().clearDirectionalBuffer();
        }
        
        eventWorker.cancelEventQueue();
    }
    
    protected void wait()
    {
        // clear directional buffer for all characters involved in script
        player().GetComponent<PlayerMovement>().clearDirectionalBuffer();
        
        foreach (var npc in npcs)
        {
            npc.GetComponent<NPCMovement>().clearDirectionalBuffer();
        }
        
        Command command = newCom();
        command.setName("wait");
        command.setEventWorkerParam(eventWorker);
        eventWorker.storeInQueue(command);
    }

    protected void waitForPrompt(PromptCallback callback)
    {
        // clear directional buffer for all characters
        player().GetComponent<PlayerMovement>().clearDirectionalBuffer();
        
        foreach (var npc in npcs)
        {
            npc.GetComponent<NPCMovement>().clearDirectionalBuffer();
        }

        Command command = newCom();
        command.setName("waitForPrompt");
        command.setCallbackParam(callback);
        command.setDialogManagerParam(dialogManager);
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

    protected void msg(string name, string message)
    {
        Command command = newCom();
        command.setName("msg");
        command.setStringParams(name, message);
        command.setDialogManagerParam(dialogManager);
        eventWorker.storeInQueue(command);
    }
    
    protected void msg(string name, string message, float height)
    {
        Command command = newCom();
        command.setName("msgWithHeight");
        command.setStringParams(name, message);
        command.setFloatParams(height);
        command.setDialogManagerParam(dialogManager);
        eventWorker.storeInQueue(command);
    }
    
    protected void picMsg(string name, string message, CharacterMovement character, int avatarIndex)
    {
        Command command = newCom();
        command.setName("picMsg");
        command.setStringParams(name, message);
        command.setDialogManagerParam(dialogManager);
        command.setGameObjectParam(character.gameObject);
        command.setIntParams(avatarIndex);
        eventWorker.storeInQueue(command);
    }
    
    protected void picMsg(string name, string message, float height, CharacterMovement character, int avatarIndex)
    {
        Command command = newCom();
        command.setName("picMsgWithHeight");
        command.setStringParams(name, message);
        command.setDialogManagerParam(dialogManager);
        command.setFloatParams(height);
        command.setGameObjectParam(character.gameObject);
        command.setIntParams(avatarIndex);
        eventWorker.storeInQueue(command);
    }

    protected void msgClose()
    {
        Command command = newCom();
        command.setName("msgClose");
        command.setDialogManagerParam(dialogManager);
        eventWorker.storeInQueue(command);
    }
    
    // go to a different scene
    protected void goToScene(float x, float y, CharacterMovement inPlayer, bool partOfSequence)
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
    
    protected void showCharacter(CharacterMovement character)
    {
        Command command = newCom();
        command.setName("showCharacter");
        command.setGameObjectParam(character.gameObject);
        eventWorker.storeInQueue(command);
    }
    
    protected void hideCharacter(CharacterMovement character)
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
