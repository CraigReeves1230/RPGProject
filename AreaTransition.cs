using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransition : MonoBehaviour
{
    public string areaEntranceName;
    public bool ignoreX;
    public bool ignoreY;
    public string areaToLoad;
    public string toAreaEntrance;

    private AreaEntrance ae;
    private AreaExit aex;
    
    // Start is called before the first frame update
    void Start()
    {
        ae = gameObject.AddComponent<AreaEntrance>();
        aex = gameObject.AddComponent<AreaExit>();

        ae.entranceName = areaEntranceName;
        ae.ignoreX = ignoreX;
        ae.ignoreY = ignoreY;
        aex.areaEntrance = toAreaEntrance;
        aex.areaToLoad = areaToLoad;
    }

    // Update is called once per frame
    void Update()
    {
        ae.UpdatePlayerPosition(areaEntranceName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        aex.handleAreaExitCollision(other, areaEntranceName, areaToLoad);
    }

    private void OnTriggerExit(Collider other)
    {
        aex.handleAreaExitEndCollision();
    }
}
