using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorConsole_Script : MonoBehaviour {

    // This item controls the conveyors.
    // The player can switch the direction of the conveyor belts when he is overlapping the console.
    
    private bool canInteract; // The player can interact with the console only if he is overlapping it
    private bool switcher; // Store the direction of the conveyors
    public List<GameObject> conveyors = new List<GameObject>(); // One console can affect multiple conveyors
    private List<Conveyor_Script> convScripts = new List<Conveyor_Script>(); // In order to switch the direction of the conveyor this script needs to access the conveyor script

	private void Start ()
    {
        //Transform conveyorsGameObject = transform.parent.GetChild(1);
        foreach (GameObject conv in conveyors)
        {
            convScripts.Add(conv.GetComponent<Conveyor_Script>());
        }
        switcher = convScripts[0].goRight; // We take the value of the first conveyor in the list
	}
	
	private void Update ()
    {
        if (!InputManager.PressUp()) return;
        if(canInteract)
        {
            switcher = !switcher;
            foreach (Conveyor_Script convScript in convScripts)
            {
                convScript.goRight =switcher;
                convScript.FlipArrow(switcher);
            }
        }
    }

    #region CollisionEvents
    
    // Only player can interact with the console.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canInteract = false;
        }
    }

    #endregion
    
}
