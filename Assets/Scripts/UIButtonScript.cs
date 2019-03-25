using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonScript : MonoBehaviour
{

    private GameController gameController;
    
    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void GoNextLevel()
    {
        gameController.GoToOtherLevel(gameController.currentLevelID+1);
    }

    public void RetryLevel()
    {
        gameController.GoToOtherLevel(gameController.currentLevelID);
    }

    public void GoToMainMenu()
    {
        gameController.RestartScene();
    }
}
