using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SampleEgress : EventCutscene
{
    
    protected override void doCutscene()
    {
        var blondie = GameManager.instance.partyLead();
        var baldman = GameManager.instance.party[1];
        
        stopAllFollowing();
        walkSouth(baldman, .5f);
        turnToFace(blondie, baldman.gameObject);
        delay(.5f);
        runSE(baldman, 2f);
        delay(.5f);
        runNW(baldman, 2f);
        delay(.5f);
        msg(":face 1 :name Baldman", "Here we go...");
        wait();
        msgCls();
        msg(":face 0 :name Blondie", "We're in this together, alright?");
        wait();
        msg("Believe in yourself...");
        wait();
        msg("Believe in me.");
        wait();
        msgCls();
        delay(1f);
        turnToFace(baldman, blondie.gameObject);
        delay(.5f);
        msg(":face 1 :name Baldman", "Alright, Blondie.");
        wait();
        msg("Whatever happens in there, just know that I will always love you.");
        wait();
        msgCls();
        delay(.5f);
        fadeToScene(29.63f, -9.1f, "Cave2", true);
        delay(2f);
        msg(":name Baldman :face 1", "We still here, daddy!");
        wait();
        msg("We in this and we the ones!");
        wait();
        msgCls();
        msg(":name Blondie :face 0", "Alright, Baldman, I'm glad to see you're back in good spirits, but let's try to remember the mission.");
        wait();
        msgCls();
        followTheLeader();
    }
}
