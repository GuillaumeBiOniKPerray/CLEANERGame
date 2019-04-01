using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Script : MonoBehaviour {

    // Just need to lif the door
    // TODO : Make the door animation stoppable by obstacle

    private Rigidbody2D rb;
    public float doorSpeed;
    public bool canMove;
    public GameObject destinationGameObject;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (canMove)
        {
            
            Vector3 currentPos = transform.position;
            Vector3 destPos = destinationGameObject.transform.position;
            transform.position = Vector3.Lerp(currentPos, destPos,Time.deltaTime * doorSpeed);
            float dist = Vector3.Distance(currentPos, destPos);
            if (dist <= 0.1f)
            {
                canMove = false;
            }
        }
        
    }

    public void DoorAction(bool isUp) // use the boolean in order to trigger the movement and rigidbody or not
    {
        canMove = isUp;
        rb.isKinematic = isUp;
    }

}
