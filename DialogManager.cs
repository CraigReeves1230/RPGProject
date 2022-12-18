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
    private bool triggeredByEvent;

    private bool dialogOpen;

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

        dialogOpen = false;
        
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
            if (typing && GameManager.instance.getMainFireKeyDown())
            {
                displayLineFull = true;
            }
        }
        
        // facebox image is required
        if (faceBoxImage == null)
        {
            GameManager.instance.errorMsg("FaceBox Image is missing from the Dialog Manager. Go to FaceBoxImage game object under the Facebox object and add it in the Dialog Manager.");
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

    
    public void showDialog(bool eventTriggered, string[] newLines)
    {
        if (dialogOpen)
        {
            if (triggeredByEvent)
            {
                currentLine = 0;
            }
            else
            {
                currentLine++;
            }
            
            if (currentLine >= dialogLines.Length && !triggeredByEvent)
            {
                closeDialog();
            }
            else
            {
                // continue displaying text
                if (triggeredByEvent)
                {
                    dialogLines = newLines;
                }

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
            // set if was triggered by event or not
            triggeredByEvent = eventTriggered;
            
            dialogBox.SetActive(true);
            dialogOpen = true;
            
            // by default, name and face boxes are not active
            nameBox.SetActive(false);
            faceBox.SetActive(false);
            
            dialogLines = newLines;
            currentLine = 0;
            
            GameManager.instance.revokeControl();

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
                        var next=  int.Parse(lineExplode[i + 1]);
                        if (faces.Length < 1 || faces[next] == null)
                        {
                            GameManager.instance.errorMsg("Face image not found. Make sure face is added to the Dialog Manager or that it is accessible.");
                        }
                        else
                        {
                            faceBox.SetActive(true);
                            faceBoxImage.sprite = faces[next];
                        }
                        break;
                    case ":close":
                        closeDialog();
                        break;
                }
            }

            currentLine++;
        }
    }

    public void closeDialog()
    {
        // close dialog box
        anim.SetBool("dialogBoxOpen", false);
        anim.SetBool("dialogBoxClose", true);
                
        nameBox.SetActive(false);
        currentLine = 0;
        dialogLines = null;
        nameText.text = null;
        faceBox.SetActive(false);
        dialogOpen = false;
        triggeredByEvent = false;
        GameManager.instance.restoreControl();
    }

    // getting and setters
    public bool getIsTyping() => typing;
}
