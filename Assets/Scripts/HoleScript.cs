using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using UnityEngine;

public class HoleScript : MonoBehaviour
{

    private LevelManager levelManager;
    private int trashThreshold;
    void Start()
    {
        levelManager = transform.parent.GetComponent<LevelManager>();
        Debug.Log("levelManager value : " + levelManager.GetTotalNumberOfTrashInLevel());
        trashThreshold = levelManager.GetTotalNumberOfTrashInLevel();
        trashThreshold = trashThreshold *80/100;
        Debug.Log("trashThreshold : " + trashThreshold);
        trashThreshold = Mathf.RoundToInt(trashThreshold);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trashball"))
        {
            Rigidbody2D trashRB = other.GetComponent<Rigidbody2D>();
            float trashMass = trashRB.mass;
            if (trashMass > trashThreshold)
            {
                Debug.Log("Vous ne pourrez pas rassembler suffisament de déchets pour finir le niveau!");
                Destroy(other.gameObject);
            }
        }
    }
}
