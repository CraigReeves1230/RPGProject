using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TestingForEquip : MonoBehaviour
{
    public EquipmentObject itemToEquip;
    private bool inZone;
    private PlayableCharacterEntity player;
    
    void Update()
    {
        if (inZone)
        {
            if (GameManager.instance.getMainFireKeyUp())
            {
                Debug.Log("fetching...");
                player.equip("Armor", itemToEquip);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (player == null)
            player = other.gameObject.GetComponent<PlayableCharacterEntity>();
            
            inZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D  other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (player == null)
            player = other.gameObject.GetComponent<PlayableCharacterEntity>();
            
            inZone = false;
        }
    }
}
