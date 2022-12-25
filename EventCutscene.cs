using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventCutscene : EventSequence
{
    
    protected abstract void doCutscene();

    public override void run()
    {
        stealControl();
        setExitsEnabled(false);

        doCutscene();
        
        setExitsEnabled(true);
        returnControl();
    }
}
