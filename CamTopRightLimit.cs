using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTopRightLimit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        GameManager.instance.setCamTopRightLimit(new Vector2(transform.position.x, transform.position.y));
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.instance.setCamTopRightLimit(new Vector2(transform.position.x, transform.position.y));
    }
}
