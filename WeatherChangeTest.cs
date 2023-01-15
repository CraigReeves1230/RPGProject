using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherChangeTest : EventCutscene
{
    
    protected override void doCutscene()
    {
        setDarkness(!Weather.instance.getIsDarkened(), 0.1f);
        setRain(!Weather.instance.getIsRaining(), 5f);

        if (!Weather.instance.getIsRaining())
        {
            setGameWorldVar("formerTalkedToWizardVar", getGameWorldVar("talkedToWizard"));
            setGameWorldVar("talkedToWizard", 3);
        }
        else
        {
            if (getGameWorldVar("talkedToWizard") == 3)
            {
                setGameWorldVar("talkedToWizard", getGameWorldVar("formerTalkedToWizardVar"));
            }
        }
        
        setSceneDefaultWeather(!Weather.instance.getIsRaining(), Weather.instance.getIsFogging(),
            Weather.instance.getIsSnowing(), !Weather.instance.getIsDarkened());
        
        setSceneDefaultWeather(!Weather.instance.getIsRaining(), Weather.instance.getIsFogging(),
            Weather.instance.getIsSnowing(), !Weather.instance.getIsDarkened(), "TrialForest1");
    }
}
