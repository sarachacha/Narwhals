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
    Rigidbody2D rb;

    // Controls shit
    [Header("Controls")]
    public LayerMask IgnorePlayer;
    public LayerMask Ground;
    public bool grounded = false;

    float horizontalInput = 0f;
    float verticalInput = 0f;

    public float deadZone = 0.05f;

    // Speed shit
    [Header("Stick Movement Variables")]
    Vector3 velocity;
    public float minHorizontalSpeed = 0.5f;
    public float maxHorizontalSpeed = 20f;
    public float terminalVelocity = 50f;

    // Acceleration shit
    private float xAcceleration = 0f;
    //private float yAcceleration = 0f;

    public float maxGroundHorizontalAcceleration = 100f;
    public float maxAerialHorizontalDeceleration = 30f;
    public float maxAerialHorizontalAcceleration = 5f;
    public float smoothTime = 0.01f;

    // Jump shit
    [Header("Jump Variables")]
    //bool jump = false;
    public float jumpForce = 1f;

    public float upGravity = 5f;
    public float downGravity = 8f;


    private void Awake()
    {
        velocity = new Vector3();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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

        Movement(horizontalInput, verticalInput);

        if (Mathf.Abs(velocity.x) > minHorizontalSpeed)
        {
            transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;
            //rb.velocity = new Vector3(velocity.x, velocity.y);
        }
        else
        {
            transform.position += new Vector3(0, velocity.y) * Time.deltaTime;
            //rb.velocity = new Vector3(0, velocity.y);
        }
        //transform.position += new Vector3(0, velocity.y) * Time.deltaTime;

        // Don't do animating if there is nothing to animate
        if (spriteRenderer && animator)
        {
            SpriteAnimation();
        }

    }

    // Updates if swoop is on the ground
    private void CheckIfOnGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + (Vector3.down*0.01f), new Vector2(1.7f, 0.8f), 0f, Vector2.down, 0.05f, Ground);

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

        if (grounded)
        {
            if (velocity.y < -0.01f)
            {
                velocity.y = 0;
            }
        }
        else if (velocity.y > 0)
        {
            velocity += Vector3.down * upGravity * Time.deltaTime;
        }
        else
        {
            velocity += Vector3.down * downGravity * Time.deltaTime;
        }
    }

    // Updates Velocity with jumping
    private void Jump()
    {
        velocity.y += jumpForce;
    }

    // Updates animations
    private void SpriteAnimation()
    {
        if(Mathf.Abs(velocity.x) > minHorizontalSpeed)
        {
            //animator.SetBool("Running", true);
            animator.speed = Mathf.Abs(velocity.x) / maxHorizontalSpeed * 2;
        }
        else
        {
            //animator.SetBool("Running", false);
            //animator.speed = 2;
        }

        if (grounded ? velocity.x > deadZone : horizontalInput > deadZone)
        {
            spriteRenderer.flipX = true;
        }
        else if (grounded ? velocity.x < -deadZone : horizontalInput < -deadZone)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void Die()
    {
        StopMoving();
        GameController.instance.PlayerDie();
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
        //jump = context.performed;
        Jump();
    }

    public void StopMoving()
    {
        velocity = new Vector3(0, 0, 0);
    }


    //**********************************************************************************
    //**************************** Unity Events ****************************************
    //**********************************************************************************

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RaycastHit2D hit;

        // See if player bumped something horizontally
        if (spriteRenderer != null ? !spriteRenderer.flipX : velocity.x < 0)
        {
            hit = Physics2D.BoxCast(transform.position + (Vector3.left * 0.3f), new Vector2(1f, 0.75f), 0f, Vector2.left, 0.4f, IgnorePlayer);
        }
        else
        {
            hit = Physics2D.BoxCast(transform.position + (Vector3.right * 0.3f), new Vector2(1f, 0.75f), 0f, Vector2.right, 0.4f, IgnorePlayer);
        }

        if(hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                velocity.x = -velocity.x;
            }

            GivesPoints tempComponentHolder = hit.collider.gameObject.GetComponent<GivesPoints>();
            if(tempComponentHolder != null)
            {
                GameController.instance.GetPoints(tempComponentHolder.pointValue);
                GameController.instance.IncreaseMultiplier();

                //print(tempComponentHolder.gameObject.name);

                GameObject.Destroy(tempComponentHolder.gameObject);
            }
        }
        else
        {
            if(collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                Die();
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (velocity.y > 0)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(1.6f, 0.7f), 0f, Vector2.up, 0.07f, Ground);

            if (hit.collider != null)
            {
                velocity.y = 0;
            }
        }
    }
}
