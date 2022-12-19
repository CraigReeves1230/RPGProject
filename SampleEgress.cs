using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SampleEgress : EventSequence
{
    
    public override void run()
    {
        var blondie = GameManager.instance.partyLead();
        var baldman = GameManager.instance.party[1];
        
        stealControl();
        stopAllFollowing();
        faceSouth(blondie);
        walkSouth(baldman, .5f);
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
        faceNorth(baldman);
        delay(.5f);
        msg(":face 1 :name Baldman", "Alright, Blondie.");
        wait();
        msg("Whatever happens in there, just know that I will always love you.");
        wait();
        msgCls();
        goToScene(29.63f, -9.1f, "Cave2", true, false);
    }
}
