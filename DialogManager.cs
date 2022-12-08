using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    public Text dialogText;
    public Text nameText;
    public GameObject dialogBox;
    public GameObject nameBox;
    public GameObject faceBox;
    public Image faceBoxImage;
    private bool typing;
    private float waitTime;
    private bool displayLineFull;

    public Sprite[] faces;

    private string[] dialogLines;

    private int currentLine;
    private Animator anim;

    public enum DisplayStyleOptions
    {
        Modern,
        Traditional
    }

    public DisplayStyleOptions displayStyle;

    // dialog box positions
    private float dialogDownPosition;
    private float dialogUpPosition;
    private float nameBoxUpPosition;
    private float nameBoxDownPosition;
    private float faceBoxUpPosition;
    private float faceBoxDownPosition;
    

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
        faceBox.SetActive(false);

        // get animator component and initialize
        anim = GetComponent<Animator>();
        anim.SetBool("dialogBoxClose", true);

        // down position is default position for message window
        dialogDownPosition = dialogBox.transform.position.y;
        nameBoxDownPosition = nameBox.transform.position.y;
        faceBoxDownPosition = faceBox.transform.position.y;
        
        // up position
        dialogUpPosition = dialogDownPosition * 4.57f;
        nameBoxUpPosition = nameBoxDownPosition * 3.14f;
        faceBoxUpPosition = faceBoxDownPosition * 4.67f;
    }

    
    // Update is called once per frame
    void Update()
    {
        // in classic display mode, if user hits fire again, quit typing and just show the line
        if (displayStyle == DisplayStyleOptions.Traditional)
        {
            if (typing && Input.GetButtonDown("Fire1"))
            {
                displayLineFull = true;
            }
        }
        
        
        // position dialog box high or low
        if (!isUp)
        {
            dialogBox.transform.position = new Vector2(dialogBox.transform.position.x, dialogDownPosition);
            nameBox.transform.position = new Vector2(nameBox.transform.position.x, nameBoxDownPosition);
            faceBox.transform.position = new Vector2(faceBox.transform.position.x, faceBoxDownPosition);
        }
        else
        {
            dialogBox.transform.position = new Vector2(dialogBox.transform.position.x, dialogUpPosition);
            nameBox.transform.position = new Vector2(nameBox.transform.position.x, nameBoxUpPosition);
            faceBox.transform.position = new Vector2(faceBox.transform.position.x, faceBoxUpPosition);
        }
        
    }

    
    public void showDialog(string[] newLines, bool showName = true)
    {        
        // show face image
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
                faceBox.SetActive(false);
                GameManager.instance.getControlTarget().setCanMove(true);
            }
            else
            {
                // continue displaying text
                
                
                // check for commands to put up either face box or name box
                CheckSpecialCommands();
                
                dialogText.text = dialogLines[currentLine];
                
                // type out text if in classic display mode
                if (displayStyle == DisplayStyleOptions.Traditional)
                {
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(dialogText.text));
                }
            }
        }
        else
        {
            // dialog box first opens
            dialogBox.SetActive(true);
            
            // by default, name and face boxes are not active
            nameBox.SetActive(false);
            faceBox.SetActive(false);
            
            GameManager.instance.getControlTarget().setCanMove(false);
            dialogLines = newLines;
            currentLine = 0;

            // check for commands to put up either face box or name box
            CheckSpecialCommands();
            
            dialogText.text = dialogLines[currentLine]; 
            
            anim.SetBool("dialogBoxOpen", true);
            anim.SetBool("dialogBoxClose", false);

            if (displayStyle == DisplayStyleOptions.Traditional)
            {
                StopAllCoroutines();
                StartCoroutine(TypeSentence(dialogText.text));
            }
        }
    }
    
    
    // types out text in classic display mode
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
        bool specialCommands = dialogLines[currentLine].Contains(":up") 
            || dialogLines[currentLine].Contains(":down") 
           || dialogLines[currentLine].Contains(":name")
            || dialogLines[currentLine].Contains(":face");
        
        // if there are special commands, treat as a command line
        if (specialCommands)
        {
            var lineExplode = dialogLines[currentLine].Split(" ");
            for (int i = 0; i < lineExplode.Length; i++) 
            {
                switch (lineExplode[i])
                {
                    case ":up":
                        isUp = true;
                        break;
                    case ":down":
                        isUp = false;
                        break;
                    case ":name":
                        if (string.Equals(lineExplode[i+1], "none", StringComparison.OrdinalIgnoreCase))
                        {
                            nameBox.SetActive(false);
                        }
                        else
                        {
                            nameBox.SetActive(true);
                            nameText.text = lineExplode[i + 1];
                        }           
                        break;
                    case ":face":
                        if (string.Equals(lineExplode[i+1], "none", StringComparison.OrdinalIgnoreCase))
                        {
                            faceBox.SetActive(false);
                        }
                        else
                        {
                            faceBox.SetActive(true);
                            faceBoxImage.sprite = faces[int.Parse(lineExplode[i + 1])];
                        }
                        break;
                }
            }

            currentLine++;
        }
    }

    // getting and setters
    public bool getIsTyping() => typing;
}
