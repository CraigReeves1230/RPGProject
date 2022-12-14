using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string nextAreaEntrance;
    public bool exitsEnabled;
    
    public Vector2 bottomLeftLimit;
    public Vector2 topRightLimit;

    public int maxLevel = 99;
    public bool mapScene = true;

    public PlayableCharacterEntity[] party;
    
    public int followLeaderLimit = 2;
    private bool isParty;

    private bool bottomLeftMarkerDetected;
    private bool topRightMarkerDetected;
    
    private ControllableEntity controlTarget;

    private bool inVehicle = false;
    
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
        if (!inVehicle)
        {
            ControllableEntity lastPlayer = null;
            for (int i = 0; i < (followLeaderLimit + 1); i++)
            { 
                // make player active
                party[i].gameObject.SetActive(true);
            
                // position player to a default location
                party[i].transform.position = new Vector2(party[0].transform.position.x, party[0].transform.position.y);

                if (lastPlayer != null)
                {
                    party[i].FollowTarget(lastPlayer);
                }
            
                lastPlayer = party[i];
            }
        }
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
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }
        
            // assign control by default to party leader if not in vehicle
            if (controlTarget == null && !inVehicle)
            {
                controlTarget = partyLead();
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
    public void assignControl(ControllableEntity entity) => controlTarget = entity;

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
}
