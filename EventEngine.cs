using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventEngine : MonoBehaviour
{
    private GameObject[] gameObjectParams;
    private float[] floatParams;
    private string[] stringParams;
    private string name;
    private bool isDone;
    private Vector2 characterDestination;
    private bool autoMoving;
    private float endTime;
    private CamController camera;
    private bool promptRunning;
    private int currentPick = 1;    // for prompt window
    private Weather weather;
    
    public bool start(Command command)
    {        
        var commandName = command.getName();
        weather = FindObjectOfType<Weather>();

        camera = FindObjectOfType<CamController>();

        if (commandName == "walkEast")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var dist = command.getFloatParameters()[0];
            return autoMove(character, "east", dist, false);
        }
        
        if (commandName == "walkWest")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var dist = command.getFloatParameters()[0];
            return autoMove(character, "west", dist, false);
        }

        if (commandName == "walkSouth")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var dist = command.getFloatParameters()[0];
            return autoMove(character, "south", dist, false);
        }
        
        if (commandName == "walkNorth")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var dist = command.getFloatParameters()[0];
            return autoMove(character, "north", dist, false);
        }

        if (commandName == "returnControl")
        {
            return returnControl();
        }

        if (commandName == "stealControl")
        {
            return stealControl();
        }

        if (commandName == "stopAllFollowing")
        {
            return stopAllFollowing();
        }

        if (commandName == "followTheLeader")
        {
            var repositionPlayers = command.getBoolParameters()[0];
            return followTheLeader(repositionPlayers);
        }
        
        if (commandName == "faceNorth")
        {
            var anim = command.getGameObjects()[0].GetComponent<Animator>();
            return faceNorth(anim);
        }
        
        if (commandName == "faceWest")
        {
            var anim = command.getGameObjects()[0].GetComponent<Animator>();
            return faceWest(anim);
        }
        
        if (commandName == "faceSouth")
        {
            var anim = command.getGameObjects()[0].GetComponent<Animator>();
            return faceSouth(anim);
        }
        
        if (commandName == "faceEast")
        {
            var anim = command.getGameObjects()[0].GetComponent<Animator>();
            return faceEast(anim);
        }

        if (commandName == "delay")
        {
            var time = command.getFloatParameters()[0];
            return delay(time);
        }

        if (commandName == "pause")
        {
            var eventWorker = command.getEventWorkerParameter();
            return pause(eventWorker);
        }

        if (commandName == "remoteResumeSeq")
        {
            var eventSequence = command.getEventSequenceParam();
            return remoteResumeSeq(eventSequence);
        }

        if (commandName == "remotePause")
        {
            var eventSequence = command.getEventSequenceParam();
            return remotePause(eventSequence);
        }
        
        if (commandName == "remoteCancelSeq")
        {
            var eventSequence = command.getEventSequenceParam();
            return remoteCancelSeq(eventSequence);
        }
        
        if (commandName == "remoteRunSeq")
        {
            var eventSequence = command.getEventSequenceParam();
            return remoteRunSeq(eventSequence);
        }

        if (commandName == "addGameWorldVariable")
        {
            var _name = command.getStringParameters()[0];
            var _value = command.getIntParameters()[0];
            return addGameWorldVariable(_name, _value);
        }
        
        if (commandName == "removeGameWorldVariable")
        {
            var _name = command.getStringParameters()[0];
            return removeGameWorldVariable(_name);
        }
        
        if (commandName == "setGameWorldVariable")
        {
            var _name = command.getStringParameters()[0];
            var _value = command.getIntParameters()[0];
            return setGameWorldVariable(_name, _value);
        }
        
        if (commandName == "msg")
        {   
            var lines = command.getStringParameters();
            return msg(lines);
        }
        
        if (commandName == "msgCls")
        {   
            return msgCls();
        }

        if (commandName == "turnToFace")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var target = command.getGameObjects()[1];
            return turnToFace(character, target);
        }
        

        if (commandName == "promptWin")
        {
            var headerText = command.getStringParameters()[0];
            
            if (command.getStringParameters().Length == 2)
            {
                return promptWin(headerText, command.getStringParameters()[1]);
            }
            
            if (command.getStringParameters().Length == 3)
            {
                return promptWin(headerText, command.getStringParameters()[1], command.getStringParameters()[2]);
            }
            
            if (command.getStringParameters().Length == 4)
            {
                return promptWin(headerText, command.getStringParameters()[1], command.getStringParameters()[2], command.getStringParameters()[3]);
            }
            
            if (command.getStringParameters().Length == 5)
            {
                return promptWin(headerText, command.getStringParameters()[1], command.getStringParameters()[2], command.getStringParameters()[3], command.getStringParameters()[4]);
            }
        }

        if (commandName == "waitForPrompt")
        {
            var callback = command.getCallbackParam();
            return waitForPrompt(callback);
        }
        

        if (commandName == "wait")
        {
            var eventWorker = command.getEventWorkerParameter();
            return wait(eventWorker);
        }

        if (commandName == "goToScene")
        {
            var sceneName = command.getStringParameters()[0];
            var x = command.getFloatParameters()[0];
            var y = command.getFloatParameters()[1];
            var partOfSequence = command.getBoolParameters()[0];
            var ew = command.getEventWorkerParameter();
            var es = command.getEventSequenceParam();
            return goToScene(sceneName, x, y, partOfSequence, ew, es);
        }

        if (commandName == "runEast")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var dist = command.getFloatParameters()[0];
            return autoMove(character, "east", dist, true);
        }
        
        if (commandName == "runSouth")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var dist = command.getFloatParameters()[0];
            return autoMove(character, "south", dist, true);
        }
        
        if (commandName == "runWest")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var dist = command.getFloatParameters()[0];
            return autoMove(character, "west", dist, true);
        }
        
        if (commandName == "runNorth")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var dist = command.getFloatParameters()[0];
            return autoMove(character, "north", dist, true);
        }

        if (commandName == "showCharacter")
        {
            var sprite = command.getGameObjects()[0].GetComponent<SpriteRenderer>();
            return showOrHideCharacter(sprite, true);
        }
        
        if (commandName == "hideCharacter")
        {
            var sprite = command.getGameObjects()[0].GetComponent<SpriteRenderer>();
            return showOrHideCharacter(sprite, false);
        }

        if (commandName == "positionCharacter")
        {
            var x = command.getFloatParameters()[0];
            var y = command.getFloatParameters()[1];
            var character = command.getGameObjects()[0];

            return positionCharacter(character, x, y);
        }

        if (commandName == "triggerAnimation")
        {
            var trigger = command.getStringParameters()[0];
            var anim = command.getGameObjects()[0].GetComponent<Animator>();
            return triggerAnimation(anim, trigger);
        }

        if (commandName == "changeCameraFollowTarget")
        {
            var newTarget = command.getGameObjects()[0];
            return changeCameraFollowTarget(newTarget);
        }
        
        /*if (commandName == "changeCameraSpeed")
        {
            var newSpeed = command.getFloatParameters()[0];
            return changeCameraSpeed(newSpeed);
        }*/

        if (commandName == "fadeOut")
        {
            return fadeOut();
        }
        
        if (commandName == "fadeIn")
        { 
            return fadeIn();
        }
        
        if (commandName == "walkNW")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var distance = command.getFloatParameters()[0];
            return autoMove(character, "NW", distance, false);
        }
        
        if (commandName == "walkNE")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var distance = command.getFloatParameters()[0];
            return autoMove(character, "NE", distance, false);
        }
        
        if (commandName == "walkSE")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var distance = command.getFloatParameters()[0];
            return autoMove(character, "SE", distance, false);
        }
        
        if (commandName == "walkSW")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var distance = command.getFloatParameters()[0];
            return autoMove(character, "SW", distance, false);
        }
        
        if (commandName == "runNW")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var distance = command.getFloatParameters()[0];
            return autoMove(character, "NW", distance, true);
        }
        
        if (commandName == "runNE")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var distance = command.getFloatParameters()[0];
            return autoMove(character, "NE", distance, true);
        }
        
        if (commandName == "runSE")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var distance = command.getFloatParameters()[0];
            return autoMove(character, "SE", distance, true);
        }
        
        if (commandName == "runSW")
        {
            var character = command.getGameObjects()[0].GetComponent<MovingEntity>();
            var distance = command.getFloatParameters()[0];
            return autoMove(character, "SW", distance, true);
        }

        if (commandName == "setExitsEnabled")
        {
            var setting = command.getBoolParameters()[0];
            return setExitsEnabled(setting);
        }

        if (commandName == "setRain")
        {
            var setting = command.getBoolParameters()[0];
            var rainTransitionSpeed = command.getFloatParameters()[0];
            var rainIntensity = command.getFloatParameters()[1];
            return setRain(setting, rainTransitionSpeed, rainIntensity);
        }
        
        if (commandName == "setSnow")
        {
            var setting = command.getBoolParameters()[0];
            var snowTransitionSpeed = command.getFloatParameters()[0];
            var snowIntensity = command.getFloatParameters()[1];
            return setSnow(setting, snowTransitionSpeed, snowIntensity);
        }
        
        if (commandName == "setFog")
        {
            var setting = command.getBoolParameters()[0];
            var fogTransitionSpeed = command.getFloatParameters()[0];
            var fogIntensity = command.getFloatParameters()[1];
            return setFog(setting, fogTransitionSpeed, fogIntensity);
        }

        if (commandName == "equipItem")
        {
            var equipTarget = command.getEquipTargetParam();
            var slotHandle = command.getStringParameters()[0];
            var itemHandle = command.getStringParameters()[1];
            var inventory = command.getInventoryObjectParams()[0];
            return equipItem(equipTarget, slotHandle, itemHandle, inventory);
        }
        
        if (commandName == "unEquipItem")
        {
            var equipTarget = command.getEquipTargetParam();
            var slotHandle = command.getStringParameters()[0];
            var inventory = command.getInventoryObjectParams()[0];
            return unEquipItem(equipTarget, slotHandle, inventory);
        }
        
        if (commandName == "setDarkness")
        {
            var setting = command.getBoolParameters()[0];
            var speed = command.getFloatParameters()[0];
            return setDarkness(setting, speed);
        }

        if (commandName == "setSceneDefaultWeather")
        {
            var rain = command.getBoolParameters()[0];
            var fog = command.getBoolParameters()[1];
            var snow = command.getBoolParameters()[2];
            var darkness = command.getBoolParameters()[3];
            var sceneName = command.getStringParameters()[0];
            return setSceneDefaultWeather(rain, fog, snow, darkness, sceneName);
        }

        if (commandName == "giveItem")
        {
            var itemName = command.getStringParameters()[0];
            var amountToGive = command.getIntParameters()[0];
            var inventoryObj = command.getInventoryObjectParams()[0];
            return giveItem(itemName, amountToGive, inventoryObj);
        }
        
        if (commandName == "takeItem")
        {
            
            var itemName = command.getStringParameters()[0];
            var amountToRemove = command.getIntParameters()[0];
            var inventoryObj = command.getInventoryObjectParams()[0];
            return takeItem(itemName, amountToRemove, inventoryObj);
        }

        return true;
    }
    
    /* ----------------------------------------------------------------------------------------------------------*/

    private bool setRain(bool setting, float rainTransitionSpeed = 10f, float rainIntensity = 550f)
    {
        weather.setRain(setting, rainTransitionSpeed, rainIntensity);
        return true;
    }

    
    private bool setSnow(bool setting, float snowTransitionSpeed = 10f, float snowIntensity = 300f)
    {
        weather.setSnow(setting, snowTransitionSpeed, snowIntensity);
        return true;
    }
    
    private bool setFog(bool setting, float fogTransitionSpeed = 10f, float fogIntensity = 10f)
    {
        weather.setFog(setting, fogTransitionSpeed, fogIntensity);
        return true;
    }

    private bool setSceneDefaultWeather(bool rain, bool fog, bool snow, bool darkness, string sceneName = null)
    {
        var currentScene = sceneName ?? Weather.instance.sceneName;
        GameManager.instance.weatherOverrides[currentScene] = new[] {rain, fog, snow, darkness};
        return true;
    }

    private bool setDarkness(bool setting, float speed = 0.5f)
    {
        weather.setDarkness(setting, speed);
        return true;
    }
    
    private bool stealControl()
    {
        // shut off follow for all players
        GameManager.instance.revokeControl();
        return true;
    }

    private bool returnControl()
    {
        GameManager.instance.restoreControl();
        return true;
    }

    private bool fadeOut()
    {
        UIFade.instance.FadeToBlack();
        return true;
    }
    
    private bool fadeIn()
    {
        UIFade.instance.FadeFromBlack();
        return true;
    }

    private bool turnToFace(MovingEntity character, GameObject target)
    {
        character.turnToFace(target);

        return true;
    }
    
    private bool autoMove(MovingEntity entity, string direction, float distance, bool running)
    {
        entity.setIsRunning(running);
        var destinationReached = false;
        
        // get player position
        var pos = entity.transform.position;
        
        if (direction == "east")
        {
            if (!autoMoving)
            {
                characterDestination = new Vector2(pos.x + distance, pos.y);
                autoMoving = true;
            }
            
            // determine if destination has been reached
            destinationReached = pos.x >= characterDestination.x;

            if (!destinationReached)
            {
                entity.setHorizontalMov(1f);
            }
            else
            {
                entity.setHorizontalMov(0f);
                entity.setIsRunning(false);
                autoMoving = false;
            }
        }
        
        if (direction == "north")
        {
            if (!autoMoving)
            {
                characterDestination = new Vector2(pos.x, pos.y + distance);
                autoMoving = true;
            }
            
            // determine if destination has been reached
            destinationReached = pos.y >= characterDestination.y;

            if (!destinationReached)
            {
                entity.setVerticalMov(1f);
            }
            else
            {
                entity.setVerticalMov(0f);
                entity.setIsRunning(false);
                autoMoving = false;
            }
        }
        
        if (direction == "south")
        {
            if (!autoMoving)
            {
                characterDestination = new Vector2(pos.x, pos.y - distance);
                autoMoving = true;
            }
            
            // determine if destination has been reached
            destinationReached = pos.y <= characterDestination.y;

            if (!destinationReached)
            {   
                entity.setVerticalMov(-1f);
            }
            else
            {
                entity.setVerticalMov(0f);
                entity.setIsRunning(false);
                autoMoving = false;
            }
        }
        
        if (direction == "west")
        {
            if (!autoMoving)
            {
                characterDestination = new Vector2(pos.x - distance, pos.y);
                autoMoving = true;
            }
            
            // determine if destination has been reached
            destinationReached = pos.x <= characterDestination.x;

            if (!destinationReached)
            { 
                entity.setHorizontalMov(-1f);
            }
            else
            {
                entity.setHorizontalMov(0f);
                entity.setIsRunning(false);
                autoMoving = false;
            }
        }

        if (direction == "NW")
        {
            if (!autoMoving)
            {
                characterDestination = new Vector2(pos.x - distance, pos.y + distance);
                autoMoving = true;
            }
            
            // determine if destination has been reached
            destinationReached = (pos.x <= characterDestination.x && pos.y >= characterDestination.y);

            if (!destinationReached)
            {
                entity.setHorizontalMov(-1f);
                entity.setVerticalMov(1f);
            }
            else
            {
                entity.setHorizontalMov(0f);
                entity.setVerticalMov(0f);
                entity.setIsRunning(false);
                autoMoving = false;
            }
        }
        
        if (direction == "NE")
        {
            if (!autoMoving)
            {
                characterDestination = new Vector2(pos.x + distance, pos.y + distance);
                autoMoving = true;
            }
            
            // determine if destination has been reached
            destinationReached = (pos.x >= characterDestination.x && pos.y >= characterDestination.y);

            if (!destinationReached)
            {
                entity.setHorizontalMov(1f);
                entity.setVerticalMov(1f);
            }
            else
            {
                entity.setHorizontalMov(0f);
                entity.setVerticalMov(0f);
                entity.setIsRunning(false);
                autoMoving = false;
            }
        }
        
        if (direction == "SE")
        {
            if (!autoMoving)
            {
                characterDestination = new Vector2(pos.x + distance, pos.y - distance);
                autoMoving = true;
            }
            
            // determine if destination has been reached
            destinationReached = (pos.x >= characterDestination.x && pos.y <= characterDestination.y);

            if (!destinationReached)
            {
                entity.setHorizontalMov(1f);
                entity.setVerticalMov(-1f);
            }
            else
            {
                entity.setHorizontalMov(0f);
                entity.setVerticalMov(0f);
                entity.setIsRunning(false);
                autoMoving = false;
            }
        }
        
        if (direction == "SW")
        {
            if (!autoMoving)
            {
                characterDestination = new Vector2(pos.x - distance, pos.y - distance);
                autoMoving = true;
            }
            
            // determine if destination has been reached
            destinationReached = (pos.x <= characterDestination.x && pos.y <= characterDestination.y);

            if (!destinationReached)
            {       
                entity.setHorizontalMov(-1f);
                entity.setVerticalMov(-1f);
            }
            else
            {
                entity.setHorizontalMov(0f);
                entity.setVerticalMov(0f);
                entity.setIsRunning(false);
                autoMoving = false;
            }
        }

        return destinationReached;
    }

    private bool faceNorth(Animator anim)
    {
        anim.SetFloat("lastMoveY", 1.0f);
        anim.SetFloat("lastMoveX", 0f);
        return anim.GetFloat("lastMoveY") >= 1.0f;
    }
    
    private bool faceSouth(Animator anim)
    {
        anim.SetFloat("lastMoveY", -1.0f);
        anim.SetFloat("lastMoveX", 0f);
        return anim.GetFloat("lastMoveY") <= 1.0f;
    }
    
    private bool faceEast(Animator anim)
    {
        anim.SetFloat("lastMoveX", 1.0f);
        anim.SetFloat("lastMoveY", 0f);
        return anim.GetFloat("lastMoveX") >= 1.0f;
    }
    
    private bool faceWest(Animator anim)
    {
        anim.SetFloat("lastMoveX", -1.0f);
        anim.SetFloat("lastMoveY", 0f);
        return anim.GetFloat("lastMoveX") <= 1.0f;
    }

    private bool stopAllFollowing()
    {
        var party = GameManager.instance.party;
        
        foreach (var member in party)
        {
            member.stopFollowing();
        }


        return true;
    }

    private bool followTheLeader(bool repositionPlayers)
    {
        GameManager.instance.initializeFollowTheLeader(repositionPlayers);
        return true;
    }

    private bool delay(float time)
    {
        
        if (endTime <= 0)
        {
            endTime = Time.fixedTime + time;
        }

        var currentTime = Time.fixedTime;

        if (currentTime >= endTime)
        {
            endTime = 0;
            return true;
        }
        
        return false;
    }

    private bool pause(EventWorker worker)
    {
        gameObject.GetComponent<MovingEntity>().setHorizontalMov(0f);
        gameObject.GetComponent<MovingEntity>().setVerticalMov(0f);
        worker.pauseNow();
        return true;
    }

    private bool equipItem(IEquippable equipTarget, string slotHandle, string itemHandle, InventoryObject inventory)
    {
        equipTarget.equip(slotHandle, itemHandle, inventory);
        return true;
    }
    
    private bool unEquipItem(IEquippable equipTarget, string slotHandle, InventoryObject inventory)
    {
        equipTarget.unEquip(slotHandle, inventory);
        return true;
    }

    private bool setExitsEnabled(bool setting)
    {
        GameManager.instance.setExitsEnabled(setting);
        return true;
    }

    private bool remoteResumeSeq(EventSequence es)
    {
        es.resumeSeq();
        return true;
    }

    private bool remotePause(EventSequence es)
    {
        es.pauseNow();
        return true;
    }

    private bool msg(params string[] lns)
    {
        
        DialogManager.instance.showDialog(lns);
        return true;
    }
    
    private bool msgCls()
    {
        DialogManager.instance.closeDialog();
        return true;
    }
    
    private bool wait(EventWorker ew)
    {
        ew.setWaitForKey(true);
        return true;
    }

    private bool goToScene(string sceneName, float x, float y, bool partOfSequence, EventWorker ew, EventSequence es)
    {   
        // if not part of sequence, clear event queue
        if (!partOfSequence)
        {
            ew.cancelEventQueue();
        }
        
        GameManager.instance.GoToScene(sceneName, x, y, partOfSequence, es, ew);
        return true;
    }

    private bool addGameWorldVariable(string _name, int _val)
    {
        GameManager.instance.gameDatabase.addGameWorldVariable(_name, _val);
        return true;
    }

    private bool setGameWorldVariable(string _name, int _value)
    {
        GameManager.instance.gameDatabase.gameWorldVariableValue(_name, _value);
        return true;
    }
    
    private bool removeGameWorldVariable(string _name)
    {
        GameManager.instance.gameDatabase.removeGameWorldVariable(_name);
        return true;
    }

    private bool showOrHideCharacter(SpriteRenderer sprite, bool show)
    {
        if (show)
        {
            sprite.enabled = true;
        }
        else
        {
            sprite.enabled = false;
        }

        return true;
    }

    private bool positionCharacter(GameObject character, float x, float y)
    {
        character.transform.position = new Vector2(x, y);
        return true;
    }

    private bool remoteCancelSeq(EventSequence eventSequence)
    {
        eventSequence.cancelSequence();
        return true;
    }

    private bool remoteRunSeq(EventSequence eventSequence)
    {
        eventSequence.run();
        return true;
    }

    private bool triggerAnimation(Animator anim, string trigger)
    {
        anim.SetTrigger(trigger);
        return true;
    }

    private bool changeCameraFollowTarget(GameObject newTarget)
    {
        camera = FindObjectOfType<CamController>();
        camera.setCameraTarget(newTarget.transform);
        return true;
    }
    
    private bool promptWin(string headerText, params string[] options)
    {
        PromptWindow.instance.showPrompt(headerText, options);
        return true;
    }

    private bool waitForPrompt(EventSequence.PromptCallback callback)
    {
        var promptCursor = PromptWindow.instance.promptCursor;
   
        var numOfOptions = PromptWindow.instance.numOfOptions;
        var cursorX = PromptWindow.instance.promptCursor.localPosition.x;
        var moveUnit = PromptWindow.instance.cursorMovUnit;
        
        // initialize cursor position
        if (!promptRunning)
        {
            currentPick = 1;
            promptRunning = true;
        }
        
        // allow cursor to move 
        if (currentPick == 1)
        {
            if (Input.GetAxisRaw("Vertical").Equals(-1f) && numOfOptions >= 2)
            {
                promptCursor.localPosition = new Vector2(promptCursor.localPosition.x, PromptWindow.instance.promptCursor.localPosition.y - moveUnit);
                currentPick = 2;
                Input.ResetInputAxes();
            } 
        }
    
        if (currentPick == 2)
        {
            if (Input.GetAxisRaw("Vertical").Equals(1f))
            {
                promptCursor.localPosition = new Vector2(promptCursor.localPosition.x, PromptWindow.instance.promptCursor.localPosition.y + moveUnit);
                currentPick = 1;
                Input.ResetInputAxes();
            } else if (Input.GetAxisRaw("Vertical").Equals(-1f) && numOfOptions >= 3)
            {
                promptCursor.localPosition = new Vector2(promptCursor.localPosition.x, PromptWindow.instance.promptCursor.localPosition.y - moveUnit);
                currentPick = 3;
                Input.ResetInputAxes();
            }
        }
            
        if (currentPick == 3)
        {
            if (Input.GetAxisRaw("Vertical").Equals(1f))
            {
                promptCursor.localPosition = new Vector2(promptCursor.localPosition.x, PromptWindow.instance.promptCursor.localPosition.y + moveUnit);
                currentPick = 2;
                Input.ResetInputAxes();
            } else if (Input.GetAxisRaw("Vertical").Equals(-1f) && numOfOptions >= 4)
            {
                promptCursor.localPosition = new Vector2(promptCursor.localPosition.x, PromptWindow.instance.promptCursor.localPosition.y - moveUnit);
                currentPick = 4;
                Input.ResetInputAxes();
            }
        }
            
        if (currentPick == 4)
        {
            if (Input.GetAxisRaw("Vertical").Equals(1f))
            {
                promptCursor.localPosition = new Vector2(promptCursor.localPosition.x, PromptWindow.instance.promptCursor.localPosition.y + moveUnit);
                currentPick = 3;
                Input.ResetInputAxes();
            } 
        }
        
        // allow choice
        if (GameManager.instance.getMainFireKeyUp())
        {
            PromptWindow.instance.closePromptWindow();
            promptRunning = false;
            
            // run callback 
            callback(currentPick);
            return true;
        }
        
        // do not move on until an option has been chosen
        return false;
    }

    private bool giveItem(string itemName, int amountToGive = 1, InventoryObject inventoryObj = null)
    {
        var item = GameManager.instance.itemsDatabase[itemName];

        if (item == null)
        {
            Debug.LogError("ERROR: Item " + name + "does not exist.");
            return true;
        }
        
        // by default, give to party inventory
        if (inventoryObj == null)
        {
            GameManager.instance.gameDatabase.defaultInventory.addItem(item, amountToGive);
            return true;
        }
        
        // add item to inventory
        inventoryObj.addItem(item, amountToGive);
        
        return true;
    }
    
    private bool takeItem(string itemName, int amountToRemove = 1, InventoryObject inventoryObj = null)
    {
        var item = GameManager.instance.itemsDatabase[itemName];

        if (item == null)
        {
            Debug.LogError("ERROR: Item " + name + "does not exist.");
            return true;
        }
        
        // by default, give to party inventory
        if (inventoryObj == null)
        {
            GameManager.instance.gameDatabase.defaultInventory.removeItem(item, amountToRemove);
            return true;
        }
        
        // add item to inventory
        inventoryObj.removeItem(item, amountToRemove);
        
        return true;
    }
}
