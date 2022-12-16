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
            
            GameManager.instance.setNextDestination(new Vector2(position.x, position.y));         
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

    public void fadeInComplete(ControllableEntity regainControlOf = null)
    {
        if (GameManager.instance.getAutoReturnControl())
        {
            if (regainControlOf == null)
            {
                GameManager.instance.assignControl(GameManager.instance.partyLead());
            }
            else
            {
                GameManager.instance.assignControl(regainControlOf);
            }
        }
    }

    public void loadScene()
    {
        GameManager.instance.revokeControl();
        SceneManager.LoadScene(levelToLoad.handle);
    }
}
