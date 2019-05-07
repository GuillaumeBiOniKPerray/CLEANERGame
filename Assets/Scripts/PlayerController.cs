﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour {

    // The avatar has to follow the player's commands.
    
    private Rigidbody2D rb;
    private Animator animator;
    // Movements and Jump related public variables
    public float moveSpeed = 5;
    public float jumpForce = 2.2f;
    [Range(0,1)]public float jumpSpeedModifier =0.5f; // The speed is modified by the jump state

    public GameObject trashBall;// This refers to the trashball prefab
    public GameObject trashBallOffset;// This is the place we want to instantiate a ball from
    public float maxDistanceToTrashball;
    public float ballDragFactor;
    private bool faceRight = true; // right/left boolean used to flip the player's orientation
    private bool isJumping = false;
//    public PusherBehavior pusherScript;
    private bool isCleaning; // Allows the player to stop gathering trash while holding a key.

    private GameObject currentTrashball; // Trashball that is being pushed

    //Layers
    public LayerMask trashBallLayer;
    public LayerMask floorLayer;

    //Player Sounds 
    private AudioSource audioSource;
    public AudioClip engineSound;
    public AudioClip jumpSound;
    public AudioClip pushingSound;
    private bool soundTransition;
    
    //Trash Sounds
    public AudioClip[] trashClips;
    
    public enum PlayerState
    {
        PLAYING, PAUSED, NOMOVE
    }

    public PlayerState state;
    
    //System elements
    private LevelManager levelManager;
    private GameController gameController;

	private void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        AudioEngine();
    }
	
	private void FixedUpdate ()
    {
        if (state == PlayerState.NOMOVE || state == PlayerState.PAUSED) return;
        
        UnderFeetNormal();
        //Expand the following region to know more about the input events
        #region Inputs
        if (InputManager.hAxis>0)
        {
            Move(InputManager.hAxis);
            if(!faceRight)
            {
                faceRight = true;
//                pusherScript.isMoving = true;
//                animator.SetTrigger("TurnReverse");
//                pusherScript.SwitchPusherDirection();
                Flip();
            }
        }
        if(InputManager.hAxis < 0)
        {
            Move(InputManager.hAxis);
            if(faceRight)
            {
                faceRight = false;
//                pusherScript.isMoving = true;
//                animator.SetTrigger("Turn");
//                pusherScript.SwitchPusherDirection();
                Flip();
            }
        }
        else if(InputManager.hAxis > -0.1f && InputManager.hAxis < 0.1f)
        {
            Brake();
        }

        if (InputManager.PressSpace())
        {
            if(!isJumping)
                Jump();
        }

        if (InputManager.PressLeftCtrl())
        {
            isCleaning = false;
            animator.SetBool("isPressed",true);
        }
        else
        {
            isCleaning = true;
            animator.SetBool("isPressed",false);
        }

        #endregion

        if (rb.velocity.y < 0 && isJumping)
        {
            //CheckFloorPosition();
            CheckTrashBallPosition();
        }

        if(currentTrashball)
        {
            Rigidbody2D ballRB = currentTrashball.GetComponent<Rigidbody2D>();
            float dist = currentTrashball.transform.position.x - transform.position.x;
            if (dist < 0) dist = dist * -1;
            float ballRadius = currentTrashball.transform.localScale.x / 2;
            if(dist <= maxDistanceToTrashball+ ballRadius)
            {
                ballRB.drag = ballDragFactor;
            }
            else
            {
                ballRB.drag = 0.5f;
                currentTrashball = null;
            }
        }

    }

    private void Move(float dir)
    {
        rb.velocity = !isJumping ? new Vector2(dir * moveSpeed, rb.velocity.y) : new Vector2(dir * moveSpeed * jumpSpeedModifier, rb.velocity.y);
        Push(Mathf.RoundToInt(dir));
        animator.SetBool("isMoving" , true);
    }

    private void Brake ()
    {
        if(!isJumping)
            rb.velocity = new Vector2 (0,rb.velocity.y);
        animator.SetBool("isMoving" , false);
    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * - 1, transform.localScale.y);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isJumping = true;
        animator.SetBool("isJumping" , true);
        AudioJump();
    }

    private void CheckTrashBallPosition()
    {
        Vector3 point = transform.position + new Vector3(transform.localScale.x / 2, 0, 0);
        RaycastHit2D hit = Physics2D.Raycast(point, Vector2.down, 0.4f, trashBallLayer); // I only consider the "Trashball" layer. Be careful about the 'distance' parameter!
        Debug.DrawRay(point, Vector2.down, Color.red);
        if (hit.collider)
        {
            isJumping = false;
            animator.SetBool("isJumping" , false);
            AudioEngine();
        }
        RaycastHit2D hitMid = Physics2D.Raycast(transform.position, Vector2.down, 0.4f, trashBallLayer); // I only consider the "Trashball" layer. Be careful about the 'distance' parameter!
        Debug.DrawRay(transform.position, Vector2.down, Color.red);
        if (hitMid.collider)
        {
            isJumping = false;
            animator.SetBool("isJumping" , false);
            AudioEngine();
        }
    }

    private void UnderFeetNormal()
    {
        Vector3 point = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(point, Vector2.down, 0.4f,floorLayer); // I only consider some layers (not "Trashball"). Be careful about the 'distance' parameter!
        Debug.DrawRay(point, Vector2.down, Color.red);
        if (hit.collider)
        {
            Vector2 nrmal = hit.normal;
            float zAngle = Vector3.Angle(Vector3.up, nrmal);
            Vector3 cross= Vector3.Cross(Vector3.up, nrmal);
            if (cross.z < 0) zAngle = -zAngle;
//            Debug.Log("cross value : " + cross);
            Debug.DrawRay(transform.position, Vector3.up, Color.green);
            Debug.DrawRay(transform.position, hit.normal, Color.cyan);
//          Debug.Log("the normal : " + hit.normal);
//            Debug.Log("zAngle : " + zAngle);
//          Debug.Log("angle to apply : " + zAngle);
            transform.rotation = Quaternion.Euler(0, 0, zAngle);
        }
    }

    private void Push(int xDirection)
    {
        Vector2 point = transform.position - new Vector3(0, transform.localScale.y / 4);
        RaycastHit2D hit = Physics2D.Raycast(point, new Vector2(xDirection, 0), 0.5f, trashBallLayer); // I only consider the "Trashball" layer
        Debug.DrawRay(point, new Vector2(xDirection,0), Color.red);
        if(hit.collider && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, -1); //sticks the player to the floor
            currentTrashball = hit.collider.gameObject;
        }
    }

    public void PausePlayer(bool pauseState)
    {
        rb.simulated = !pauseState;
        audioSource.Stop();
        animator.enabled = !pauseState;
        if(!pauseState) audioSource.Play();
        foreach (GameObject activeTrashball in levelManager.trashToDestroy) //We want to disable the rigidbody of all the active trasballs
        {
            activeTrashball.GetComponent<Rigidbody2D>().isKinematic = pauseState;
            activeTrashball.GetComponent<Rigidbody2D>().simulated = !pauseState;
        }
    }

    //Expand this region to see the methods related to collision events
    #region CollisionEvents

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Dust":
                if(isCleaning)
                {
                    GameObject newTrash = Instantiate(trashBall, trashBallOffset.transform.position, Quaternion.identity);
                    levelManager.AddToTrashToDestroy(newTrash);
                    newTrash.GetComponent<Rigidbody2D>().mass = other.GetComponent<TrashManager>().trashValue;
                    PlayTrashClip();
//                    levelManager.SetNumberOfTrashCollected();
                    Destroy(other.gameObject);
                }
                break;
            
            case "Souvenir":
                Destroy(other.gameObject);
                levelManager.PickUpSouvenir();
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Lifter"))
        {
            isJumping = false;
            animator.SetBool("isJumping" , false);
            AudioEngine();
        }
    }

