using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    public string areaToLoad;
    public string areaEntrance;
    private Collider2D other;
    
    public float waitToLoad = 1f;
    private bool shouldLoadAfterFade;
    
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0)
            {
                SceneManager.LoadScene(areaToLoad);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collidingObject)
    {
        handleAreaExitCollision(collidingObject, areaEntrance, areaToLoad);
    }

    private void OnTriggerExit2D(Collider2D collidingObject)
    {
        if (collidingObject.CompareTag("Player"))
        {
            // re-enable exists
            handleAreaExitEndCollision();
        }
    }

    
    public void handleAreaExitCollision(Collider2D coll, string theAreaEntranceName, string theAreaToLoad)
    {
        if (coll.CompareTag("Player") && GameManager.instance.getExitsEnabled())
        {
            GameManager.instance.setNextAreaEntrance(theAreaEntranceName);
            areaToLoad = theAreaToLoad;
            shouldLoadAfterFade = true;
            GameManager.instance.revokeControl();
            UIFade.instance.FadeToBlack();
        }
    }

    public void handleAreaExitEndCollision()
    {
        GameManager.instance.setExistEnabled(true);
    }
    
    
}
