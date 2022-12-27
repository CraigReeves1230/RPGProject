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
    private int currentPick = 1;
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
        

        /*if (commandName == "promptWin")
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
        }*/

        /*if (commandName == "waitForPrompt")
        {
            var dm = command.getDialogManagerParam();
            var callback = command.getCallbackParam();
            return waitForPrompt(dm, callback);
        }
        */

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
            return goToScene(sceneName, x, y, partOfSequence, ew);
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
            var darken = command.getBoolParameters()[1];
            var darknessSpeed = command.getFloatParameters()[0];
            var rainTransitionSpeed = command.getFloatParameters()[1];
            var rainIntensity = command.getFloatParameters()[2];
            return setRain(setting, darken, darknessSpeed, rainTransitionSpeed, rainIntensity);
        }
        
        if (commandName == "setSnow")
        {
            var setting = command.getBoolParameters()[0];
            var darken = command.getBoolParameters()[1];
            var darknessSpeed = command.getFloatParameters()[0];
            var snowTransitionSpeed = command.getFloatParameters()[1];
            var snowIntensity = command.getFloatParameters()[2];
            return setSnow(setting, darken, darknessSpeed, snowTransitionSpeed, snowIntensity);
        }
        
        if (commandName == "setFog")
        {
            var setting = command.getBoolParameters()[0];
            var darken = command.getBoolParameters()[1];
            var darknessSpeed = command.getFloatParameters()[0];
            var fogTransitionSpeed = command.getFloatParameters()[1];
            var fogIntensity = command.getFloatParameters()[2];
            return setFog(setting, darken, darknessSpeed, fogTransitionSpeed, fogIntensity);
        }

        if (commandName == "setNextWeather")
        {
            var rain = command.getBoolParameters()[0];
            var snow = command.getBoolParameters()[1];
            var fog = command.getBoolParameters()[2];
            var darkness = command.getBoolParameters()[3];
            return setNextWeather(rain, snow, fog, darkness);
        }

        /*if (commandName == "giveItem")
        {
            var itemName = command.getStringParameters()[0];
            return giveItem(itemName);
        }*/

        return true;
    }
    
    /* ----------------------------------------------------------------------------------------------------------*/

    private bool setRain(bool setting, bool darkenScene, float darknessSpeed = 0.5f, float rainTransitionSpeed = 10f, float rainIntensity = 550f)
    {
        weather.setRain(setting, darkenScene, darknessSpeed, rainTransitionSpeed, rainIntensity);
        return true;
    }

    private bool setNextWeather(bool rain, bool snow, bool fog, bool darkness)
    {
        GameManager.instance.setNextRain(rain);
        GameManager.instance.setNextFog(fog);
        GameManager.instance.setNextSnow(snow);
        GameManager.instance.setNextDarkness(darkness);

        return true;
    }
    
    private bool setSnow(bool setting, bool darkenScene, float darknessSpeed = 0.5f, float snowTransitionSpeed = 10f, float snowIntensity = 300f)
    {
        weather.setSnow(setting, darkenScene, darknessSpeed, snowTransitionSpeed, snowIntensity);
        return true;
    }
    
    private bool setFog(bool setting, bool darkenScene, float darknessSpeed = 0.5f, float fogTransitionSpeed = 10f, float fogIntensity = 10f)
    {
        weather.setFog(setting, darkenScene, darknessSpeed, fogTransitionSpeed, fogIntensity);
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

    private bool goToScene(string sceneName, float x, float y, bool partOfSequence, EventWorker ew)
    {   
        // if not part of sequence, clear event queue
        if (!partOfSequence)
        {
            ew.cancelEventQueue();
        }
        
        GameManager.instance.GoToScene(sceneName, x, y, partOfSequence);
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
