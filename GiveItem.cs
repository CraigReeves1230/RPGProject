using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveItem : EventCutscene
{
    public ItemObject item;
    public int amount = 1;
    
    protected override void doCutscene()
    {
        giveItem(item.name, amount);
        if (amount != 1)
        {
            msg("Received " + amount + " " + item.name);
        }
        else
        {
            msg("Received " + item.name);
        }
        
        wait();
        msgCls();
    }
}
