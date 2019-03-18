using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifterConsoleManager : MonoBehaviour {

    //This lifter can only lift the player
    
    public LifterManager lifterManager;

    private GameObject commandText; // Hint is showing when player overlaps the lifter
    private bool canInteract;

    private void Start()
    {
        commandText = transform.GetChild(0).gameObject;
        Vector3 textScale = commandText.transform.localScale;
        if (transform.root.localScale.x < 0) commandText.transform.localScale = new Vector3(-textScale.x, textScale.y, textScale.z);
    }

    private void Update()
    {
        if (!InputManager.PressUp()) return; //The player can only interact with the lifter by pressing the up arrow when he is overlapping the lifter
        if(canInteract)
        {
            if (lifterManager.massOnPlateform <= lifterManager.maxWeightAllowed)
                lifterManager.MoveLifter();
        }
    }

    //Toggle the CollisionEvents region to see what happens in the related events
    #region CollisionEvents
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            commandText.SetActive(true);
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            commandText.SetActive(false);
            canInteract = false;
        }
    }

    #endregion
    
}
