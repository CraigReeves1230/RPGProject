using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    public GameObject fader;

    public GameObject messageWindow;

    public GameObject promptWindow;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(this);
        // make UI active
        fader.SetActive(true);
        messageWindow.SetActive(true);
        promptWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
