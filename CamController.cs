using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CamController : MonoBehaviour
{
    public static CamController mainCamera;
    private Transform target;
    
    private float height;
    private float width;

    public bool lockXPosition;
    public bool lockYPosition;

    public bool ignoreMapBoundsX;
    public bool ignoreMapBoundsY;
    private float boundX;
    private float boundY;

    public bool usingCamBounds;

    
    // Start is called before the first frame update
    void Start()
    {        
        // main cam should be singleton
        if (mainCamera == null)
        {
            mainCamera = this;
        }
        
        height = Camera.main.orthographicSize * 2;
        width = (height) * Camera.main.aspect;
        
        // change background color
        Camera.main.backgroundColor = new Color(0f, 0f, 0f);

    }


    void Update()
    {
    }

    // Late Update is called once per frame after Update. Good for cameras.
    void LateUpdate()
    {        
        // make sure there is a target
        if (target == null)
        {
            target = GameManager.instance.getControlTarget().gameObject.transform;
        }

        float x = lockXPosition ? transform.position.x : target.transform.position.x;
        float y = lockYPosition ? transform.position.y : target.transform.position.y;
        float z = transform.position.z;
        
        // control camera
        
        transform.position = new Vector3(x, y, z);

        
        // keep in bounds
        if (!ignoreMapBoundsX)
        {
            if (!usingCamBounds)
            {
                boundX = lockXPosition ? x : Mathf.Clamp(transform.position.x, (GameManager.instance.getBottomLeftLimit().x + (width / 2)),
                    GameManager.instance.getTopRightLimit().x - (width / 2));
            }
            else
            {
                boundX = lockXPosition ? x : Mathf.Clamp(transform.position.x, (GameManager.instance.getCamBottomLeftLimit().x + (width / 2)),
                    GameManager.instance.getCamTopRightLimit().x - (width / 2));
            }
            
        }
        else
        {
            boundX = x;
        }


        if (!ignoreMapBoundsY)
        {
            if (!usingCamBounds)
            {
                boundY = lockYPosition ? y : Mathf.Clamp(transform.position.y, GameManager.instance.getBottomLeftLimit().y + (height / 2),
                    GameManager.instance.getTopRightLimit().y - (height / 2));
            }
            else
            {
                boundY = lockYPosition ? y : Mathf.Clamp(transform.position.y, GameManager.instance.getCamBottomLeftLimit().y + (height / 2),
                    GameManager.instance.getCamTopRightLimit().y - (height / 2));
            }
            
        }
        else
        {
            boundY = y;
        }
        
    
    
        transform.position = new Vector3(boundX, boundY, z);
        
        
    }

    // getter
    public Transform getTarget() => target;
}
