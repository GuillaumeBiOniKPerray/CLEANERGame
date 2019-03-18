using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndZoneScript : MonoBehaviour {

    // The endzone is the incinerator, it destroys the trash accumulated during the completion of the level.
    // For the moment it only accept one ball.
    // There is already a completion system telling the player if he has fully completed the level or not!
    // TODO : Make the endzone accept multiple trashballs
    // TODO : Create an engame panel with the "star system"
    // TODO : Create a system on special levels to allow the loss of a given amount of trash -> boolean?
    // TODO : Player tells the system whenever he has finished the level by pressing the "Incinerator Button"
    
    private LevelManager levelManager;

    // UI variables
    private GameObject TMProObject;
    private TextMeshProUGUI TMProText;

    // Completion variables
    private float minTrashRequired;
    private float goldMedalRequired;

    private float currentCompletion;


    private void Start()
    {
        levelManager = transform.parent.GetComponent<LevelManager>();
        GameObject gameUI = GameObject.Find("Canvas");
        if (gameUI)
        {
            TMProObject = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        }
        else Debug.LogWarning("There is no Canvas on the scene! Go pick it in the prefab folder");
        goldMedalRequired = levelManager.GetTotalNumberOfTrashInLevel();
        minTrashRequired = goldMedalRequired * 80 / 100 ; // To complete the level the player has got gather 80% of the total amount of trash
        minTrashRequired = Mathf.RoundToInt(minTrashRequired);
        //Debug.Log("mintrashtowin : " + minTrashRequired);
        TMProText = TMProObject.GetComponent<TextMeshProUGUI>();
    }

    public void ClearUI()
    {
        TMProObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trashball"))
        {
            GameObject trashBall = collision.gameObject;
            float trashBallWeight = trashBall.GetComponent<Rigidbody2D>().mass; // It is the mass of the trashball that counts for the completion purposes
            Debug.Log("trashBall weight : " + trashBallWeight);
            currentCompletion += trashBallWeight;
            if (currentCompletion >= minTrashRequired) // Minimum completion
            {
                TMProObject.SetActive(true);
                TMProText.text = "GG!";

                if (currentCompletion >= goldMedalRequired) // Maximum completion
                {
                    TMProText.text = "INCREDIBLE!";
                }
            }
        }
    }
}
