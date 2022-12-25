using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class NPCDialog : EventSequence
{
    private MovementSequence ms;
    private MovingEntity character;
    public bool turnToFacePlayer = true;

    private void Start()
    {
        base.Start();
        ms = gameObject.GetComponent<MovementSequence>();
        character = gameObject.GetComponent<MovingEntity>();
    }

    protected abstract void doDialog();

    public override void run()
    {
        if (ms != null)
        {
            remotePause(ms);
        }
        
        stealControl();

        if (turnToFacePlayer)
        {
            character.turnToFace(GameManager.instance.getControlTarget().gameObject);
        }
        
        doDialog();
        
        returnControl();
        remoteResumeSeq(ms);
    }
}
