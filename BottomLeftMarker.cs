using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomLeftMarker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        GameManager.instance.bottomLeftLimit = new Vector2(transform.position.x, transform.position.y);
        GameManager.instance.setBottomLeftMarkerDetected(true);
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.instance.bottomLeftLimit = new Vector2(transform.position.x, transform.position.y);
        GameManager.instance.setBottomLeftMarkerDetected(true);
    }
}
