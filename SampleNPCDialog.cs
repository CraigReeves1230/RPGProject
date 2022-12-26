using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleNPCDialog : NPCDialog
{
   
    protected override void doDialog()
    {
        msg(":name Wizard", "I am an NPC. This is sample dialog.");
        wait();
        msg("Have a nice day!");
        wait();
        msgCls();
    }

    
    
}
