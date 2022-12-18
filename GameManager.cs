using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private string nextAreaEntrance;
    private bool exitsEnabled;
    
    
    private Vector2 bottomLeftLimit;
    private Vector2 topRightLimit;

    public Vector2 camBottomLeftLimit;
    public Vector2 camTopRightLimit;

    public int maxLevel = 99;
    public bool mapScene = true;

    private ControllableEntity lastControlled;

    public PlayableCharacterEntity[] party;
    
    public int followLeaderLimit = 2;
    private bool isParty;

    private bool bottomLeftMarkerDetected;
    private bool topRightMarkerDetected;

    private bool userControl;    // does user have control of players
    
    private ControllableEntity controlTarget;    // player or vehicle user controls on screen

    private bool autoReturnControl;
    
    private bool inVehicle = false;
    
    private bool nextRain;
    private bool nextSnow;
    private bool nextFog;
    private bool nextDarkness;

    public Vector2 nextDestination;
    
    // Start is called before the first frame update
    void Start()
    {
        // game world should be a singleton
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
        
        
        // initialize party 
        for (int i = 0; i < party.Length; i++)
        {
            party[i].init();
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
        
        // initialize follow the leader
        initializeFollowTheLeader();
    }

    // Update is called once per frame
    void Update()
    {          
        // look for bounds
        if (mapScene)
        {
            if (bottomLeftMarkerDetected == false)
            {
                errorMsg("Error: Bottom left marker is missing.");
            }
        
            if (topRightMarkerDetected == false)
            {
                errorMsg("Error: Top right marker is missing.");
            }

            if (!isParty)
            {
                errorMsg("Error: No party leader. Add party members in Game Manager.");
            }
        
            // allow user to quit game for now
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Application.Quit();
                Debug.Log("Debugging");
            }
        
            // assign control by default to party leader if not in vehicle
            if (controlTarget == null && !inVehicle)
            {
                controlTarget = partyLead();
            }   
        }     
    }

    public void initializeFollowTheLeader()
    {
        if (!inVehicle)
        {
            var limit = party.Length < (followLeaderLimit+1) ? party.Length : (followLeaderLimit+1);
                
            ControllableEntity lastPlayer = null;
            for (int i = 0; i < limit; i++)
            { 
                // make player active
                party[i].gameObject.SetActive(true);
            
                // position player to a default location
                if (lastControlled == null)
                {
                    party[i].transform.position = new Vector2(party[0].transform.position.x, party[0].transform.position.y);
                }
                

                if (lastPlayer != null)
                {
                    party[i].FollowTarget(lastPlayer);
                }
            
                lastPlayer = party[i];
            }
        }
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
    
    // returns true if controllable entity is the current target
    public bool isControlTarget(ControllableEntity entity) => entity == controlTarget;
    public bool isControlTarget(GameObject entityObj) => entityObj == controlTarget.gameObject;
    
    
    // getters and setters
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
    
    public void revokeControl()
    {
        foreach (var member in party)
        {
            Input.ResetInputAxes();
            member.tag = "WalkThrough";
            member.clearDirectionalBuffer();
        }

        lastControlled = controlTarget;
        userControl = false;
    }

    public void restoreControl()
    {        
        Input.ResetInputAxes();
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

    public bool getAutoReturnControl() => autoReturnControl;
    public void setAutoReturnControl(bool setting) => autoReturnControl = setting;
    public Vector2 getNextDestination() => nextDestination;
    public void setNextDestination(Vector2 dest) => nextDestination = dest;
    public bool getExitsEnabled() => exitsEnabled;
    public void setExitsEnabled(bool setting) => exitsEnabled = setting;

    public bool hasControl() => userControl;
}
