using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallMovement : MonoBehaviour {

    //public float speed = 10f;
    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody2D>();
        //rb.velocity = Vector2.right * speed;
        //LauchAtSpeed(100);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (transform.position.y >= GameManager.yLimit)
        {
            rb.velocity = new Vector2(rb.velocity.x, -Mathf.Abs(rb.velocity.y));
        }
        else if (transform.position.y <= -GameManager.yLimit)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(rb.velocity.y));
        }
        if (transform.position.x >= GameManager.xLimit)
        {
            rb.velocity = new Vector2(-Mathf.Abs(rb.velocity.x),rb.velocity.y);
        }
        else if(transform.position.x <= -GameManager.xLimit)
        {
            rb.velocity = new Vector2(Mathf.Abs(rb.velocity.x), rb.velocity.y);
        }
    }

    /// <summary>
    /// Makes the fire ball move, will generate a random direction
    /// </summary>
    /// <param name="speed"></param>
    public void LauchAtSpeed(float speed)
    {
        rb.velocity = new Vector2(GameManager.NextFloat(-1, 1), GameManager.NextFloat(-1, 1)).normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Wall")
        {
            

            /*
            // Collision side detection with distance between position.
            float cx = transform.position.x - collision.transform.position.x; // negative == left, positive == right
            float cy = transform.position.y - collision.transform.position.y;
            if (Mathf.Abs(cx) > Mathf.Abs(cy))
            { // collision is left or right
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
                
                if (cx > 0)
                { // left
                    rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
                }
                else
                { // right

                }
            }
            else
            { // collision is up or down
                rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
                /*
                if(cy > 0)
                { // up

                }
                else
                { // down

                }
            }*/
        }
    }

}
