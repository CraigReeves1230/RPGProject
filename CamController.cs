using System;
using System.Collections;
using System.Collections.Generic;
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

    private Animator anim;

    
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

        // get animator
        anim = GetComponent<Animator>();
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
        float boundX = lockXPosition ? x : Mathf.Clamp(transform.position.x, (GameManager.instance.bottomLeftLimit.x + (width / 2)),
            GameManager.instance.topRightLimit.x - (width / 2));
        float boundY = lockYPosition ? y : Mathf.Clamp(transform.position.y, GameManager.instance.bottomLeftLimit.y + (height / 2),
            GameManager.instance.topRightLimit.y - (height / 2));
    
    
        transform.position = new Vector3(boundX, boundY, z);
        
        
    }

    // getter
    public Transform getTarget() => target;
}
