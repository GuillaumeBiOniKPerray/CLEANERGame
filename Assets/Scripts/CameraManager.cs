using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    
    // The objective of this script is make the camera feel smooth when the player is playing
    // The camera can also move to destinations at the beginning of the level
    
    public GameObject player;
    private float playerYPos;
    private float cameraYpos;

    public bool isMenu;
    public bool isReadyToMove; // This boolean is toggled by the GameController. it makes the cam reach destinations so that the player discovers the level before playing it
    public float camSpeed; //The speed of the camera
    
    private List<GameObject> camDestinations = new List<GameObject>(); // The list of destination points
    private int camDestIndex;

    public GameController gameController;

    private void Update ()
    {
        if (isMenu) return;
        
        if(isReadyToMove)
        {
            if (InputManager.PressSpace())
            {
                CancelCamMove();
            }
            MoveToNewPosition();
        }
        else // After moving to the different level destinations the camera gets back to the player
        {
            cameraYpos = transform.position.y;
            if (!Camera.main) return;
            Vector3 playViewportPos = Camera.main.WorldToViewportPoint(player.transform.position);
            CenterOnTarget(playViewportPos);
        }
    }

    public void AssignCameraPosition() // Makes the camera reach the player with an offset.
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z - 2.5f);
    }

    private void CenterOnTarget(Vector3 targetPos) //The camera focuses the target when he goes too low or too high
    {
        playerYPos = targetPos.y; // playerYpos is the position of the target relative to the screenspace.
        if (playerYPos <= 0.2f) // if the target goes to low, the camera switches back to a new height
        {
            cameraYpos -= 1f;
        }
        else if (playerYPos >= 0.8f) // if the target goes to high, the camera switches back to a new height
        {
            cameraYpos += 1f;
        }
        Vector3 newPos = new Vector3(player.transform.position.x, cameraYpos, player.transform.position.z - 2.5f);
        transform.position = newPos;
    }

    private void MoveToNewPosition()
    {
        // TODO : Learn Lerp!
        if (camDestIndex < camDestinations.Count)
        {
            if (!camDestinations[camDestIndex]) return;
            Vector3 currDestination = camDestinations[camDestIndex].transform.position;
            Vector3 currDestinationWithOffset = new Vector3(currDestination.x, currDestination.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, currDestinationWithOffset, Time.deltaTime*camSpeed);
            float dist = Vector3.Distance(transform.position, currDestinationWithOffset);
            if (dist <= 1.2f) // In order to make the camera switch faster to the new destination, we add an offset to the desired destination's position
            {
                camDestIndex++;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime*camSpeed);
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist <= 1.2f)
            {
                isReadyToMove = false;
                gameController.playerController.state = PlayerController.PlayerState.PLAYING;
            }
        }
    }

    private void CancelCamMove()
    {
        camDestinations.Clear();
        transform.position = player.transform.position;
        isReadyToMove = false;
        gameController.playerController.state = PlayerController.PlayerState.PLAYING;
    }

    public void FillDestinationList(List<GameObject> destinations) // The destination objects are stored in the LevelManager Script and this function allows us to get them
    {
        if (camDestinations.Count > 0) camDestinations.Clear();
        foreach (GameObject dest in destinations)
        {
            camDestinations.Add(dest);
//            Debug.Log("Destination : " + dest);
        }
        camDestIndex = 0;
    }
}
