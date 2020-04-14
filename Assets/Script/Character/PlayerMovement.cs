using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction
{
    North, East, South, West
}

public class PlayerMovement : MonoBehaviour {

    Direction currentDirection;
    Vector2 input;
    bool isMoving = false;
    bool canMove;
    Vector3 startPos;
    Vector3 endPos;
    float t;

    //Set in inspector
    public GameObject tilemap;
    public GameObject moveblock;
    public GameObject moveblockContainer;
    public float walkSpeed = 10f;

	// Use this for initialization
	void Awake () {
        canMove = true;

    }

    public IEnumerator Move(Transform entity)
    {
        isMoving = true;
        startPos = entity.position;
        t = 0;
        endPos = new Vector3(startPos.x + System.Math.Sign(input.x)*3.2f, 
            startPos.y + System.Math.Sign(input.y) * 3.2f, 
            startPos.z);

        
        if (CheckIfFreeSpace(endPos))
        {
            while (t < 1f)
            {
                t += Time.deltaTime * walkSpeed;
                entity.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }
            GameObject o = Instantiate(Resources.Load("MoveBlock"), startPos, Quaternion.identity) as GameObject;
            o.transform.parent = moveblockContainer.transform;
        }
        
        isMoving = false;
        yield return 0;
    }
	
	// Update is called once per frame
	void Update () {
		if(!isMoving && canMove)
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if(Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                input.y = 0;
            }
            else
            {
                input.x = 0;
            }

            if(input != Vector2.zero)
            {
                if(input.x < 0)
                {
                    currentDirection = Direction.West;
                }
                else if(input.x > 0)
                {
                    currentDirection = Direction.East;
                }
                else if(input.y < 0)
                {
                    currentDirection = Direction.South;
                }
                else if(input.y > 0)
                {
                    currentDirection = Direction.North;
                }
                StartCoroutine(Move(transform));
            }
        }
	}
    
    /// <summary>
    /// return false if occupied
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckIfFreeSpace(Vector3 pos)
    {
        foreach(Transform tr in moveblockContainer.transform)
        {
            if (tr.GetComponent<Collider2D>().OverlapPoint(pos))
            {
                return false;
            }
        }
        return !tilemap.GetComponent<Collider2D>().OverlapPoint(pos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Attack")
        {
            GameManager.instance.Death();
        }
        else if(collision.transform.tag == "EndPoint")
        {
            GameManager.instance.ReachedEndPoint();
        }
    }
    /// <summary>
    /// Use restart from GameManager
    /// </summary>
    public void Restart()
    {
        
        StartCoroutine(RestartRoutine(transform));
    }

    IEnumerator RestartRoutine(Transform entity)
    {
        while(isMoving)
        {
            yield return null;
        }
        canMove = false;
        foreach (Transform block in moveblockContainer.transform)
        {
            Destroy(block.gameObject);
        }
        entity.transform.position = GameManager.instance.lastStartPosition;
        t = 0;
        while(t < .3f)
        {
            t += Time.deltaTime;
            yield return null;
        }
        canMove = true;
        yield return 0;
    }

}
