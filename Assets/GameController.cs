using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {


    public bool publicPlayTest;
    public static bool playTest;
    public List<LevelManager> levels = new List<LevelManager>();
    private static LevelManager currentLevelManager;
    public int startingLevelID;
    private GameObject currentLevel;
    public int currentLevelID;

    public GameObject canvasObject;

    public CameraManager camManager;

    public GameObject player;
    private GameObject currentPlayer;

	void Start ()
    {
        canvasObject.SetActive(true);
        playTest = publicPlayTest;
        if(!playTest)
        {
            currentLevel = Instantiate(levels[startingLevelID].gameObject);
            //currentPlayer = Instantiate(player, levels[startingLevelID].playerSpawn.transform.position, Quaternion.identity);
            currentPlayer = player;
            currentPlayer.transform.position = levels[startingLevelID].playerSpawn.transform.position;
            currentLevelManager = currentLevel.GetComponent<LevelManager>();
            currentPlayer.GetComponent<PlayerController>().SetLevelManager(currentLevelManager);
            currentLevelID = startingLevelID;
            camManager.player = currentPlayer;
            camManager.AssignCameraPosition();
            camManager.FillDestinationList(currentLevelManager.camPoints);
            camManager.isReadyToMove = true;
        }
        else
        {
            camManager.player = player;
            camManager.AssignCameraPosition();
        }


    }
	
	void Update ()
    {
        if (InputManager.PressNextLevel()) SwitchToOtherLevel(1);
        if (InputManager.PressPrevLevel()) SwitchToOtherLevel(-1);
        if (InputManager.PressRKey()) SceneManager.LoadScene(0);
    }

    public void SwitchToOtherLevel(int selector)
    {
        currentLevelManager.ClearScene();
        Destroy(currentLevel);
        int nextLevelID;
        if (currentLevelID + selector < 0) nextLevelID = 0;
        else if (currentLevelID + selector > levels.Count -1) nextLevelID = 0;
        else nextLevelID = currentLevelID + selector;
        currentLevelID = nextLevelID;
        currentLevel = Instantiate(levels[currentLevelID].gameObject);
        currentLevelManager = currentLevel.GetComponent<LevelManager>();
        currentPlayer.transform.position = levels[currentLevelID].playerSpawn.transform.position;
        currentPlayer.GetComponent<PlayerController>().SetLevelManager(currentLevelManager);
        camManager.FillDestinationList(currentLevelManager.camPoints);
        camManager.isReadyToMove = true;
    }

    public static LevelManager GetCurrentLevelManager()
    {
        Debug.Log("level Manager : " + currentLevelManager);
        return currentLevelManager;
    }
}
