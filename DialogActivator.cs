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

    public bool eventActivate()
    {
        var currentLine = 0;
        while (currentLine < lines.Length)
        {
            if (currentLine == 0)
            {
                DialogManager.instance.showDialog(lines);
                currentLine++;
            }
            else
            {
                if (GameManager.instance.getMainFireKeyUp())
                {
                    DialogManager.instance.showDialog(lines);
                    currentLine++;
                }
            }
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canActivateFromCollision && !DialogManager.instance.getIsTyping() && GameManager.instance.getMainFireKeyUp())
        {
            DialogManager.instance.showDialog(lines);
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
