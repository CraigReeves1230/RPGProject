using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayableCharacter : MovingEntity
{
    public bool solidCollider = false;
    private EventSequence eventSequence;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        if (solidCollider)
        {
            gameObject.tag = "Player";
            gameObject.GetComponent<Rigidbody2D>().mass = 1000000;
        }
    }
    
}
