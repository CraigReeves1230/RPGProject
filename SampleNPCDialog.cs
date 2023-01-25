using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleNPCDialog : NPCDialog
{
   
    protected override void doDialog()
    {
        PlayerObject lead = GameManager.instance.partyLead().player;
        
        if (getCustomInt(lead.customVariables, "talkedToWizard") == 0)
        {
            msg(":name Wizard", "I am an NPC. This is sample dialog.");
            wait();
            msg("Have a nice day!");
            wait();
            msg(":name Baldman :face 0", "You too, sir!");
            wait();
            msgCls();
            setCustomInt(lead.customVariables, "talkedToWizard", 1);
        }
        else if (getCustomInt(lead.customVariables, "talkedToWizard") == 1)
        {
            msg(":name Baldman :face 0", "We already spoke, sir.");
            wait();
            msgCls();
            setCustomInt(lead.customVariables, "talkedToWizard", 2);
        }
        else if (getCustomInt(lead.customVariables, "talkedToWizard") == 2)
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
            removeCustomInt(lead.customVariables, "talkedToWizard");
        } else if (getCustomInt(lead.customVariables, "talkedToWizard") == 3)
        {
            msg(":name Wizard :face none", "Bro, how did you make it rain!?");
            wait();
            msgCls();
        }
    }
}
