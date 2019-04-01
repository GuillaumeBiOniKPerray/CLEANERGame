using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndZoneScript : MonoBehaviour {

    // The endzone is the incinerator, it destroys the trash accumulated during the completion of the level.
    // For the moment it only accept one ball.
    // There is already a completion system telling the player if he has fully completed the level or not!
    // The endzone can accept multiple trashball
    // TODO : Create an engame panel with the "star system"
    // TODO : Create a system on special levels to allow the loss of a given amount of trash -> boolean?
    // TODO : Player tells the system whenever he has finished the level by pressing the "Incinerator Button"
    
    private LevelManager levelManager;
    private GameController gameController;
    
    // UI variables
    private GameObject TMProObject;
    private TextMeshProUGUI TMProText;
    private GameObject endGamePanel;
    private GameObject enGameProgBar;
    private Text progressionText;
    private Text endGameMessage;

    // Completion variables
    private float minTrashRequired;
    private float goldMedalRequired;
    public float currentCompletion;

    //Absorption variables
    private GameObject ballToAbsorb;
    private bool canAbsorb;

    private void Start()
    {
        levelManager = transform.parent.GetComponent<LevelManager>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        GameObject gameUI = GameObject.Find("Canvas");
        if (gameUI)
        {
            TMProObject = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
            endGamePanel = UIManager.endGamePanel;
            endGameMessage = endGamePanel.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>();
            enGameProgBar = endGamePanel.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            progressionText = endGamePanel.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Text>();
        }
        else Debug.LogWarning("There is no Canvas on the scene! Go pick it in the prefab folder");
        goldMedalRequired = levelManager.GetTotalNumberOfTrashInLevel();
        minTrashRequired = goldMedalRequired * 80 / 100 ; // To complete the level the player has got gather 80% of the total amount of trash
        minTrashRequired = Mathf.RoundToInt(minTrashRequired);
//        Debug.Log("mintrashtowin : " + minTrashRequired);
        TMProText = TMProObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (canAbsorb)
        {
            Vector3 ballPos = ballToAbsorb.transform.position;
            ballToAbsorb.transform.position = Vector3.Lerp(ballPos, transform.position, Time.deltaTime);
            float dist = Vector3.Distance(ballPos, transform.position);
            if (dist < 0.1f) canAbsorb = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trashball"))
        {
            GameObject trashBall = collision.gameObject;
            ballToAbsorb = trashBall;
            Rigidbody2D trashBallRb = trashBall.GetComponent<Rigidbody2D>();
            float trashBallWeight = trashBallRb.mass; // It is the mass of the trashball that counts for the completion purposes
            trashBall.SetActive(false);
//            trashBall.layer = 11;
//            trashBallRb.simulated = false;
//            Destroy(trashBall.GetComponent<CircleCollider2D>());
            currentCompletion += trashBallWeight;
            UIManager.UpdateProgressionBar((int)currentCompletion/goldMedalRequired);
        }
    }

    public void FinishLevel()
    {
        if (currentCompletion >= minTrashRequired) // Minimum completion
        {
            gameController.playerController.state = PlayerController.PlayerState.NOMOVE;
            endGameMessage.text = "Presque parfait.";
            UIManager.ShowEndGamePanel();
            SetProgressionLevel();
            if (levelManager.playerPickedSouvenir)
            {
                endGamePanel.transform.GetChild(1).gameObject.SetActive(true);
            }
            if (currentCompletion >= goldMedalRequired) // Maximum completion
            {
                endGameMessage.text = "Le Secteur est propre!";
            }
        }
        else
        {
            TMProText.gameObject.SetActive(true);
            TMProText.text = "Il reste des déchets dans ce secteur.";
            StartCoroutine(DelayedDestroy());
        }
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(4);
        TMProText.gameObject.SetActive(false);
    }

    private void SetProgressionLevel()
    {
        float completion = currentCompletion / goldMedalRequired;
//        Debug.Log("completion level : " + completion);
        Vector3 progressBarCurrScale = enGameProgBar.transform.localScale;
        Vector3 progressBarNewScale = new Vector3(completion, progressBarCurrScale.y, progressBarCurrScale.z);
        completion = completion * 100;
        completion = (int) completion;
        progressionText.text = (completion) + " %";
        enGameProgBar.transform.localScale = progressBarNewScale;
    }
    
}
