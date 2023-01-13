using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TestingForEquip : EventCutscene
{
    protected override void doCutscene()
    {
        msg(":name Blondie :face 1", "Let's equip!");
        wait();
        msgCls();
        equipItem(GameManager.instance.partyLead(), "RightHand", "Dagger");
    }
}
