using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TreasureBox : EventSequence
{
    private Animator anim;

    [Required] public string uniqueHandle;
    
    public ItemObject itemGiven;
    public int quantityGiven;

    void Start()
    {
        base.Start();
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        base.Update();
        
        anim.SetBool("opened", GameManager.instance.gameDatabase.openedTreasureBoxes.Contains(uniqueHandle));
        
    }
    

    public override void run()
    {
        if (!GameManager.instance.gameDatabase.openedTreasureBoxes.Contains(uniqueHandle))
        {
            stealControl();
            anim.SetBool("opened", true);
            GameManager.instance.gameDatabase.openedTreasureBoxes.Add(uniqueHandle);
            delay(0.5f);
        
            OnOpen();
            
            returnControl(); 
        }  
    }

    protected void OnOpen()
    {
        if (itemGiven != null && quantityGiven > 0)
        {
            giveItem(itemGiven.handle, quantityGiven);

            if (quantityGiven == 1)
            {
                msg("Received " + itemGiven.name + ".");
            }
            else
            {
                msg("Received " + quantityGiven + " " + itemGiven.name + ".");
            }
        
            wait();
            msgCls();
        }
    }
}
