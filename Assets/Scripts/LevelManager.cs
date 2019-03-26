using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour {

    /*
        To create a Level there are some rules to follow :
        - There must be :
            - player start gameobject (empty)
            - end of game 
            - "dusts" object (empty that contains all the trash of the level)
        
        - Be careful to the public variables :
            - Some of them are prefabs:
                - player
                - totalTrash
                - souvenirUI
            - Others are from Hierarchy :
                - playerSpawn
                - progressionBar
         */

    // TODO : Update progression bar behavior

    private Transform canvas;

    // Progression related variables
    [Tooltip ("Put here the GameObject that contains all the trash of the level")]
    public GameObject totalTrash;
    public int massToCollect;
    [Tooltip("Put here the progression bar GameObject")]
    public GameObject progressionBar;
    public int numberOfTrashInLevel;
    public int numberOfCollectedTrash;

    //Souvenir related variables
    private GameObject souvenir; //The code will try to find if the is a souvenir on the map
    [Tooltip("Put here the prefab the Souvenir_Text")]
    public GameObject souvenirUI; 
    private TextMeshProUGUI souvenirText; //This component will allow us to tell the player whether he took the souvenir or not
    public bool hasSouvenir;
    public bool playerPickedSouvenir;

    //Player related variables
    public GameObject player;
    public GameObject playerSpawn;
    private GameObject currentPlayer;

    public List<GameObject> camPoints = new List<GameObject>(); // Camera travelling destinations

    public List<GameObject> trashToDestroy = new List<GameObject>(); // Trash is destroyed when switching to another level

    public EndZoneScript endZone;

	private void Start ()
    {
        if (!souvenirUI) Debug.LogWarning("You didn't set the souvenirUI prefab! Go pick it in the prefab folder.");
        if (!totalTrash) Debug.LogWarning("You didn't set the totalTrash GameObject! Put all your 'trash' in an empty gameobject and put it in the Level Manager input called totalTrash ");

        numberOfCollectedTrash = 0;
        numberOfTrashInLevel = totalTrash.transform.childCount;
        
        canvas = GameObject.Find("Canvas").transform;
        FindSouvenirGameObject();
        progressionBar = canvas.transform.GetChild(1).GetChild(0).GetChild(0).gameObject; // The progression bar is a children BE CAREFUL!
        progressionBar.transform.parent.gameObject.SetActive(true);
    }

    private void FindSouvenirGameObject() // All levels don't have souvenir (yet), so when there's one, we cycle through the level elements to catch the souvenir
    {
        foreach (Transform levelChild in transform)
        {
            if (levelChild.gameObject.name == "Souvenir")
            {
                hasSouvenir = true;
                GameObject souvGo = Instantiate(souvenirUI, canvas);
                souvenirText = souvGo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            }
        }
    }

    public void SetNumberOfTrashCollected() // Updates the progression bar size (not final)
    {
        numberOfCollectedTrash++;
        float xProgToAdd = numberOfCollectedTrash / (float)numberOfTrashInLevel;
        Vector3 progToAdd = new Vector3(xProgToAdd, progressionBar.transform.localScale.y, progressionBar.transform.localScale.z);
        progressionBar.transform.localScale = progToAdd;
    }

    public void PickUpSouvenir()
    {
        souvenirText.text = "1 / 1";
        playerPickedSouvenir = true;
    }

    public int  GetTotalNumberOfTrashInLevel() // Gets the total amount of trash 
    {
        foreach(Transform trash in totalTrash.transform)
        {
            massToCollect += trash.GetComponent<TrashManager>().trashValue;
        }
        return massToCollect;
    }

    public void AddToTrashToDestroy(GameObject trash)
    {
        trashToDestroy.Add(trash);
    }

    public void ClearScene() // When switching to another level, we need to clear the scene before making the new level spawn
    {
        progressionBar.transform.localScale = new Vector3(0, progressionBar.transform.localScale.y, progressionBar.transform.localScale.z);
        UIManager.CloseEndGamePanel();
        UIManager.GoToGame();
        if (hasSouvenir)
        {
            souvenirText.text = "0 / 1";
            souvenirText.transform.parent.gameObject.SetActive(false);
            hasSouvenir = false;
            playerPickedSouvenir = false;
        }
        foreach (GameObject trsh in trashToDestroy)
        {
            Debug.Log("trash to destroy : " + trsh);
            Destroy(trsh);
        }
            trashToDestroy.Clear();
    }
}
