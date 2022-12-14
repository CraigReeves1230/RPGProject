using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllableEntity : MovingEntity
{    
    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
       
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();

        // control entity if it is targeted to be controlled
        if (GameManager.instance.isControlTarget(this) && !isFollowing)
        {
            
            // give Player tag to controllable entity
            if (!gameObject.CompareTag("Player"))
            {
                gameObject.tag = "Player";
            }
            
            horizontalMov = Input.GetAxisRaw("Horizontal");
            verticalMov = Input.GetAxisRaw("Vertical");
        }        
    }
}
