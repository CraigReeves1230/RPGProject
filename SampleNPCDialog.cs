using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleNPCDialog : NPCDialog
{
   
    protected override void doDialog()
    {
        if (getGameWorldInt("talkedToWizard") == 0)
        {
            msg(":name Wizard", "I am an NPC. This is sample dialog.");
            wait();
            msg("Have a nice day!");
            wait();
            msg(":name Baldman :face 0", "You too, sir!");
            wait();
            msgCls();
            setGameWorldInt("talkedToWizard", 1);
        }
        else if (getGameWorldInt("talkedToWizard") == 1)
        {
            msg(":name Baldman :face 0", "We already spoke, sir.");
            wait();
            msgCls();
            setGameWorldInt("talkedToWizard", 2);
        }
        else if (getGameWorldInt("talkedToWizard") == 2)
        {
            msg(":name Blondie :face 1", "Dude, how many times do we have to--");
            delay(1f);
            msg("name Baldman :face 0", "Easy, Blondie. This is just a test.");
            wait();
            msg("C'mon, let's get moving.");
            wait();
            msg("Sorry to bother you, old man.");
            wait();
            msg(":name Wizard :face none", "Oh no, you're not bothering me, haha.");
            wait();
            msgCls();
            removeGameWorldInt("talkedToWizard");
        } else if (getGameWorldInt("talkedToWizard") == 3)
        {
            msg(":name Wizard :face none", "Bro, how did you make it rain!?");
            wait();
            msgCls();
        }
    }
}
