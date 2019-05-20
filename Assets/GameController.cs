using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    // Debug Tools
    public bool publicPlayTest;
    public bool noMenu;
    public bool isPaused;
    public int startingLevelID;
    
    //LevelManager
    public List<LevelManager> levels = new List<LevelManager>();
    private static LevelManager currentLevelManager;
    private GameObject currentLevel;
    public int currentLevelID;

    public GameObject canvasObject;

    public CameraManager camManager;

    //Player
    public GameObject player;
    public PlayerController playerController;

    //Audio
    public AudioClip backgroundAmbient;
    private AudioSource audioSource;
    
	private void Start ()
    {
        playerController = player.GetComponent<PlayerController>();
        
        if (noMenu)
        {
            if(!publicPlayTest) InitiateFirstLevel(startingLevelID);
            InitiateCamera(publicPlayTest);
            UIManager.GoToGame();
        }
        else
        {
            UIManager.GoToMainMenu();
            camManager.isMenu = true;
        }

        audioSource = GetComponent<AudioSource>();
        SetUpAudio();
    }
	
	private void Update ()
    {
        if (InputManager.PressNextLevel()) SwitchToOtherLevel(1);
        if (InputManager.PressPrevLevel()) SwitchToOtherLevel(-1);
        if (InputManager.PressRKey()) GoToOtherLevel(currentLevelID);
        if (InputManager.PressPause())
        {
            if(!UIManager.onMenu) PauseGame();
            else
            {
                UIManager.GoToMainMenu();
            }
        }
    }
	
    public void InitiateFirstLevel(int idSelector)
    {
        currentLevel = Instantiate(levels[idSelector].gameObject);
        player.transform.position = levels[idSelector].playerSpawn.transform.position;
        currentLevelManager = currentLevel.GetComponent<LevelManager>();
        playerController.SetLevelManager(currentLevelManager);
        currentLevelID = idSelector;
        player.SetActive(true);
    }

    private void InitiateCamera(bool isTest)
    {
        if (!isTest)
        {
//            camManager.player = player;
            camManager.AssignCameraPosition();
            camManager.FillDestinationList(currentLevelManager.camPoints);
            camManager.isReadyToMove = true;
            playerController.state = PlayerController.PlayerState.NOMOVE;
        }
        else
        {
//            camManager.player = player;
            camManager.AssignCameraPosition();
            Debug.Log("test mode camera");
        }
    }

    private void SetUpAudio()
    {
        audioSource.clip = backgroundAmbient;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    public void CloseMenu()
    {
        UIManager.GoToGame();
        camManager.isMenu = false;
        InitiateCamera(publicPlayTest);
        player.SetActive(true);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToMainMenu()
    {
        // TODO : Save progression
        if (isPaused)
        {
            PauseGame();
        }
        currentLevelManager.ClearScene();
        Destroy(currentLevel);
        UIManager.GoToMainMenu();
        camManager.isMenu = true;
        player.SetActive(false);
    }
    
    public void PauseGame()
    {
        if(playerController.state == PlayerController.PlayerState.NOMOVE) return;
        isPaused = !isPaused;
        if (isPaused) playerController.state = PlayerController.PlayerState.PAUSED;
        else playerController.state = PlayerController.PlayerState.PLAYING;
        UIManager.ShowPausePanel(isPaused);
        player.GetComponent<PlayerController>().PausePlayer(isPaused);
    }

    private void SwitchToOtherLevel(int selector)
    {
        currentLevelManager.ClearScene();
        Destroy(currentLevel);
        int nextLevelID;
        if (currentLevelID + selector < 0) nextLevelID = 0;
        else if (currentLevelID + selector > levels.Count -1) nextLevelID = 0;
        else nextLevelID = currentLevelID + selector;
        SetLevel(nextLevelID);
    }
    
    public void GoToOtherLevel(int levelID)
    {
        if (isPaused)
        {
            PauseGame();
        }
        currentLevelManager.ClearScene();
        Destroy(currentLevel);
        playerController.state = PlayerController.PlayerState.NOMOVE;
        if (levelID < levels.Count)
        {
            SetLevel(levelID);
        }
    }
    
    private void SetLevel(int id)
    {
        currentLevelID = id;
        currentLevel = Instantiate(levels[currentLevelID].gameObject);
        currentLevelManager = currentLevel.GetComponent<LevelManager>();
        player.transform.position = levels[currentLevelID].playerSpawn.transform.position;
        playerController.SetLevelManager(currentLevelManager);
        camManager.FillDestinationList(currentLevelManager.camPoints);
        camManager.isReadyToMove = true;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public static LevelManager GetCurrentLevelManager()
    {
        return currentLevelManager;
    }
}
