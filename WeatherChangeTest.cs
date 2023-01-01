using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherChangeTest : EventCutscene
{
    
    protected override void doCutscene()
    {
        /*setDarkness(!Weather.instance.getIsDarkened(), 0.1f);
        setRain(!Weather.instance.getIsRaining(), 5f);
        
        setSceneDefaultWeather(!Weather.instance.getIsRaining(), Weather.instance.getIsFogging(),
            Weather.instance.getIsSnowing(), !Weather.instance.getIsDarkened());
        
        setSceneDefaultWeather(!Weather.instance.getIsRaining(), Weather.instance.getIsFogging(),
            Weather.instance.getIsSnowing(), !Weather.instance.getIsDarkened(), "TrialForest1");*/
        
        msg(":face 0 :name Blondie", "Here we go.");
        wait();
        msgCls();
        promptWin("What do you want to do?", "Go high!", "Go low!", "Go in the middle!", "Don't do anything...");
        waitForPrompt((choice) =>
        {
            switch (choice)
            {
                case 1:
                    msg(":face 1 :name Baldman", "Alright, looks like we're going high. Good choice!");
                    wait();
                    msgCls();
                    break;
                case 2:
                    msg(":face 1 :name Baldman", "Alright, looks like we're going low. Not bad!");
                    wait();
                    msgCls();
                    break;
                case 3:
                    msg(":face 1 :name Baldman", "Playing it safe, eh? OK, I see ya!");
                    wait();
                    msgCls();
                    break;
                case 4:
                    msg(":face 1 :name Baldman", "Oh come on, you have to pick something!");
                    wait();
                    msgCls();
                    break;
            }
        });
    }
}
