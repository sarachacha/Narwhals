using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoyStickController : MonoBehaviour
{
    public float maxTilt = 45f;

    public float smoothTime = 0.1f;
    private float smoothVelocityX = 0f;
    private float smoothVelocityZ = 0f;

    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    // Update is called once per frame
    void Update()
    {
        float xTilt = Mathf.SmoothDampAngle(transform.localRotation.eulerAngles.x, horizontalInput * maxTilt, ref smoothVelocityX, smoothTime);
        float zTilt = Mathf.SmoothDampAngle(transform.localRotation.eulerAngles.z, verticalInput * maxTilt, ref smoothVelocityZ, smoothTime);
        Quaternion tilt = Quaternion.Euler(xTilt, 0, zTilt);

        transform.localRotation = tilt;
    }

    public void Tilt(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        horizontalInput = -input.x;
        verticalInput = -input.y;
        
    }
}
