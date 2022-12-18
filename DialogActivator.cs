using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    public string[] lines;
    private bool canActivateFromCollision;

    private bool triggeredByEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canActivateFromCollision && !DialogManager.instance.getIsTyping() && GameManager.instance.getMainFireKeyUp())
        {
            DialogManager.instance.showDialog(false, lines);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canActivateFromCollision = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canActivateFromCollision = false;
        }
    }

    
}
