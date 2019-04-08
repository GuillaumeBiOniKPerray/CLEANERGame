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
    public float maxAngleThreshold;
    private float minAngleThreshold;
    private Vector3 vectorDistance;
    private Vector3 linkPosition;
    private float armSize;
    

    private void Start()
    {
        minAngleThreshold = -maxAngleThreshold;
//        linkPosition = transform.GetChild(0).position;
//        armSize = arm.GetComponent<SpriteRenderer>().size.x;

    }

    // Update is called once per frame
    private void Update()
    {
        linkPosition = transform.GetChild(0).position;
        arm.transform.position = linkPosition;
        
        
        Debug.Log("-------------------------------------");
        //Rotation
        const float c = 0.5f;
//        float a = Vector2.Distance(new Vector2(0, body.transform.position.y), new Vector2(0, arm.transform.position.y));
        float a = body.transform.position.y - arm.transform.position.y;
        a = Mathf.Clamp(a, -c, c);
        Debug.Log("a : " +a );
        
        float cosAngle = a / c;
        Debug.Log("cosAngle : " +cosAngle);
        float angle = Mathf.Acos(cosAngle);
//        Debug.Log("radian angle : " +angle );
        float degreeAngle = angle * Mathf.Rad2Deg;
        Debug.Log("angle into degrees : " +degreeAngle);
        Quaternion rot;
        if (degreeAngle < 90) //If the angle is lower than 90 degrees, it means that the pusher is supposed to be lower that the body's center
        {
            float newAngle = -90 + degreeAngle;
            newAngle = Mathf.Clamp(newAngle, minAngleThreshold, 0);
            Debug.Log("angle : " + newAngle);
            rot= Quaternion.Euler(0,0,newAngle);
        }
        else //If the angle is greater than 90 degrees, it means that the pusher is supposed to be higher that the body's center
        {
            float newAngle = degreeAngle-90;
            newAngle = Mathf.Clamp(newAngle, 0, maxAngleThreshold);
            Debug.Log("angle : " + newAngle);
            rot= Quaternion.Euler(0,0,newAngle);
        }
        arm.transform.rotation = rot;

        
        //Position
        Vector3 pusherPos = transform.position;
        float yPos = pusherPos.y ;
        Vector3 raycastPosition = new Vector3(pusherPos.x, pusherPos.y + 0.05f, 0);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.down, 0.5f, layerMask); //Hits the 'Environment' layer
        Debug.DrawRay(pusherPos,Vector3.down,Color.red);
        if (hit.collider )
        {
            Debug.Log("Hit!");
            if (a < c-0.01f && a > -c+0.01f)
            {
                yPos = hit.point.y;
            }
        }
        
        transform.position = new Vector2(body.transform.position.x + distanceWithBody, yPos);
    }
}
