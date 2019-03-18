using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatAndBeingEaten : MonoBehaviour {

    // Change the eaten trashball properties in order to make it agglomerate with the "bigger trashball"
    
    public List<GameObject> objectsToAffect = new List<GameObject>();
    public float eatSpeed = 0.5f;

    public bool isEating;

	private void Update ()
    {
		if(isEating)
        {
            foreach (GameObject affectedObj in objectsToAffect) //All the ball being eaten by another ball are added to a List
            {
                affectedObj.transform.position = Vector3.Lerp(affectedObj.transform.position, transform.position, Time.deltaTime * eatSpeed); //The eaten ball is "absorbed" by the "bigger one"
                if (affectedObj.transform.position == transform.position - new Vector3 (0.1f,0.1f, 0.1f))
                {
//                    isEating = false; 
                    Destroy(affectedObj);
                }
            }
        }
	}

    public void EatABall(GameObject otherGO) // As a ball eats another, the affected ball needs to respond to some new rules
    {
        otherGO.transform.parent = transform; //Be parented
        otherGO.layer = 11; //Change the layer to avoid collision with the eatingball
        otherGO.tag = "Untagged"; //Change the tag 
        // Remove components to prevent malfunction
        Destroy(otherGO.GetComponent<Rigidbody2D>()); 
        Destroy(otherGO.GetComponent<TrashBallPhysicsConstraints>());
        Destroy(otherGO.GetComponent<TrashBallManager>());
        objectsToAffect.Add(otherGO); // finally add to the list of affected objects
        isEating = true;
    }

}