//    private void OnCollisionStay2D(Collision2D other)
//    {
//        if (other.gameObject.CompareTag("Trashball"))
//        {
//            
//            GameObject trashballGO = other.gameObject;
//            Rigidbody2D trashRB = trashballGO.GetComponent<Rigidbody2D>();
//            if (transform.position.y < trashballGO.transform.position.y)
//            {
////                Debug.Log("slow the trashball");
////                trashRB.velocity = new Vector2(rb.velocity.x * ballSlowFactor, trashRB.velocity.y);
////                rb.velocity = new Vector3(rb.velocity.x * playerSlowFactor, rb.velocity.y);
//            }
//        }
//    }

    #endregion
    
    public void SetLevelManager(LevelManager newLevelManager)
    {
        levelManager = newLevelManager;
    }

    private void AudioEngine()
    {
        audioSource.clip = engineSound;
        audioSource.volume = 0.25f;
        audioSource.Play();
        audioSource.loop = true;
    }

    private void AudioJump()
    {
        audioSource.clip = jumpSound;
        audioSource.Play();
        audioSource.loop = false;
        if(!soundTransition) StartCoroutine(PlayNextSoundAfterThisOne(jumpSound));
        
    }

    private IEnumerator PlayNextSoundAfterThisOne(AudioClip audioClip)
    {
        soundTransition = true;
        yield return new WaitForSeconds(audioClip.length);
        AudioEngine();
        soundTransition = false;
    }

    private void PlayTrashClip()
    {
        AudioClip selectClip = trashClips[Random.Range(0, trashClips.Length - 1)];
        audioSource.clip = selectClip;
        audioSource.Play();
    }
}
