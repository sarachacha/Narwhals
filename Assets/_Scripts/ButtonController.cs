using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonController : MonoBehaviour
{
    private float startingXPos = 0f;
    private float startingYPos = 0f;
    private float startingZPos = 0f;
    public float maxCompression = 0.2f;

    public float smoothTime = 0.1f;
    private float smoothVelocity = 0f;

    public float buttonInput = 0f;

    private void Start()
    {
        startingXPos = transform.localPosition.x;
        startingYPos = transform.localPosition.y;
        startingZPos = transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        float softCompress = Mathf.SmoothDamp(transform.localPosition.y, startingYPos - buttonInput * maxCompression, ref smoothVelocity, smoothTime);

        if (softCompress > startingYPos)
        {
            softCompress = startingYPos;
        }
        else if (softCompress < startingYPos - maxCompression)
        {
            softCompress = startingYPos - maxCompression;
        }

        Vector3 pos = new Vector3(startingXPos, softCompress, startingZPos);

        transform.localPosition = pos;

    }

    public void Press(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();

        buttonInput = input;
    }
}
