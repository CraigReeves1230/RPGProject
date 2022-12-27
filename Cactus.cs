using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : EventCutscene
{
    protected override void doCutscene()
    {
        msg(":name Blondie :face 0", "A cactus, Craig? Really???");
        wait();
        msg(":name Baldman :face 1", "C'mon, Blondie, go easy on 'em. It's just a test!");
        wait();
        msgCls();
    }
}
