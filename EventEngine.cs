using UnityEngine;
using UnityEngine.SceneManagement;

public class EventEngine : MonoBehaviour
{
    private GameObject gameObjectParam;
    private float[] floatParams;
    private string[] stringParams;
    private string name;
    private bool isDone;
    private Vector2 characterDestination;
    private bool autoMoving;
    private float endTime;
    private GoToScene gts;
    private CamController camera;
    private bool promptRunning;
    private int currentPick = 1;
    private Weather weather;
    
    public bool start(Command command)
    {        
        var commandName = command.getName();
        gts = FindObjectOfType<GoToScene>();
        weather = FindObjectOfType<Weather>();

        camera = FindObjectOfType<CamController>();

        if (commandName == "walkEast")
        {
            var cm = command.getGameObject().GetComponent<MovingEntity>();
            var dist = command.getFloatParameters()[0];
            return autoMove(cm, "east", dist, false);
        }
        
        if (commandName == "walkWest")
        {
            var cm = command.getGameObject().GetComponent<MovingEntity>();
            var dist = command.getFloatParameters()[0];
            return autoMove(cm, "west", dist, false);
        }

        if (commandName == "walkSouth")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            var dist = command.getFloatParameters()[0];
            return autoMove(cm, "south", dist, false);
        }
        
