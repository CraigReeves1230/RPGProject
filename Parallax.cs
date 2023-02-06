using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform cam;
    public Transform[] layers;
    private float[] parallaxScales;
    public float amount = 2f;
    private Vector3 previousCamPos;

    void Awake()
    {
        cam = Camera.main.transform;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        previousCamPos = cam.position;
        
        parallaxScales = new float[layers.Length];

        for (var i = 0; i < parallaxScales.Length; i++)
        {
            parallaxScales[i] = layers[i].position.z * -1;
            
        }
        
    }
    
    private void FixedUpdate()
    {
        for (var i = 0; i < layers.Length; i++)
        {
            float parallax_x = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            float parallax_y = (previousCamPos.y - cam.position.y) * parallaxScales[i];

            float backgroundTargetPosX = layers[i].position.x + parallax_x;
            float backgroundTargetPosY = layers[i].position.y + parallax_y;

            Vector3 backgroundTargetPosition =
                new Vector3(backgroundTargetPosX, backgroundTargetPosY, layers[i].position.z);

            layers[i].position = Vector3.Lerp(layers[i].position, backgroundTargetPosition,
                amount * Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}