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
    private bool typing;
    private float waitTime;
    private bool displayLineFull;

    public string[] dialogLines;

    private int currentLine;
    private Animator anim;

    // dialog box positions
    private float dialogDownPosition;
    private float dialogUpPosition;
    private float nameBoxUpPosition;
    private float nameBoxDownPosition;

    // determines if dialog boxes is on top or down
    private bool isUp;
    
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

        // typing speed
        waitTime = 0.02f;
        
        // have inactive by default
        nameBox.SetActive(false);

        // get animator component and initialize
        anim = GetComponent<Animator>();
        anim.SetBool("dialogBoxClose", true);

        // down position is default position for message window
        dialogDownPosition = dialogBox.transform.position.y;
        nameBoxDownPosition = nameBox.transform.position.y;
        
        // up position
        dialogUpPosition = dialogDownPosition * 4.57f;
        nameBoxUpPosition = nameBoxDownPosition * 3.14f;
    }

    // Update is called once per frame
    void Update()
    {
        // if user hits fire again, quit typing and just show the line
        if (typing && Input.GetButtonDown("Fire1"))
        {
            displayLineFull = true;
        }
        
        // position dialog box high or low
        if (!isUp)
        {
            dialogBox.transform.position = new Vector2(dialogBox.transform.position.x, dialogDownPosition);
            nameBox.transform.position = new Vector2(nameBox.transform.position.x, nameBoxDownPosition);
        }
        else
        {
            dialogBox.transform.position = new Vector2(dialogBox.transform.position.x, dialogUpPosition);
            nameBox.transform.position = new Vector2(nameBox.transform.position.x, nameBoxUpPosition);
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
                CheckSpecialCommands();
                
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

            // check for special commands
            CheckSpecialCommands();
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

    public void CheckSpecialCommands()
    {
        if (dialogLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogLines[currentLine].Replace("n-", "");
            currentLine++;
            
            // check for up or down
            if (dialogLines[currentLine] == ":up" || dialogLines[currentLine] == ":down")
            {
                isUp = dialogLines[currentLine] == ":up";
                currentLine++;
            }
            
        } else if (dialogLines[currentLine].StartsWith(":up"))
        {
            isUp = true;
            currentLine++;
        } else if (dialogLines[currentLine].StartsWith(":down"))
        {
            isUp = false;
            currentLine++;
        }
    }

    // getting and setters
    public bool getIsTyping() => typing;
}
