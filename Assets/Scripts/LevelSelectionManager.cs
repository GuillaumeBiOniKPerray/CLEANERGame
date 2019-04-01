using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    private List<GameObject> levels = new List<GameObject>();
    public GameObject levelIcon;
    public GameController gameController;
    
    private void Start()
    {
        int i = 0;
        foreach (LevelManager levelManager in gameController.levels)
        {
            levels.Add(levelManager.gameObject);
            GameObject icon = Instantiate(levelIcon, transform);
            icon.GetComponent<ClickableLevelIcon>().levelId = i;
            Transform buttonText = icon.transform.GetChild(0).GetChild(0);
            buttonText.GetComponent<Text>().text = "Niveau " + (i+1);
            i++;
        }
    }
}
