using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDialogCut : EventCutscene
{
    protected override void doCutscene()
    {
        var blondie = GameManager.instance.partyLead();
        var baldman = GameManager.instance.party[1];
        
        walkEast(blondie, 4f);
        delay(1f);
        msg(":name Blondie :face 0", "This is a test. Yada, yada yada.");
        wait();
        msg("For I am Blondie, the princess of all Saiyans!");
        wait();
        msgCls();
        runSouth(blondie, 4f);
        delay(1f);
        msg(":name Baldman :face 1", "This is yet another test. Yada, yada yada.");
        wait();
        msgCls();
        stopAllFollowing();
        walkWest(baldman, 4f);
        faceWest(blondie);
        delay(.5f);
        msg(":name Baldman :face 1", "I don't know, Blondie. I get the impression we're in for a pretty wild ride.");
        wait();
        msg("I'd be on my P's and Q's...");
        wait();
        walkWest(blondie, 2f);
        msg(":name Blondie :face 0", "You worry too much, Baldman. I'm really happy you're here.");
        wait();
        msgCls();
        followTheLeader(false);
    }
}
