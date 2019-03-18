using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableLevelIcon : MonoBehaviour
{
    public int levelId;
    public GameController gameController;


    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void ReachWantedLevel()
    {
        Debug.Log("levelID : " + levelId);
        gameController.InitiateFirstLevel(levelId);
        gameController.CloseMenu();
    }
    
}
