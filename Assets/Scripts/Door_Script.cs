using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Script : MonoBehaviour {

    // Just need to lif the door
    // TODO : Make the door animation stoppable by obstacle
    
    public float doorSpeed;

    private Animator animController;

	private void Start ()
    {
        animController = GetComponent<Animator>();	
	}

    public void ChangeAnimState(bool raiseValue)
    {
        animController.SetBool("raise", raiseValue);
    }

}
