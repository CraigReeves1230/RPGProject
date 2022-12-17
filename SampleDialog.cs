using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SampleDialog : EventSequence
{   
    
    public override void run()
    {
        stopAllFollowing();
        walkEast(GameManager.instance.partyLead(), 9f);
        walkSouth(GameManager.instance.partyLead(), 9f);
        walkWest(GameManager.instance.partyLead(), 9f);
        
        returnControl();
        followTheLeader();
        
    }
}
