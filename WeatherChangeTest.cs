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
            setGameWorldString("formerTalkedToWizardVar", getGameWorldString("talkedToWizard"));
            setGameWorldString("talkedToWizard", "rain");
        }
        else
        {
            if (getGameWorldString("talkedToWizard") == "rain")
            {
                setGameWorldString("talkedToWizard", getGameWorldString("formerTalkedToWizardVar"));
            }
        }
        
        setSceneDefaultWeather(!Weather.instance.getIsRaining(), Weather.instance.getIsFogging(),
            Weather.instance.getIsSnowing(), !Weather.instance.getIsDarkened());
        
        setSceneDefaultWeather(!Weather.instance.getIsRaining(), Weather.instance.getIsFogging(),
            Weather.instance.getIsSnowing(), !Weather.instance.getIsDarkened(), "TrialForest1");
    }
}
