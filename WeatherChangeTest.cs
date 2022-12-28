using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherChangeTest : EventCutscene
{
    
    protected override void doCutscene()
    {
        var r = GameManager.instance.partyLead();
        var s = GameManager.instance.party[1];

       
        setDarkness(!Weather.instance.getIsDarkened(), 0.1f);
        setRain(!Weather.instance.getIsRaining(), 5f);
    }
}
