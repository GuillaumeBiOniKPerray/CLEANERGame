using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : MonoBehaviour {

    //This platform is meant to travel between 2 destinations
    //There are 2 speed curves used by this platform ():
    // - linear
    // - Ease-In / Ease-Out
    // Use the "useLerp" boolean to toggle the second type of curve.
    
    // Destinations variables
    private GameObject[] destinations = new GameObject[2];
    public GameObject dest1;
    public GameObject dest2;
    private GameObject destObject;
    private Vector3 destination;
    private Vector3 roundedSavedPos;
    
    public bool useLerp;
    
    public float platSpeed;

    private bool isMoving;
    
	private void Start ()
    {
        // Initialization of the destinations 
        if (!dest1) Debug.LogWarning("Assign a Destination to 'dest1'!");
        if (!dest2) Debug.LogWarning("Assign a Destination to 'dest2'!");
        destinations[0] = dest1;
        destinations[1] = dest2;
        destObject = destinations[0];
        destination = destObject.transform.position;
        destination = RoundVector3(destination);
        isMoving = true;
	}
	
	private void Update ()
    {
        roundedSavedPos = transform.position;
        roundedSavedPos = RoundVector3(roundedSavedPos);

        if (isMoving && useLerp) // We use the Vector3.Lerp function in one case, to make the platform travel "smooth"
        {
            transform.position = Vector3.Lerp(transform.position, destination, Time.smoothDeltaTime * platSpeed); 
            if (roundedSavedPos == destination)
            {
                isMoving = false;
                ChangeDestination();
            }
        }

        if (!isMoving || useLerp) return; // Although, if the useLerp is false, we use the Vector3.MoveTowards function to make the platform travel linear
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.smoothDeltaTime * platSpeed);
        if (roundedSavedPos == destination)
        {
            isMoving = false;
            ChangeDestination();
        }
    }

    private void ChangeDestination() // There are 2 destinations to switch from
    {
        if (destObject != dest1) // It's either destination 1
        {
            destObject = dest1;
            destination = dest1.transform.position;
        }
        else // Or destination 2
        {
            destObject = dest2;
            destination = dest2.transform.position;
        }
        destination = RoundVector3(destination); // "destination" is always the point that the platform is moving toward to.
        isMoving = true; // The change on the destination also triggers the moving sequence.
    }

    private Vector3 RoundVector3(Vector3 vectorToRound) // We need to round the vector to make the transitions "smoothier".
    {
        vectorToRound = new Vector3(Mathf.Round(vectorToRound.x * 10)/ 10, Mathf.Round(vectorToRound.y* 10) / 10, Mathf.Round(vectorToRound.z* 10) / 10);
        return vectorToRound;
    }

    
    // Expand region to see events related to the collisions
    #region CollisionEvents

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag) // In order to make the player and the trashball follow the platform perfectly as it moves, we parent them when they step on it
        {
            case "Player":
            {
                GameObject player = collision.gameObject;
                player.transform.parent = transform;
                break;
            }
            case "Trashball":
            {
                GameObject trashball = collision.gameObject;
                trashball.transform.parent = transform;
                break;
            }
            default: 
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag) // As the player or the trashball leave the platform, we unparent them
        {
            case "Player":
            {
                GameObject player = collision.gameObject;
                player.transform.parent = null;
                break;
            }
            case "Trashball":
            {
                GameObject trashball = collision.gameObject;
                trashball.transform.parent = null;
                break;
            }
            default:
                break;
        }
    }

    #endregion   
}
