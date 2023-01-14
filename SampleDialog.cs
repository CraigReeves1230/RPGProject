using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SampleDialog : EventCutscene
{
    public MovingEntity baldman;
    public MovingEntity blondie;

    public void Start()
    {
        base.Start();
    }
    
    protected override void doCutscene()
    {

        msg(":name Blondie :face 1", "This is a test. Yada, yada yada.");
        wait();
        msg("For I am Blondie, the princess of all Saiyans!");
        wait();
        msgCls();
       
        msg(":name Baldman :face 0", "This is yet another test. Yada, yada yada.");
        wait();
        msg(":name Baldman :face 1", "I don't know, Blondie. I get the impression we're in for a pretty wild ride.");
   
        wait();
        msgCls();
    }
}
