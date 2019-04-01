using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor_Script : MonoBehaviour {

    public bool goRight; // player can switch to left/right with the console linked to the conveyor object
    public float conveyorSpeed;

    private GameObject trashBall;
    private GameObject arrow;

	private void Start ()
    {
        arrow = transform.GetChild(0).gameObject;
        if (!arrow) Debug.LogError("There's supposed to be an 'arrow' object as a child of the conveyor!");
        if (!goRight)
            FlipArrow(false);
	}
	
	private void Update ()
    {
		if(trashBall)
        {
            Rigidbody2D ballRB = trashBall.GetComponent<Rigidbody2D>();
            ballRB.AddForce(new Vector2(ChoseDirection() * conveyorSpeed, 0));
        }
	}

    private int ChoseDirection()
    {
        if (goRight) return 1;
        else return -1;
    }

    public void FlipArrow(bool right) // Changes the size of the arrow to flip it
    {
        Vector3 currentArrowScale = arrow.transform.localScale;
        float xValue = currentArrowScale.x;
        float yValue = currentArrowScale.y;
        float zValue = currentArrowScale.z;
        /*if (!right)*/ xValue = -xValue;
        Vector3 newScale = new Vector3(xValue, yValue, zValue);
        arrow.transform.localScale = newScale;
    }
    
    #region CollsionEvents
	
    // For the moment, the conveyor belt reacts only to the Trashball
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trashball")
        {
            trashBall = collision.gameObject;
            trashBall.transform.parent = transform;
            Rigidbody2D trashBallRB = trashBall.GetComponent<Rigidbody2D>();
            trashBallRB.drag = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trashball")
        {
            Rigidbody2D trashBallRB = trashBall.GetComponent<Rigidbody2D>();
            trashBallRB.drag = 0.5f;
            trashBall.transform.parent = null;
            trashBall = null;
        }
    }
    #endregion
    
}
