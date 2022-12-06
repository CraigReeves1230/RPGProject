using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    public Text dialogText;
    public Text nameText;
    public GameObject dialogBox;
    public GameObject nameBox;
    public bool typing;
    private float waitTime;
    private bool displayLineFull;

    public string[] dialogLines;

    public int currentLine;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        // should be a singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        waitTime = 0.02f;
        
        // have inactive by default
        nameBox.SetActive(false);

        // get animator component and initialize
        anim = GetComponent<Animator>();
        anim.SetBool("dialogBoxClose", true);        
    }

    // Update is called once per frame
    void Update()
    {
        // if user hits fire again, quit typing and just show the line
        if (typing && Input.GetButtonDown("Fire1"))
        {
            displayLineFull = true;
        }
    }

    public void showDialog(string[] newLines, bool showName = true)
    {
        nameBox.SetActive(showName);
        
        if (dialogBox.activeInHierarchy)
        {
            
            currentLine++;

            if (currentLine >= dialogLines.Length)
            {
                // close dialog box
                anim.SetBool("dialogBoxOpen", false);
                anim.SetBool("dialogBoxClose", true);
                
                nameBox.SetActive(false);
                currentLine = 0;
                dialogLines = null;
                nameText.text = null;
                GameManager.instance.getControlTarget().setCanMove(true);
            }
            else
            {
                // continue writing with dialog box still open
                CheckIfName();
                dialogText.text = dialogLines[currentLine];
                
                StopAllCoroutines();
                StartCoroutine(TypeSentence(dialogText.text));
            }
        }
        else
        {
            // dialog box first opens
            nameBox.SetActive(showName);
            dialogBox.SetActive(true);
            
            GameManager.instance.getControlTarget().setCanMove(false);
            dialogLines = newLines;
            currentLine = 0;
            CheckIfName();
            dialogText.text = dialogLines[currentLine]; 
            
            anim.SetBool("dialogBoxOpen", true);
            anim.SetBool("dialogBoxClose", false);
            
            StopAllCoroutines();
            StartCoroutine(TypeSentence(dialogText.text));
        }
    }
    
    private IEnumerator TypeSentence(string sentence)
    {
        typing = true;
        dialogText.text = "";

        int i = 0;
        while (!displayLineFull && i < sentence.Length)
        {
            dialogText.text += sentence[i];

            i++;
            yield return new WaitForSeconds(waitTime); 
        }

        dialogText.text = sentence;
        
        typing = false;
        displayLineFull = false;
        Input.ResetInputAxes();
    }

    public void CheckIfName()
    {
        if (dialogLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }
}
