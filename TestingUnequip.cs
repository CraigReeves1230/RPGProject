using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingUnequip : EventCutscene
{
    protected override void doCutscene()
    {
        msg(":face 0 :name Baldman", "Alright, Blondie. Let's un-equip!");
        wait();
        msgCls();
        unEquipItem(GameManager.instance.partyLead(), "RightHand");
    }
}
