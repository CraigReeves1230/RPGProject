using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleNPCDialog : NPCDialog
{
   
    protected override void doDialog()
    {
        msg(":name Man", "I'd be careful. That cave is really dangerous.");
        wait();
        msg(":name Blondie :face 0", "Oh trust me, we're well aware!");
        wait();
        msg("But thank you for the heads up!");
        wait();
        msgCls();
    }

    
    
}
