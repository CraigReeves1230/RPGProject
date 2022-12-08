using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string nextAreaEntrance;
    public bool exitsEnabled;
    
    public Vector2 bottomLeftLimit;
    public Vector2 topRightLimit;

    public int maxLevel = 99;

    public PlayableCharacterEntity[] party;
    
    public int followLeaderLimit = 2;
    
    private ControllableEntity controlTarget;
    
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
            
            // by default, only party lead appears on screen
            if (i == 0)
            {
                party[i].gameObject.SetActive(true);
            }
            else
            {
                party[i].gameObject.SetActive(false);
            }
        } 
        
        // initialize follow the leader
        var followerCount = 0;
        ControllableEntity lastPlayer = null;
        for (int i = 0; i < (followLeaderLimit + 1); i++)
        { 
            // make player active
            party[i].gameObject.SetActive(true);

            if (lastPlayer != null)
            {
                party[i].FollowTarget(lastPlayer);
            }
            
            lastPlayer = party[i];
        }         
    }

    // Update is called once per frame
    void Update()
    {
        // allow user to quit game for now
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        // assign control by default to party leader
        if (controlTarget == null)
        {
            controlTarget = partyLead();
        }
    }
    
    // assigns control of an entity
    public void assignControl(ControllableEntity entity) => controlTarget = entity;

    // gets the current control target
    public ControllableEntity getControlTarget() => controlTarget;
    
    // returns true if controllable entity is the current target
    public bool isControlTarget(ControllableEntity entity) => entity == controlTarget;
    public bool isControlTarget(GameObject entityObj) => entityObj == controlTarget.gameObject;
    
    
    // gets party leader
    public PlayableCharacterEntity partyLead() => party[0];
}
