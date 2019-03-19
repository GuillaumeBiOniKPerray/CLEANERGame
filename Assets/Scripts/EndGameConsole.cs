using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class EndGameConsole : MonoBehaviour
{

    private EndZoneScript endZone;

    private bool canInteract;
    
    private void Start()
    {
        endZone = transform.parent.GetComponent<EndZoneScript>();
    }

    private void Update()
    {
        if (!InputManager.PressUp()) return;
        if (canInteract)
        {
            endZone.FinishLevel();
        }
    }

    #region CollisionEvents

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canInteract = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canInteract = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }    }

    #endregion
}
