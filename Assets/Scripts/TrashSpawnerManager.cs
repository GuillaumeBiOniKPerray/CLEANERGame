using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawnerManager : MonoBehaviour {

    public GameObject trashPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(mousePos.x, mousePos.y, 0);
            Debug.Log("spawn a trash box");
            Instantiate(trashPrefab, mousePos, Quaternion.identity);
        }
	}
}
