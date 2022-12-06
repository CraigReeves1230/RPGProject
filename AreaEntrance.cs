using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AreaEntrance : MonoBehaviour
{
    public string entranceName;
    public bool ignoreX;
    public bool ignoreY;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerPosition(entranceName);
    }
    
    // Update player position
    public void UpdatePlayerPosition(string inEntranceName)
    {
        if (GameManager.instance.nextAreaEntrance == inEntranceName)
        {
            // new entity positions
            var newX = !ignoreX ? gameObject.transform.position.x : GameManager.instance.getControlTarget().gameObject.transform.position.x;
            var newY = !ignoreY ? gameObject.transform.position.y : GameManager.instance.getControlTarget().gameObject.transform.position.y;
            
            GameManager.instance.exitsEnabled = false;
            GameManager.instance.getControlTarget().gameObject.transform.position = new Vector2(newX, newY);
            GameManager.instance.nextAreaEntrance = null;

            GameManager.instance.getControlTarget().setCanMove(true);
            
            UIFade.instance.FadeFromBlack();
        }
    }
}
