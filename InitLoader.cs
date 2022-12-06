using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitLoader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject UIScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
        
        if (UIFade.instance == null)
        {
            Instantiate(UIScreen);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
