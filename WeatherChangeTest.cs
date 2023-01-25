using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherChangeTest : EventCutscene
{
    
    protected override void doCutscene()
    {
        setDarkness(!Weather.instance.getIsDarkened(), 0.1f);
        setRain(!Weather.instance.getIsRaining(), 5f);
        var vars = GameManager.instance.partyLead().player.customVariables;

        if (!Weather.instance.getIsRaining())
        {
            setCustomInt(vars, "formerTalkedToWizardVar", getCustomInt(vars, "talkedToWizard"));
            setCustomInt(vars, "talkedToWizard", 3);
        }
        else
        {
            if (getCustomInt(vars, "talkedToWizard") == 3)
            {
                setCustomInt(vars, "talkedToWizard", getCustomInt(vars, "formerTalkedToWizardVar"));
            }
        }
        
        setSceneDefaultWeather(!Weather.instance.getIsRaining(), Weather.instance.getIsFogging(),
            Weather.instance.getIsSnowing(), !Weather.instance.getIsDarkened());
        
        setSceneDefaultWeather(!Weather.instance.getIsRaining(), Weather.instance.getIsFogging(),
            Weather.instance.getIsSnowing(), !Weather.instance.getIsDarkened(), "TrialForest1");
    }
}
