using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Button_Script : MonoBehaviour {

    // This script is triggered on a button which is linked to a door.
    // The button reacts to different elements, the player or the weight of the trashball.
    // This element is supposed to be versatile (at least for prototyping purposes) 
    
    public bool playerOnly; // Toggle this boolean ON/OFF

    public float minWeightRequired; // This is the weight required to make the button run its script.


    public TextMeshPro massText;
    private Animator animController;

    //Door related variables
    private Door_Script doorScript;

	private void Start ()
    {
        GameObject door = transform.parent.parent.transform.GetChild(0).gameObject;
        doorScript = door.GetComponent<Door_Script>();
        animController = GetComponent<Animator>();
        massText.text = "Min : " + minWeightRequired + "kg";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag) // We want the button to react with only 2 elements, the player or the trashball
        {
            case "Player":
            {
                if(playerOnly) 
                {
                    ChangeAnimState(true);
                    doorScript.ChangeAnimState(true);
                }

                break;
            }
            case "Trashball":
            {
                GameObject trashBall = collision.gameObject;
                Rigidbody2D ballRB = trashBall.GetComponent<Rigidbody2D>();
                if(playerOnly)
                {
                    Debug.Log("The door can only be unlocked by the player");
                }
                else
                {
                    if(ballRB.mass>=minWeightRequired) //!\ !BE CAREFUL! This won't work with multiple balls /!\\
                    {
                        ChangeAnimState(true);
                        doorScript.ChangeAnimState(true);
                    }
                }

                break;
            }
            default:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // As the player/trashball leave the button, it returns to normal state
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
            {
                if(playerOnly)
                {
                    ChangeAnimState(false);
                    doorScript.ChangeAnimState(false);
                }

                break;
            }
            case "Trashball":
                if(!playerOnly)
                {
                    ChangeAnimState(false);
                    doorScript.ChangeAnimState(false);
                }
                ChangeAnimState(false);
                doorScript.ChangeAnimState(false);
                break;
            default:
                break;
        }
    }

    private void ChangeAnimState(bool pressValue)
    {
        animController.SetBool("press", pressValue);
    }
}
