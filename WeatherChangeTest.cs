using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherChangeTest : EventCutscene
{
    
    protected override void doCutscene()
    {
        var r = GameManager.instance.partyLead();
        var s = GameManager.instance.party[1];
        
        turnToFace(s, r.gameObject);
        msg(":name Blondie :face 0", "OK, here goes nothing...");
        wait();
        msgCls();
        setRain(false, true);
        delay(1f);
        setSnow(true, true, 0.01f, 1f);
        setFog(true, true, 0.01f, 0.005f);
        delay(4f);
        faceEast(r);
        delay(2f);
        faceWest(r);
        delay(2f);
        faceSouth(r);
        delay(2f);
        msg(":name Baldman :face 1", "Well, well...looks like the snow came just in time.");
        wait();
        msg(":name Blondie :face 0", "Aren't we so lucky, haha!");
        wait();
        msgCls();
    }
}
