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
        walkSouth(baldman, .5f);
        turnToFace(blondie, baldman.gameObject);
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
        delay(.5f);
        //fadeToBlack();
        goToScene(29.63f, -9.1f, "Cave2", true, true);
        positionCharacter(blondie, 27.6f, -9f);
        fadeIn();
        delay(2f);
        msg(":name Baldman :face 1", "We still here, daddy!");
        wait();
        msg("We in this and we the ones!");
        wait();
        msgCls();
        msg(":name Blondie :face 0", "Alright, Baldman, I'm glad to see you're back in good spirits, but let's try to remember the mission.");
        wait();
        msgCls();
        returnControl();
    }
}
