using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashMovement : MonoBehaviour
{
    [SerializeField]
    float speedX = 1f;
    [SerializeField]
    float waveSize = 1f; 
    [SerializeField]
    float waveSpeed = 1f;

    [SerializeField]
    LayerMask ground;

    Vector3 velocity;

    private void Awake()
    {
        velocity = new Vector3();

        float coinToss = Random.value;
        if(coinToss > 0.5)
        {
            speedX = -speedX;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localPosition = new Vector3(100f, Random.value * 13f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        velocity.y = Mathf.Sin(Time.time*waveSpeed)* waveSize;
        velocity.x = speedX;

        gameObject.transform.position += velocity*Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        RaycastHit2D hit;

        CircleCollider2D tempHolder = gameObject.GetComponent<CircleCollider2D>();

        // See if player bumped something horizontally
        if (speedX < 0)
        {
            hit = Physics2D.BoxCast(transform.position + (Vector3.left * 0.05f), new Vector2(tempHolder.radius, tempHolder.radius-0.05f), 0f, Vector2.left, 0.1f, ground);
        }
        else
        {
            hit = Physics2D.BoxCast(transform.position + (Vector3.right * 0.05f), new Vector2(tempHolder.radius, tempHolder.radius - 0.05f), 0f, Vector2.right, 0.1f, ground);
        }

        if(hit)
        {
            speedX = -speedX;
        }
    }
}
