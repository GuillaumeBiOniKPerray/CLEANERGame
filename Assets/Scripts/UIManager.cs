using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    /*
     * Has to manage the UI behavior
     * 
     * 
     */

    // UI related objects
    // -Main Menu Objects-
    public static GameObject mainMenu;
    public static GameObject mainMenuPanel;
    public static GameObject levelSelectionPanel;
    
    public static GameObject gameUI;
    
    public static GameObject endGamePanel;
    public static GameObject souvenirText;
    
    public static GameObject pausePanel;
    
    public static GameObject currentUIOnScreen;

    private void Awake()
    {
        foreach (Transform uiElement in transform)
        {
            GameObject uiElementGo = uiElement.gameObject;
            string uiElementName = uiElementGo.name;
            switch (uiElementName)
            {
                case "Game_UI":
                    gameUI = uiElementGo;
                    break;
                case "Menu":
                    mainMenu = uiElementGo;
                    mainMenuPanel = mainMenu.transform.GetChild(0).gameObject;
                    levelSelectionPanel = mainMenu.transform.GetChild(1).gameObject;
                    break;
                case "End_Game_Panel":
                    endGamePanel = uiElementGo;
                    souvenirText = endGamePanel.transform.GetChild(1).gameObject;
                    break;
                case "Pause_Panel":
                    pausePanel = uiElementGo;
                    break;
                default: 
                    break;
            }
        }
    }

    public static void GoToMainMenu()
    {
        endGamePanel.SetActive(false);
        pausePanel.SetActive(false);
        gameUI.SetActive(false);
        
        //The main menu is composed of a "mainMenuPanel" and a "levelSelectionPanel"
        //We always want to have the mainMenuPanel active when we go to the menu
        mainMenu.SetActive(true);
        mainMenuPanel.SetActive(true);
        levelSelectionPanel.SetActive(false);
    }

    public static void ShowPausePanel(bool value)
    {
        pausePanel.SetActive(value);
        gameUI.SetActive(!value);
    }

    public static void CloseEndGamePanel()
    {
        endGamePanel.SetActive(false);
        souvenirText.SetActive(false);
    }
    
    public static void ShowEndGamePanel()
    {
        endGamePanel.SetActive(true);
        gameUI.SetActive(false);
    }

    public static void GoToGame()
    {
        mainMenu.SetActive(false);
        endGamePanel.SetActive(false);
        gameUI.SetActive(true);
    }
    
//    public static GameObject GetCurrenUIOnScreen()
//    {
//        return currentUIOnScreen;
//    }
//    
//
//    public static void SetCurrentUIOnScreen(GameObject newUI)
//    {
//        currentUIOnScreen = newUI;
//    }

//    public static void SwitchUI(GameObject newUI)
//    {
//        currentUIOnScreen.SetActive(false);
//        currentUIOnScreen = newUI;
//        currentUIOnScreen.SetActive(true);
//    }
}
