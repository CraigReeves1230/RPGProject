using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[Required]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Required, PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]
    public GameData gameDatabase;
    
    private string nextAreaEntrance;
    private bool exitsEnabled;
    
    
    private Vector2 bottomLeftLimit;
    private Vector2 topRightLimit;

    private Vector2 camBottomLeftLimit;
    private Vector2 camTopRightLimit;

    [PropertySpace(SpaceAfter = 5)]
    public bool mapScene = true;

    private ControllableEntity lastControlled;

    [Required]
    public List<PlayableCharacterEntity> party;
    public Dictionary<string, Sprite> partyFaces = new Dictionary<string, Sprite>();
    
    [System.NonSerialized]
    public int followLeaderLimit = 3;
    
    private bool redoFollowTheLeaderScheduled;
    private bool returnControlScheduled;
    
    private bool isParty;

    private bool varHydrationCheck;
    
    private string currentScene;

    public Dictionary<string, bool[]> weatherOverrides;

    private bool eventSequenceRunning;
    
    private bool bottomLeftMarkerDetected;
    private bool topRightMarkerDetected;

    private bool userControl;    // does user have control of players
    
    private ControllableEntity controlTarget;    // the player or vehicle user controls on screen

    private bool autoReturnControl;

    private string nextScene;

    private bool positionPartyScheduled;

    private bool itemDatabaseFilled;

    private EventSequence destroyEventSequenceScheduled;
    private EventWorker destroyEventWorkerScheduled;
    
    private bool inVehicle;

    private Vector2 nextPosition;
    private string nextSceneToLoad;
    private bool fadeInScheduled;
    
    private bool nextRain;
    private bool nextSnow;
    private bool nextFog;
    private bool nextDarkness;

    // Items and inventory
    public Dictionary<string, ItemObject> itemsDatabase = new Dictionary<string, ItemObject>();

    // Start is called before the first frame update
    void Start()
    {
        // game manager should be a singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
       
        // should stay forever and is never destroyed
        DontDestroyOnLoad(gameObject);

        // initialize game variables
        exitsEnabled = true;
        nextAreaEntrance = null;
        userControl = true;
        followLeaderLimit = gameDatabase.followLeaderLimit;
        
        // intialize in-game inventory dictionary
        foreach (var item in gameDatabase.allItems)
        {
            if (item.customVariables != null)
            {
                CustomVariables.hydrateDictionaries(item.customVariables);
            }
            
            itemsDatabase[item.name] = item;
        }
        
        // add main currency
        if (gameDatabase.mainCurrency != null)
        {
            itemsDatabase[gameDatabase.mainCurrency.name] = gameDatabase.mainCurrency;
        }
                
        // initialize players
        foreach (var player in gameDatabase.allPlayers)
        {
            player.init();
        }
        
        // initialize party 
        for (int i = 0; i < party.Count; i++)
        {
            isParty = true;
            
            // by default, only party lead appears on screen (if not in a vehicle)
            if (i == 0)
            {
                party[i].gameObject.SetActive(!inVehicle);
            }
            else
            {
                party[i].gameObject.SetActive(false);
            }
        } 
        
        // weather overrides
        weatherOverrides = new Dictionary<string, bool[]>();
        
        // store party faces
        foreach (var member in party)
        {
            if (member.player.face != null)
            partyFaces[member.player.defaultName] = member.player.face;
        }
        
        // initialize follow the leader
        initializeFollowTheLeader();
    }

    // Update is called once per frame
    void Update()
    {        
        // hydrate game variable dictionary
        if (!varHydrationCheck)
        {
            if (gameDatabase.customVariables != null)
            {
                CustomVariables.hydrateDictionaries(gameDatabase.customVariables);
            }

            varHydrationCheck = true;
        }
        
        
        if (mapScene)
        {
            // look for bounds
            if (bottomLeftMarkerDetected == false)
            {
                errorMsg("Error: Bottom left marker is missing.");
            }
        
            if (topRightMarkerDetected == false)
            {
                errorMsg("Error: Top right marker is missing.");
            }

            // Make sure party is loaded
            if (!isParty)
            {
                errorMsg("Error: No party leader. Add party members in Game Manager.");
            }
        
            // allow user to quit game for now
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        
            // assign control by default to party leader if not in vehicle
            if (controlTarget == null && !inVehicle)
            {
                controlTarget = partyLead();
            }

            // keeps track of the current scene
            currentScene = SceneManager.GetActiveScene().name;
            
            // scene management
            if (nextScene != null && nextScene != currentScene)
            {
                SceneManager.LoadScene(nextScene);
                
                // position players
                if (positionPartyScheduled)
                {
                    foreach (var member in party)
                    {
                        member.transform.position = nextPosition;
                    }
                    positionPartyScheduled = false;
                }

                if (redoFollowTheLeaderScheduled)
                {
                    initializeFollowTheLeader();
                    redoFollowTheLeaderScheduled = false;
                }
                
                if (returnControlScheduled)
                {
                    restoreControl();
                    
                    returnControlScheduled = false;
                }

                nextScene = null;
            }
            
            // if an event was triggered from another scene, destroy it in the current scene once it's completed
            if (destroyEventSequenceScheduled != null)
            {
                if (destroyEventWorkerScheduled.numberOfEventsInQueue() < 1)
                {
                    Destroy(destroyEventSequenceScheduled.gameObject);
                    destroyEventSequenceScheduled = null;
                    destroyEventWorkerScheduled = null;
                }
            }

            // manage fading in
            if (fadeInScheduled)
            {
                UIFade.instance.FadeFromBlack();
                fadeInScheduled = false;
            }
        }     
    }

    public void initializeFollowTheLeader(bool positionFollowers = true)
    {
        if (!inVehicle)
        {
            var limit = party.Count < (followLeaderLimit+1) ? party.Count : (followLeaderLimit+1);
                
            ControllableEntity lastPlayer = null;
            for (int i = 0; i < limit; i++)
            { 
                // make player active
                party[i].gameObject.SetActive(true);
            
                // position player to a default location
                if (positionFollowers)
                {
                    party[i].transform.position = new Vector2(party[0].transform.position.x, party[0].transform.position.y);
                }
                
                if (lastPlayer != null)
                {
                    party[i].FollowTarget(lastPlayer);
                    party[i].tag = "WalkThrough";
                }
            
                lastPlayer = party[i];
            }
        }
    }
    
    // Find player by default name
    public PlayableCharacterEntity findByDefName(string searchName)
    {
        foreach (var member in party)
        {
            if (member.player.defaultName == searchName)
            {
                return member;
            }
        }

        return null;
    }
    
    
    // Error message
    public void errorMsg(string message)
    {
        Debug.Log("ERROR: " + message);
        Debug.LogError("ERROR: " + message);
    }
    
    // assigns control of an entity
    public void assignControl(ControllableEntity entity)
    {
        entity.tag = "Player";
        controlTarget = entity;
    }

    // gets the current control target
    public ControllableEntity getControlTarget() => controlTarget;

    public void revokeControl()
    {
        foreach (var member in party)
        {
            Input.ResetInputAxes();
            member.tag = "WalkThrough";
            member.setIsRunning(false);
            member.clearDirectionalBuffer();
        }

        lastControlled = controlTarget;
        userControl = false;
    }

    public void restoreControl()
    {        
        Input.ResetInputAxes();
        lastControlled.setIsRunning(false);
        lastControlled.tag = "Player";
        userControl = true;
    }

    public bool getMainFireKeyUp()
    {
        return Input.GetButtonUp("Fire1");
    }
    
    public bool getMainFireKeyDown()
    {
        return Input.GetButtonDown("Fire1");
    }

    public bool getSecondaryFireKeyDown()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }
    
    public bool getSecondaryFireKeyUp()
    {
        return Input.GetKeyUp(KeyCode.LeftShift);
    }
    
    // go to new scene
    public void GoToScene(string sceneName, float x, float y, bool partOfSequence, EventSequence eventSequence = null, EventWorker ew = null)
    {
        if (!partOfSequence)
        {
            nextPosition = new Vector2(x, y);
            positionPartyScheduled = true;

            if (followLeaderLimit > 0)
            {
                redoFollowTheLeaderScheduled = true;
            }

            fadeInScheduled = true;
            
            returnControlScheduled = true;
        }
        else
        {
            if (eventSequence != null && ew != null)
            {
                // remove event sequence from last scene
                destroyEventSequenceScheduled = eventSequence;
                destroyEventWorkerScheduled = ew;
            }
        }
        
        nextScene = sceneName;
    }

    
    // getters and setters

    public bool getIsEventSequenceRunning() => eventSequenceRunning;
    public void setIsEventSequenceRunning(bool setting) => eventSequenceRunning = setting;
    public bool getAutoReturnControl() => autoReturnControl;
    public void setAutoReturnControl(bool setting) => autoReturnControl = setting;
    public bool getExitsEnabled() => exitsEnabled;
    public void setExitsEnabled(bool setting) => exitsEnabled = setting;
    public string getCurrentScene() => currentScene;
    public void setNextScene(string scn) => nextScene = scn;
    public void scheduleFadeIn() => fadeInScheduled = true;
    public bool hasControl() => userControl;
    
    public bool isControlTarget(ControllableEntity entity) => entity == controlTarget;
    public bool isControlTarget(GameObject entityObj) => entityObj == controlTarget.gameObject;
    public PlayableCharacterEntity partyLead() => party[0];
    public void setBottomLeftMarkerDetected(bool setting) => bottomLeftMarkerDetected = setting;
    public void setTopRightMarkerDetected(bool setting) => topRightMarkerDetected = setting;
    public bool isInVehicle() => inVehicle;
    public void setInVehicle(bool setting) => inVehicle = setting;
    public Vector2 getBottomLeftLimit() => bottomLeftLimit;
    public void setBottomLeftLimit(Vector2 pos) => bottomLeftLimit = pos;
    public Vector2 getTopRightLimit() => topRightLimit;
    public void setTopRightLimit(Vector2 pos) => topRightLimit = pos;
    
    public Vector2 getCamBottomLeftLimit() => camBottomLeftLimit;
    public void setCamBottomLeftLimit(Vector2 pos) => camBottomLeftLimit = pos;
    public Vector2 getCamTopRightLimit() => camTopRightLimit;
    public void setCamTopRightLimit(Vector2 pos) => camTopRightLimit = pos;

    public void setNextRain(bool setting) => nextRain = setting; 
    public void setNextSnow(bool setting) => nextSnow = setting; 
    public void setNextFog(bool setting) => nextFog = setting; 
    public void setNextDarkness(bool setting) => nextDarkness = setting;
    public bool getNextRain() => nextRain;
    public bool getNextSnow() => nextSnow;
    public bool getNextFog() => nextFog;
    public bool getNextDarkness() => nextDarkness;
    public void setNextAreaEntrance(string ae) => nextAreaEntrance = ae;
    public string getNextAreaEntrance() => nextAreaEntrance;
}
