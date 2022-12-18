using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SampleDialog : EventSequence
{   
    
    public override void run()
    {
        walkEast(GameManager.instance.partyLead(), 4f);
        delay(1f);
        msg(":name Blondie :face 0", "This is a test. Yada, yada yada.");
        wait();
        msg(":name Blondie :face 0", "For I am Vegeta, the prince of all Saiyans!");
        wait();
        msg(":close");
        runSouth(GameManager.instance.partyLead(), 4f);
        delay(1f);
        msg(":name Baldman :face 1", "This is another test. Yada, yada yada.");
        wait();
        msg(":close");
    }
}