        if (commandName == "walkNorth")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            var dist = command.getFloatParameters()[0];
            return autoMove(cm, "north", dist, false);
        }

        if (commandName == "returnControl")
        {
            var es = command.getEventSequenceParam();
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            return returnControl(cm, es);
        }

        if (commandName == "stealControl")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            return stealControl(cm);
        }

        if (commandName == "faceNorth")
        {
            var anim = command.getGameObject().GetComponent<Animator>();
            return faceNorth(anim);
        }
        
        if (commandName == "faceWest")
        {
            var anim = command.getGameObject().GetComponent<Animator>();
            return faceWest(anim);
        }
        
        if (commandName == "faceSouth")
        {
            var anim = command.getGameObject().GetComponent<Animator>();
            return faceSouth(anim);
        }
        
        if (commandName == "faceEast")
        {
            var anim = command.getGameObject().GetComponent<Animator>();
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

        if (commandName == "msg")
        {
            var name = command.getStringParameters()[0];
            var message = command.getStringParameters()[1];
            var dialogManager = command.getDialogManagerParam();
            return msg(dialogManager, name, message);
        }
        
        if (commandName == "msgWithHeight")
        {
            var name = command.getStringParameters()[0];
            var message = command.getStringParameters()[1];
            var height = command.getFloatParameters()[0];
            var dialogManager = command.getDialogManagerParam();
            return msg(dialogManager, name, message, height);
        }

        if (commandName == "picMsg")
        {
            var name = command.getStringParameters()[0];
            var message = command.getStringParameters()[1];
            var dialogManager = command.getDialogManagerParam();
            var character = command.getGameObject();
            var avatarIndex = command.getIntParameters()[0];
            return picMsg(dialogManager, name, message, character, avatarIndex);
        }
        
        if (commandName == "picMsgWithHeight")
        {
            var name = command.getStringParameters()[0];
            var message = command.getStringParameters()[1];
            var dialogManager = command.getDialogManagerParam();
            var character = command.getGameObject();
            var avatarIndex = command.getIntParameters()[0];
            var height = command.getFloatParameters()[0];
            return picMsg(dialogManager, name, message, height, character, avatarIndex);
        }

        if (commandName == "msgClose")
        {
            var dm = command.getDialogManagerParam();
            return msgClose(dm);
        }

        if (commandName == "promptWin")
        {
            var dm = command.getDialogManagerParam();
            var headerText = command.getStringParameters()[0];
            
            if (command.getStringParameters().Length < 4)
            {
                return promptWin(dm, headerText, command.getStringParameters()[1], command.getStringParameters()[2]);
            }
            
            if (command.getStringParameters().Length == 4)
            {
                return promptWin(dm, headerText, command.getStringParameters()[1], command.getStringParameters()[2], command.getStringParameters()[3]);
            }
            
            if (command.getStringParameters().Length == 5)
            {
                return promptWin(dm, headerText, command.getStringParameters()[1], command.getStringParameters()[2], command.getStringParameters()[3], command.getStringParameters()[4]);
            }
        }

        if (commandName == "waitForPrompt")
        {
            var dm = command.getDialogManagerParam();
            var callback = command.getCallbackParam();
            return waitForPrompt(dm, callback);
        }

        if (commandName == "wait")
        {
            var eventWorker = command.getEventWorkerParameter();
            return wait(eventWorker);
        }

        if (commandName == "goToScene")
        {
            var scene = command.getSceneParam();
            var x = command.getFloatParameters()[0];
            var y = command.getFloatParameters()[1];
            var partOfSequence = command.getBoolParameters()[0];
            var player = command.getGameObject();
            return goToScene(scene, x, y, player, partOfSequence);
        }

        if (commandName == "runEast")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            var dist = command.getFloatParameters()[0];
            return autoMove(cm, "east", dist, true);
        }
        
        if (commandName == "runSouth")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            var dist = command.getFloatParameters()[0];
            return autoMove(cm, "south", dist, true);
        }
        
        if (commandName == "runWest")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            var dist = command.getFloatParameters()[0];
            return autoMove(cm, "west", dist, true);
        }
        
        if (commandName == "runNorth")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            var dist = command.getFloatParameters()[0];
            return autoMove(cm, "north", dist, true);
        }

        if (commandName == "showCharacter")
        {
            var sprite = command.getGameObject().GetComponent<SpriteRenderer>();
            return showOrHideCharacter(sprite, true);
        }
        
        if (commandName == "hideCharacter")
        {
            var sprite = command.getGameObject().GetComponent<SpriteRenderer>();
            return showOrHideCharacter(sprite, false);
        }

        if (commandName == "positionCharacter")
        {
            var x = command.getFloatParameters()[0];
            var y = command.getFloatParameters()[1];
            var character = command.getGameObject();

            return positionCharacter(character, x, y);
        }

        if (commandName == "triggerAnimation")
        {
            var trigger = command.getStringParameters()[0];
            var anim = command.getGameObject().GetComponent<Animator>();
            return triggerAnimation(anim, trigger);
        }

        if (commandName == "changeCameraFollowTarget")
        {
            var newTarget = command.getGameObject();
            return changeCameraFollowTarget(newTarget);
        }
        
        if (commandName == "changeCameraSpeed")
        {
            var newSpeed = command.getFloatParameters()[0];
            return changeCameraSpeed(newSpeed);
        }

        if (commandName == "walkNW")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            var distance = command.getFloatParameters()[0];
            return autoMove(cm, "NW", distance, false);
        }
        
        if (commandName == "walkNE")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            var distance = command.getFloatParameters()[0];
            return autoMove(cm, "NE", distance, false);
        }
        
        if (commandName == "walkSE")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            var distance = command.getFloatParameters()[0];
            return autoMove(cm, "SE", distance, false);
        }
        
        if (commandName == "walkSW")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            var distance = command.getFloatParameters()[0];
            return autoMove(cm, "SW", distance, false);
        }
        
        if (commandName == "runNW")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            var distance = command.getFloatParameters()[0];
            return autoMove(cm, "NW", distance, true);
        }
        
        if (commandName == "runNE")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            var distance = command.getFloatParameters()[0];
            return autoMove(cm, "NE", distance, true);
        }
        
        if (commandName == "runSE")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            var distance = command.getFloatParameters()[0];
            return autoMove(cm, "SE", distance, true);
        }
        
        if (commandName == "runSW")
        {
            var cm = command.getGameObject().GetComponent<CharacterMovement>();
            var distance = command.getFloatParameters()[0];
            return autoMove(cm, "SW", distance, true);
        }

        if (commandName == "setRain")
        {
            var setting = command.getBoolParameters()[0];
            var darken = command.getBoolParameters()[1];
            return setRain(setting, darken);
        }
        
        if (commandName == "setSnow")
        {
            var setting = command.getBoolParameters()[0];
            return setSnow(setting);
        }
        
        if (commandName == "setFog")
        {
            var setting = command.getBoolParameters()[0];
            return setFog(setting);
        }

        if (commandName == "setNextWeather")
        {
            var rain = command.getBoolParameters()[0];
            var snow = command.getBoolParameters()[1];
            var fog = command.getBoolParameters()[2];
            var darkness = command.getBoolParameters()[3];
            return setNextWeather(rain, snow, fog, darkness);
        }

        if (commandName == "giveItem")
        {
            var itemName = command.getStringParameters()[0];
            return giveItem(itemName);
        }

        return true;
    }
    
    /* ----------------------------------------------------------------------------------------------------------*/

    private bool setRain(bool setting, bool darkenScene)
    {
        weather.setRain(setting, darkenScene);
        return true;
    }

    private bool setNextWeather(bool rain, bool snow, bool fog, bool darkness)
    {
        gameWorld.nextRain = rain;
        gameWorld.nextFog = fog;
        gameWorld.nextSnow = snow;
        gameWorld.nextDarkness = darkness;

        return true;
    }
    
    private bool setSnow(bool setting)
    {
        weather.setSnow(setting);
        return true;
    }
    
    private bool setFog(bool setting)
    {
        weather.setFog(setting);
        return true;
    }
    
    private bool stealControl(CharacterMovement cm)
    {
        // shut off follow for all players
        foreach (var player in gameWorld.party)
        {
            player.setControlOverride(true);
            player.setIsFollowing(false);
        }
        
        cm.setControlOverride(true);
        return !cm.isUnderPlayerControl();
    }

    private bool returnControl(CharacterMovement cm, EventSequence es)
    {
        cm.setControlOverride(false);
        es.updateWithinZone(cm.gameObject);
        gameWorld.initializeParty();
        return cm.isUnderPlayerControl();
    }
    
    private bool autoMove(MovingEntity entity, string direction, float distance, bool running)
    {
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
                GameManager.instance.revokeControl();
                
                // determine if running
                if (running)
                {
                    entity.setIsRunning(true);
                }
                
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
                GameManager.instance.revokeControl();
                
                // determine if running
                if (running)
                {
                    entity.setIsRunning(true);
                }
                
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
                GameManager.instance.revokeControl();
                
                // determine if running
                if (running)
                {
                    entity.setIsRunning(true);
                }
                
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
                GameManager.instance.revokeControl();
                
                // determine if running
                if (running)
                {
                    entity.setIsRunning(true);
                }
                
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
                GameManager.instance.revokeControl();
                
                // determine if running
                if (running)
                {
                    entity.setIsRunning(true);
                }
                
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
                GameManager.instance.revokeControl();
                
                // determine if running
                if (running)
                {
                    entity.setIsRunning(true);
                }
                
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
                GameManager.instance.revokeControl();
                
                // determine if running
                if (running)
                {
                    entity.setIsRunning(true);
                }
                
              
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
                characterDestination = new Vector3(pos.x - distance, pos.y - distance);
                autoMoving = true;
            }
            
            // determine if destination has been reached
            destinationReached = (pos.x <= characterDestination.x && pos.y <= characterDestination.y);

            if (!destinationReached)
            {
                GameManager.instance.revokeControl();
                
                // determine if running
                if (running)
                {
                    entity.setIsRunning(true);
                }
                
               
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
        anim.SetFloat("LastMoveY", 1.0f);
        anim.SetFloat("LastMoveX", 0f);
        return anim.GetFloat("LastMoveY") >= 1.0f;
    }
    
    private bool faceSouth(Animator anim)
    {
        anim.SetFloat("LastMoveY", -1.0f);
        anim.SetFloat("LastMoveX", 0f);
        return anim.GetFloat("LastMoveY") <= 1.0f;
    }
    
    private bool faceEast(Animator anim)
    {
        anim.SetFloat("LastMoveX", 1.0f);
        anim.SetFloat("LastMoveY", 0f);
        return anim.GetFloat("LastMoveX") >= 1.0f;
    }
    
    private bool faceWest(Animator anim)
    {
        anim.SetFloat("LastMoveX", -1.0f);
        anim.SetFloat("LastMoveY", 0f);
        return anim.GetFloat("LastMoveX") <= 1.0f;
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
        worker.pauseNow();
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

    private bool msg(DialogManager dm, params string[] lines)
    {
        dm.showDialog(lines);
        //return dm.getIsRunning();
        return dm.gameObject.activeInHierarchy;
    }

    
    private bool wait(EventWorker ew)
    {
        ew.setWaitForKey(true);
        return true;
    }

    private bool goToScene(Scene scene, float x, float y, bool partOfSequence, ControllableEntity assignControlTo = null)
    {
        gts.setLevelToLoad(scene);
        gts.setAsEvent(partOfSequence);
        
        if (!partOfSequence)
        {
            if (assignControlTo == null)
            {
                GameManager.instance.assignControl(GameManager.instance.partyLead());
            }
            else
            {
                GameManager.instance.assignControl(assignControlTo);
            }
        }
        
        gts.setPosition(x, y);
        gts.go();

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
        camera.setCameraTarget(newTarget.transform);
        return true;
    }

    /*private bool changeCameraSpeed(float newSpeed)
    {
        camera.setCamSpeed(newSpeed);
        return true;
    }*/

    /*private bool promptWin(DialogManager dm, string headerText, params string[] options)
    {
        dm.promptDialog(headerText, options);
        return true;
    }*/

    /*private bool waitForPrompt(DialogManager dm, EventSequence.PromptCallback callback)
    {
     
        var promptCursor = dm.promptCursor;
        var promptCursorPos1 = dm.promptCursorPos1;
        var promptCursorPos2 = dm.promptCursorPos2;
        var promptCursorPos3 = dm.promptCursorPos3;
        var promptCursorPos4 = dm.promptCursorPos4;
        var numOfOptions = dm.numOfOptions;
        
        // initialize cursor position
        if (!promptRunning)
        {
            currentPick = 1;
            promptRunning = true;
        }
        
        // allow cursor to move 
        if (currentPick == 1)
        {
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                promptCursor.localPosition = promptCursorPos2;
                currentPick = 2;
            } else if (Input.GetKeyUp(KeyCode.RightArrow) && numOfOptions >= 3)
            {
                promptCursor.localPosition = promptCursorPos3;
                currentPick = 3;
            }
        }
    
        if (currentPick == 2)
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                promptCursor.localPosition = promptCursorPos1;
                currentPick = 1;
            } else if (Input.GetKeyUp(KeyCode.RightArrow) && numOfOptions >= 4)
            {
                promptCursor.localPosition = promptCursorPos4;
                currentPick = 4;
            }
        }
            
        if (currentPick == 3)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                promptCursor.localPosition = promptCursorPos1;
                currentPick = 1;
            } else if (Input.GetKeyUp(KeyCode.DownArrow) && numOfOptions >= 4)
            {
                promptCursor.localPosition = promptCursorPos4;
                currentPick = 4;
            }
        }
            
        if (currentPick == 4)
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                promptCursor.localPosition = promptCursorPos3;
                currentPick = 3;
            } else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                promptCursor.localPosition = promptCursorPos2;
                currentPick = 2;
            }
        }
        
        // allow choice
        if (Input.GetKeyUp(KeyCode.Return))
        {
            dm.endPromptDialog();
            promptRunning = false;
            
            // run callback 
            callback(currentPick);
            return true;
        }
        
        // do not move on until an option has been chosen
        return false;
    }*/

    /*private bool giveItem(string itemName)
    {
        gameWorld.giveItem(itemName);
        return true;
    }*/
}
