using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayableCharacterEntity : ControllableEntity
{
    [Required]
    public PlayerObject player;
    
    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        
        // create graphics
        if (player.sprite != null)
        spriteRenderer.sprite = player.sprite;
        
        if (player.animator != null)
        anim.runtimeAnimatorController = player.animator;
        
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
        gameObject.SetActive(GameManager.instance.party.Contains(this));  
        
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
        
        // deal with sprite and animator changes   
        spriteRenderer.sprite = player.sprite;
        anim.runtimeAnimatorController = player.animator;        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("WalkThrough"))
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
        }
    }

    
    
}
