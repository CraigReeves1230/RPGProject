using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEgress : EventCutscene
{
    
    protected override void doCutscene()
    {
        var vanessa_mv = GameManager.instance.partyLead();
        var reggie_mv = GameManager.instance.party[1];
        var reggie = reggie_mv.player;
        var vanessa = vanessa_mv.player;

        switch (getCustomInt(GameManager.instance.gameDatabase.customVariables, "sampleEgress"))
        {
            case 0:
                stopAllFollowing();
                walkSouth(reggie_mv, .5f);
                turnToFace(vanessa_mv, reggie_mv.gameObject);
                delay(.5f);
                pfmsg(reggie, "Here we go...");
                wait();
                msgCls();
                pfmsg(vanessa, "We're in this together, alright?");
                wait();
                msg("Believe in yourself...");
                wait();
                msg("Believe in me.");
                wait();
                msgCls();
                delay(1f);
                turnToFace(reggie_mv, vanessa_mv.gameObject);
                delay(.5f);
                pfmsg(reggie, "Alright, " + vanessa_mv.player.charName + ".");
                wait();
                msg("Whatever happens in there, just know that I will always love you.");
                wait();
                msgCls();
                delay(.5f);
                fadeToScene("Cave2", true);
                positionCharacter(vanessa_mv, 43.21f, 4.46f);
                positionCharacter(reggie_mv, 43.21f, 3.46f);
                turnToFace(reggie_mv, vanessa_mv.gameObject);
                turnToFace(vanessa_mv, reggie_mv.gameObject);
                delay(2f);
                pfmsg(reggie, "We still here, daddy!");
                wait();
                msg("We in this and we the ones!");
                wait();
                msgCls();
                pfmsg(vanessa, "Alright, "+ reggie_mv.player.charName +", I'm glad to see you're back in good spirits, but let's try to remember the mission.");
                wait();
                msgCls();
                setCustomInt(GameManager.instance.gameDatabase.customVariables, "sampleEgress", 1);
                followTheLeader(false);
                break;
            
            case 1:
                pfmsg(vanessa, "Off to the forest!");
                wait();
                msgCls();
                break;
        }       
    }
}
