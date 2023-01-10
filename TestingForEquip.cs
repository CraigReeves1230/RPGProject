using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingForEquip : MonoBehaviour
{
    public EquipmentObject itemToEquip;
    

    private void OnCollisionEnter2D(Collision2D player)
    {
        if (GameManager.instance.getMainFireKeyUp())
        {
            var playerEntity = player.gameObject.GetComponent<PlayableCharacterEntity>();
            playerEntity.equip("RightHand", itemToEquip);
        }
    }
}
