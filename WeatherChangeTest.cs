using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherChangeTest : EventCutscene
{
    
    protected override void doCutscene()
    {
        var r = GameManager.instance.partyLead();
        
        msg(":name Reinhardt", "OK, here goes nothing...");
        wait();
        msgCls();
        delay(1f);
        setSnow(true, false, 0, 1f);
        setFog(true, true, 0.01f, 0.005f);
        delay(4f);
        faceEast(r);
        delay(2f);
        faceWest(r);
        delay(2f);
        faceSouth(r);
        delay(2f);
        msg(":name Reinhardt", "Well, well...looks like the snow came just in time.");
        wait();
        msgCls();
    }
}
