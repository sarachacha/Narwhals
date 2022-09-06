using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    public float screenBounds = 12f;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > screenBounds)
        {
            transform.position = new Vector3(-screenBounds, transform.position.y);
        }
        else if (transform.position.x < -screenBounds)
        {
            transform.position = new Vector3(screenBounds, transform.position.y);
        }
    }
}
