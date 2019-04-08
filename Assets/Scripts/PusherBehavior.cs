using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Networking;

public class PusherBehavior : MonoBehaviour
{

    public GameObject body;
    public float distanceWithBody;
    public GameObject arm;
    public LayerMask layerMask;
    private Vector3 vectorDistance;
    private Vector3 linkPosition;
    private float armSize;
    

    private void Start()
    {
//        linkPosition = transform.GetChild(0).position;
//        armSize = arm.GetComponent<SpriteRenderer>().size.x;
        
    }

    // Update is called once per frame
    private void Update()
    {
        linkPosition = transform.GetChild(0).position;
        arm.transform.position = linkPosition;
        Vector3 pusherPos = transform.position;
        float yPos = 0 ;
        Vector3 raycastPosition = new Vector3(pusherPos.x, pusherPos.y + 0.05f, 0);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.down, 0.2f, layerMask); //Hits the 'Environment' layer
        Debug.DrawRay(pusherPos,Vector3.down,Color.red);
        if (hit.collider)
        {
            yPos = hit.point.y;
        }
        else Debug.Log("Hit Nothing!");
        
        transform.position = new Vector2(body.transform.position.x + distanceWithBody, yPos);
        float c = 0.5f;
//        float a = Vector2.Distance(new Vector2(0, body.transform.position.y), new Vector2(0, arm.transform.position.y));
        float a = body.transform.position.y - arm.transform.position.y;
        Debug.Log("a : " +a );
        float cosAngle = a / c;
        Debug.Log("cosAngle : " +cosAngle);
        float angle = Mathf.Acos(cosAngle);
//        Debug.Log("radian angle : " +angle );
        float degreeAngle = angle * Mathf.Rad2Deg;
        Debug.Log("angle into degrees : " +degreeAngle);
        if (degreeAngle < 45) // If the angle is too sharp, we move the linkposition to the top 
        {
            Debug.Log("out of bounds !");
            Vector2 newArmPos = new Vector2(linkPosition.x,linkPosition.y+0.1f);
            transform.GetChild(0).position = newArmPos;
        }
//        else if()
//        {
//            Vector2 newArmPos = new Vector2(linkPosition.x,linkPosition.y-0.1f);
//            transform.GetChild(0).position = newArmPos;
//        }
        Quaternion rot;
        if (degreeAngle < 90) //If the angle is lower than 90 degrees, it means that the pusher is supposed to be lower that the body's center
        {
            rot= Quaternion.Euler(0,0,-90+degreeAngle);
        }
        else //If the angle is greater than 90 degrees, it means that the pusher is supposed to be higher that the body's center
        {
            Debug.Log("degrees to add : " + (degreeAngle-90));
            rot= Quaternion.Euler(0,0,degreeAngle-90);
        }
        arm.transform.rotation = rot;

    }
}
