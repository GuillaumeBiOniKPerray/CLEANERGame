using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class TrashBallManager : MonoBehaviour {

    public float scaleFactor = 0.05f;
    public float massFactor = 0.05f;
    
    //Sounds
    private AudioSource audioSource;
    private bool isPlayingSound;

    private Rigidbody2D rb;
    private float ownMass;

    private TextMeshPro massText;

    private LevelManager levelManager;

	private void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        levelManager = GameController.GetCurrentLevelManager();
        massText = transform.GetChild(0).GetComponent<TextMeshPro>();
        audioSource = GetComponent<AudioSource>();
    }
	
	private void Update ()
    {
        KeepTextRot();
        if (rb.velocity.magnitude > 0 && !isPlayingSound)
        {
            audioSource.Play();
            isPlayingSound = true;
        }

        if (rb.velocity.magnitude == 0)
        {
            audioSource.Stop();
            isPlayingSound = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Dust":
                TrashManager trash = other.GetComponent<TrashManager>();
                int trashValue = trash.trashValue;
                GrowTrashBall(other.gameObject, trashValue);
//                levelManager.SetNumberOfTrashCollected();
                break;
            case "Souvenir":
                Destroy(other.gameObject);
                break;
            default:
                break;
        }
    }

    private void GrowTrashBall(GameObject toDestroy, int massScaleAmount)
    {
        float scaleF = massScaleAmount / 100f;
        transform.localScale += new Vector3(scaleF, scaleF, scaleF);
        rb.mass += massScaleAmount;
        if (toDestroy.CompareTag("Dust"))
        {
            TrashManager trashScript = toDestroy.GetComponent<TrashManager>();
            trashScript.PlayTrashClip();
//            Destroy(toDestroy.gameObject);
        }
        else GetComponent<EatAndBeingEaten>().EatABall(toDestroy);
        if(rb.mass >= 10)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            massText.text = rb.mass + "kg";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trashball"))
        {
            GameObject otherTrashball = collision.gameObject;
            Rigidbody2D otherTrashBallRB = otherTrashball.GetComponent<Rigidbody2D>();
            float otherTrashBallMass = otherTrashBallRB.mass;
            //Debug.Log("Other Trashball Mass : " + otherTrashBallMass);
//            TrashBallManager trashballManager = otherTrashball.GetComponent<TrashBallManager>();
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
