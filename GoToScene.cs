using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    private Scene levelToLoad;
    private Vector2 position;
    private bool isEvent;
    
    protected void Start()
    {
    }
    
    public void go()
    {
        if (GameManager.instance.getControlTarget() != null || isEvent)
        {
            GameManager.instance.revokeControl();
            
            GameManager.instance.getControlTarget().clearDirectionalBuffer();
            
            GameManager.instance.setNextDestination(position.x, position.y);         
        }
    }

    public void setAsEvent(bool setting)
    {
        isEvent = setting;
    }

    public void setLevelToLoad(Scene levelToLoad)
    {
        this.levelToLoad = levelToLoad;
    }

    public void setPosition(float x, float y)
    {
        position = new Vector2(x, y);
    }

    public void fadeInComplete()
    {
        if (gameWorld.getAutoReturnControl())
        {
            gameWorld.partyLeader().setControlOverride(false);  
        }
    }

    public void loadScene()
    {
        gameWorld.partyLeader().setControlOverride(true);
        SceneManager.LoadScene(levelToLoad.handle);
    }
}
