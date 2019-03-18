using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrashBallManager : MonoBehaviour {

    public float scaleFactor = 0.05f;
    public float massFactor = 0.05f;

    Rigidbody2D rb;
    float ownMass;

    private TextMeshPro massText;

    private LevelManager levelManager;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        levelManager = GameController.GetCurrentLevelManager();
        massText = transform.GetChild(0).GetComponent<TextMeshPro>();
    }
	
	void Update ()
    {
        KeepTextRot();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Dust")
        {
            TrashManager trash = other.GetComponent<TrashManager>();
            int trashValue = trash.trashValue;
            GrowTrashBall(other.gameObject, trashValue);
            levelManager.SetNumberOfTrashCollected();
        }
        if (other.tag == "Souvenir")
        {
            Destroy(other.gameObject);
        }

    }

    public void GrowTrashBall(GameObject toDestroy, int massScaleAmount)
    {
        //Debug.Log("amount given : " + amount);
        float scaleF = massScaleAmount / 100f;
        //Debug.Log("scalefactor : " + scaleF);
        transform.localScale += new Vector3(scaleF, scaleF, scaleF);
        rb.mass += massScaleAmount;
        if (toDestroy.tag == "Dust")
            Destroy(toDestroy.gameObject);
        else GetComponent<EatAndBeingEaten>().EatABall(toDestroy);
        if(rb.mass >= 10)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            massText.text = rb.mass + "kg";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trashball")
        {
            GameObject otherTrashball = collision.gameObject;
            Rigidbody2D otherTrashBallRB = otherTrashball.GetComponent<Rigidbody2D>();
            float otherTrashBallMass = otherTrashBallRB.mass;
            //Debug.Log("Other Trashball Mass : " + otherTrashBallMass);
            TrashBallManager trashballManager = otherTrashball.GetComponent<TrashBallManager>();
            if (otherTrashBallMass <= rb.mass / 2f)
            {
                GrowTrashBall(otherTrashball, (int)otherTrashBallMass);
            }
        }
    }

    private void KeepTextRot()
    {
        transform.GetChild(0).transform.rotation = Quaternion.identity;
    }
}
