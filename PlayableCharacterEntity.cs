using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Cache = UnityEngine.Cache;

public class PlayableCharacterEntity : ControllableEntity
{
    public PlayerObject player;
    
    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        
        // may have to be called during game, which is why it is a separate function
        init();
    }

    public void init()
    {
        DontDestroyOnLoad(gameObject);
        
        player.init();
    }

    protected void Update()
    {
        base.Update();
        
        // only characters in party are active
        gameObject.SetActive(Array.Exists(GameManager.instance.party, element => element == this));  
        
        // run button
        if (GameManager.instance.hasControl())
        {
            if (GameManager.instance.getSecondaryFireKeyDown())
            {
                isRunning = true;
            }
            if (GameManager.instance.getSecondaryFireKeyUp())
            {
                isRunning = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("WalkThrough"))
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
        }
    }

    
    
}
