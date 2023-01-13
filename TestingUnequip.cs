using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingUnequip : MonoBehaviour
{
    public EquipmentObject itemToEquip;
    private bool inZone;
    public PlayableCharacterEntity baldman;
    public PlayableCharacterEntity blondie;
    
    void Update()
    {
        if (inZone)
        {
            if (GameManager.instance.getMainFireKeyUp())
            {
                blondie.unEquip("RightHand");
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D  other)
    {
        if (other.gameObject.CompareTag("Player"))
        {            
            inZone = false;
        }
    }
}
