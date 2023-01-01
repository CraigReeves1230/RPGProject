using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptWindow : MonoBehaviour
{
    public static PromptWindow instance;
    public Text headingText;
    public Text option1;
    public Text option2;
    public Text option3;
    public Text option4;
    public RectTransform promptCursor;
    public int numOfOptions;
    public float cursorMovUnit = 51f;
    private Vector2 cursorHomePosition;
    
    // Start is called before the first frame update
    void Start()
    {
        // should be singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // default cursor position
        cursorHomePosition = new Vector2(promptCursor.localPosition.x, promptCursor.localPosition.y);
    }

    public void showPrompt(string headerText, params string[] options)
    {
        gameObject.SetActive(true);
        headingText.text = headerText;
        numOfOptions = options.Length;
        option1.text = (numOfOptions >= 1) ? options[0] : null;
        option2.text = (numOfOptions >= 2) ? options[1] : null;
        option3.text = (numOfOptions >= 3) ? options[2] : null;
        option4.text = (numOfOptions >= 4) ? options[3] : null;
    }

    public void closePromptWindow()
    {
        gameObject.SetActive(false);
        
        // reset cursor position
        promptCursor.localPosition = new Vector2(cursorHomePosition.x, cursorHomePosition.y);
    }
}
