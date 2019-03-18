using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {


    public bool publicPlayTest;
    public bool noMenu;
    public bool isPaused;
    public List<LevelManager> levels = new List<LevelManager>();
    private static LevelManager currentLevelManager;
    public int startingLevelID;
    private GameObject currentLevel;
    public int currentLevelID;

    public GameObject canvasObject;
    public GameObject menuGameObject;

    public CameraManager camManager;

    public GameObject player;
    private GameObject currentPlayer;

	private void Start ()
    {
        canvasObject.SetActive(true);
        if (noMenu)
        {
            if(!publicPlayTest) InitiateFirstLevel(startingLevelID);
            InitiateCamera(publicPlayTest);
        }
        else
        {
            menuGameObject.SetActive(true);
            camManager.isMenu = true;
        }
    }
	
	private void Update ()
    {
        if (InputManager.PressNextLevel()) SwitchToOtherLevel(1);
        if (InputManager.PressPrevLevel()) SwitchToOtherLevel(-1);
        if (InputManager.PressRKey()) SceneManager.LoadScene(0);
    }
	
	public void InitiateFirstLevel(int idSelector)
    {
        currentLevel = Instantiate(levels[idSelector].gameObject);
        //currentPlayer = Instantiate(player, levels[startingLevelID].playerSpawn.transform.position, Quaternion.identity);
        currentPlayer = player;
        currentPlayer.transform.position = levels[idSelector].playerSpawn.transform.position;
        currentLevelManager = currentLevel.GetComponent<LevelManager>();
        currentPlayer.GetComponent<PlayerController>().SetLevelManager(currentLevelManager);
        currentLevelID = idSelector;
    }

    private void InitiateCamera(bool isTest)
    {
        if (!isTest)
        {
            camManager.player = currentPlayer;
            camManager.AssignCameraPosition();
            camManager.FillDestinationList(currentLevelManager.camPoints);
            camManager.isReadyToMove = true;
        }
        else
        {
            camManager.player = player;
            camManager.AssignCameraPosition();
            Debug.Log("test mode camera");
        }
    }

    public void CloseMenu()
    {
        menuGameObject.SetActive(false);
        camManager.isMenu = false;
        InitiateCamera(publicPlayTest);
    }

    private void SwitchToOtherLevel(int selector)
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

    public void QuitApplication()
    {
        Application.Quit();
    }

    public static LevelManager GetCurrentLevelManager()
    {
        Debug.Log("level Manager : " + currentLevelManager);
        return currentLevelManager;
    }
}
