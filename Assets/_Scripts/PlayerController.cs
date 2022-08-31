using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Component References")]

    [SerializeField]
    Animator animator;
    [SerializeField]
    SpriteRenderer spriteRenderer;

    // Controls shit
    [Header("Controls")]
    public LayerMask CheckForGroundLayer;
    bool grounded = false;

    float horizontalInput = 0f;
    float verticalInput = 0f;

    public float deadZone = 0.05f;

    // Speed shit
    [Header("Stick Movement Variables")]
    Vector3 velocity;
    public float maxHorizontalSpeed = 20f;
    public float terminalVelocity = 50f;

    // Acceleration shit
    private float xAcceleration = 0f;
    private float yAcceleration = 0f;

    public float maxGroundHorizontalAcceleration = 100f;
    public float maxAerialHorizontalDeceleration = 30f;
    public float maxAerialHorizontalAcceleration = 5f;
    public float smoothTime = 0.01f;

    // Jump shit
    [Header("Jump Variables")]
    bool jump = false;
    public float jumpForce = 2f;

    float groundHeight = 0f;
    float jumpTime = 0f;

    public float upGravity = 10f;
    public float downGravity = 20f;


    private void Awake()
    {
        velocity = new Vector3();
    }

    private void Start()
    {
        //rb = GetComponent<Rigidbody2D>();

        // If no Animator is supplied, try to find one
        if(animator == null)
            animator = GetComponentInChildren<Animator>();

        // If no Sprite Renderer is supplied, try to find one
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfOnGround();

        Jump();

        Movement(horizontalInput, verticalInput);

        if(transform.position.x > 10f)
        {
            transform.position = new Vector3(-10f, transform.position.y);
        }
        else if (transform.position.x < -10f)
        {
            transform.position = new Vector3(10f, transform.position.y);
        }


        transform.position += velocity * Time.deltaTime;

        // Don't do animating is there is nothing to animate
        if (spriteRenderer && animator)
        {
            SpriteAnimation();
        }

    }

    // Updates if swoop is on the ground
    private void CheckIfOnGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(1.6f, 1.6f), 0f, Vector2.down, 0.07f, CheckForGroundLayer);

        if (hit.collider == null)
        {
            grounded = false;
        }
        else
        {
            grounded = true;
        }
    }

    // Updates Velocity with an x and y input
    private void Movement(float xInput, float yInput)
    {
        if (Mathf.Abs(xInput) > 0.01f)
        {
            if (grounded)
            {
                float targetHorizontalSpeed = xInput * maxHorizontalSpeed;
                float newXVel = Mathf.SmoothDamp(velocity.x, targetHorizontalSpeed, ref xAcceleration, smoothTime, maxGroundHorizontalAcceleration);
                velocity.x = newXVel;// = new Vector2(newXVel, velocity.y);
            }
            else
            {

                float targetHorizontalSpeed = velocity.x + xInput;
                targetHorizontalSpeed = Mathf.Clamp(targetHorizontalSpeed, -terminalVelocity, terminalVelocity);

                float newXVel = Mathf.SmoothDamp(velocity.x, targetHorizontalSpeed, ref xAcceleration, smoothTime, maxAerialHorizontalAcceleration);
                velocity.x = newXVel;// = new Vector2(newXVel, velocity.y);
            }
        }
    }

    // Updates Velocity with jumping
    private void Jump()
    {
        if (grounded)
        {
            groundHeight = transform.position.y;
            jumpTime = -1;
            velocity.y = 0;
        }
        else if(velocity.y > 0)
        {
            velocity += Vector3.down * upGravity * Time.deltaTime;
        }
        else
        {
            velocity += Vector3.down * downGravity * Time.deltaTime;
        }

        if (grounded && jump && velocity.y < 0.01f)
        {
            velocity.y = jumpForce;
        }

        //velocity += Vector3.down * upGravity * Time.deltaTime;
    }

    // Updates animations
    private void SpriteAnimation()
    {
        if(Mathf.Abs(horizontalInput) > deadZone)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }

        if(velocity.x > deadZone)
        {
            spriteRenderer.flipX = false;
        }
        else if(velocity.x < -deadZone)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        horizontalInput = input.x;
        verticalInput = input.y;
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        //rb.AddForce(Vector2.up * jumpForce);
        jump = context.performed;
    }
}
