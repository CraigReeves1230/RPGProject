using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    public string entranceName;
    public bool ignoreX;
    public bool ignoreY;
    public float fadeInWaitTime = 1f;
    
    
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
        if (GameManager.instance.getNextAreaEntrance() == inEntranceName)
        {
            var newX = !ignoreX ? gameObject.transform.position.x : GameManager.instance.getControlTarget().gameObject.transform.position.x;
            var newY = !ignoreY ? gameObject.transform.position.y : GameManager.instance.getControlTarget().gameObject.transform.position.y;
            
          
            GameManager.instance.setExitsEnabled(false);
            
            // new entity positions
            var party = GameManager.instance.party;
            for (int i = 0; i < party.Count; i++)
            {
                party[i].gameObject.transform.position = new Vector2(newX, newY);
                
                // get facing position of player in front and turn player
                if (i > 0)
                {
                    var lastMoveX = party[i - 1].GetComponent<Animator>().GetBool("lastMoveX");
                    var lastMoveY = party[i - 1].GetComponent<Animator>().GetBool("lastMoveY");
                    
                    party[i].GetComponent<Animator>().SetBool("lastMoveX", lastMoveX);
                    party[i].GetComponent<Animator>().SetBool("lastMoveY", lastMoveY);
                }
            }
            
            GameManager.instance.setNextAreaEntrance(null);

            StopAllCoroutines();
            StartCoroutine(restoreControlAndFadeIn());
            
        }
    }

    IEnumerator restoreControlAndFadeIn()
    {
        yield return new WaitForSeconds(fadeInWaitTime);
        GameManager.instance.restoreControl();
        UIFade.instance.FadeFromBlack();
    }
}
