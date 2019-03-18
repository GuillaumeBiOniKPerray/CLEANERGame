//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifterManager : MonoBehaviour {

    // This Lifter can lift both player and the trashball
    // It travels between 2 points:
    // - origin position
    // - destination position
    
    public float lifterSpeed;
    public float maxWeightAllowed;
    public TextMeshPro massText;

    private GameObject commandText;
    private Vector2 initialPosition;
    private Vector2 destinationPosition;
    private Vector2 movingTo;
    private bool leaveInitPos;
    private bool isMoving;
    private bool canInteract;

    public float massOnPlateform;

    private GameObject destinationPoint;


	private void Start ()
    {
        commandText = transform.GetChild(0).gameObject;
        Vector3 textScale = commandText.transform.localScale;
        if (transform.root.localScale.x < 0) commandText.transform.localScale = new Vector3(-textScale.x, textScale.y, textScale.z); // If you use the parent scale to flip the object, the text will not flip
        destinationPoint = transform.GetChild(1).gameObject;
        if (!destinationPoint) Debug.LogWarning("You have to set a destination point in order for " + gameObject.name + " to work.");
        destinationPosition = destinationPoint.transform.position;
        initialPosition = transform.parent.position;
    }
	
	private void Update ()
    {
        if(InputManager.PressUp())
        {
            if(canInteract)
            {
                if (massOnPlateform <= maxWeightAllowed) // The ball on the platform must not exceed a certain weight otherwise the platform won't lift.
                    MoveLifter();
            }
        }

        if (!isMoving) return;
        Transform parent = transform.parent;
        parent.position = Vector2.Lerp(parent.position, movingTo, Time.deltaTime * lifterSpeed);
        if(transform.position.y == movingTo.y -0.02f) //Moving Up!
        {
            isMoving = false;
        }
    }

    public void MoveLifter()
    {
        if(!leaveInitPos) // Means the platform is moving to the origin position
        {
            leaveInitPos = true;
            movingTo = destinationPosition;
            isMoving = true;
        }
        else if (leaveInitPos) // Means the platform is leaving the origin position
        {
            leaveInitPos = false;
            movingTo = initialPosition;
            isMoving = true;
        }
    }

    #region CollisionEvents

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player": // The player doesn't count as a weight
            {
                GameObject player = collision.gameObject;
                canInteract = true;
                commandText.SetActive(true);
                //massOnPlateform += player.GetComponent<Rigidbody2D>().mass;
                player.transform.parent = transform;
                break;
            }
            case "Trashball": // Each trashball adds a weight on the lifter
            {
                GameObject trashball = collision.gameObject;
                massOnPlateform = 0;
                massOnPlateform += trashball.GetComponent<Rigidbody2D>().mass;
//                Debug.Log("current mass on platform (at enter) : " + massOnPlateform);
                //massText.text = massOnPlateform + " / " + maxWeightAllowed;
                trashball.transform.parent = transform;
                break;
            }
            default: 
                break;    
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player": 
            {
                GameObject player = collision.gameObject;
                canInteract = false;
                commandText.SetActive(false);
                //massOnPlateform -= player.GetComponent<Rigidbody2D>().mass;
                player.transform.parent = null;
                break;
            }
            case "Trashball":
            {
                GameObject trashball = collision.gameObject;
                massOnPlateform -= trashball.GetComponent<Rigidbody2D>().mass;
                trashball.transform.parent = null;
                if (massOnPlateform < 0) massOnPlateform = 0;
//                Debug.Log("current mass on platform (at exit) : " + massOnPlateform);
                //massText.text = "Max : " + maxWeightAllowed + "kg";
                break;
            }
            default:
                break;
        }
    }

    #endregion
}
