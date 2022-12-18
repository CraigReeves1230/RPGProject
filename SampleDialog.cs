using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SampleDialog : EventSequence
{
    public MovingEntity baldman;
    public MovingEntity blondie;

    public void Start()
    {
        base.Start();
    }
    
    public override void run()
    {
        walkEast(GameManager.instance.partyLead(), 4f);
        delay(1f);
        msg(":name Blondie :face 0", "This is a test. Yada, yada yada.");
        wait();
        msg("For I am Blondie, the princess of all Saiyans!");
        wait();
        msgCls();
        runSouth(GameManager.instance.partyLead(), 4f);
        delay(1f);
        msg(":name Baldman :face 1", "This is yet another test. Yada, yada yada.");
        wait();
        msgCls();
        stopAllFollowing();
        walkWest(GameManager.instance.party[1], 4f);
        faceWest(GameManager.instance.partyLead());
        delay(.5f);
        msg(":name Baldman :face 1", "I don't know, Blondie. I get the impression we're in for a pretty wild ride.");
        wait();
        msg("I'd be on my P's and Q's...");
        wait();
        walkWest(GameManager.instance.partyLead(), 2f);
        msg(":name Blondie :face 0", "You worry too much, Baldman. I'm really happy you're here.");
        wait();
        msgCls();
        followTheLeader();
        returnControl();
    }
}
