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
    private int numOfOptions;

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

    private DisplayStyleOptions displayStyle;

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
        anim.SetBool("MessageWindowOpened", false);
        

        // down position is default position for message window
        dialogDownPosition = dialogBox.transform.position.y;
        nameBoxDownPosition = nameBox.transform.position.y;
        faceBoxDownPosition = faceBox.transform.position.y;

        // dialog is closed by default
        dialogOpen = false;
        
        // get display style
        displayStyle = GameManager.instance.gameDatabase.messageWindowStyle;
        
        // up position
        dialogUpPosition = dialogDownPosition * 4.57f;
        nameBoxUpPosition = nameBoxDownPosition * 3.14f;
        faceBoxUpPosition = faceBoxDownPosition * 4.66f;
    }

    
    // Update is called once per frame
    void Update()
    {
        // get faces
        if (GameManager.instance.getIfUsingFaces())
        {
            if (faces.Length < 1)
            {
                faces = GameManager.instance.getFaces();
            }
        }
        
        
        // in traditional display mode, if user hits fire again, quit typing and just show the line
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

    
    public void showDialog(string[] newLines)
    {
        if (dialogOpen)
        {
            // reset line counter
            currentLine = 0;
            
            // continue displaying text if dialog box already open        
            dialogLines = newLines;
            

            // check for commands to put up either face box or name box
            CheckSpecialCommands();
            
            dialogText.text = dialogLines[currentLine];
            
            // type out text if in traditional display mode
            if (displayStyle == DisplayStyleOptions.Traditional)
            {
                StopAllCoroutines();
                StartCoroutine(TypeSentence(dialogText.text));
            }
            
        }
        else
        {
            // dialog box opening for the first time
            
            dialogBox.SetActive(true);
            dialogOpen = true;
            
            // by default, name and face boxes are not active
            nameBox.SetActive(false);
            faceBox.SetActive(false);
            
            dialogLines = newLines;
            currentLine = 0;


            // check for commands to put up either face box or name box
            CheckSpecialCommands();
            
            dialogText.text = dialogLines[currentLine]; 
            
            anim.SetBool("MessageWindowOpened", true);

            // type out text if in traditional display mode
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

    // checks for special commands
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
                        if (string.Equals(lineExplode[i + 1], "none", StringComparison.OrdinalIgnoreCase))
                        {
                            faceBox.SetActive(false);
                        }
                        else
                        {
                            var next = int.Parse(lineExplode[i + 1]);
                            if (faces.Length < 1 || faces[next] == null)
                            {
                                GameManager.instance.errorMsg("Face image not found. Make sure face is added to the Game Database or that it is accessible.");
                            }
                            else
                            {
                                faceBox.SetActive(true);
                                faceBoxImage.sprite = faces[next];
                            } 
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

    // closes dialog box
    public void closeDialog()
    {
        anim.SetBool("MessageWindowOpened", false);
                
        nameBox.SetActive(false);
        currentLine = 0;
        dialogLines = null;
        nameText.text = null;
        faceBox.SetActive(false);
        dialogOpen = false;
    }
}
