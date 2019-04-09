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
    public GameObject rayOrigin;
    public float rayLength;
    public float maxAngleThreshold;
    public float maxDistance;
    private float minAngleThreshold;
    private Vector3 vectorDistance;
    private Vector3 linkPosition;
    private float ySize;

    private void Start()
    {
        minAngleThreshold = -maxAngleThreshold;
        ySize = transform.position.y - rayOrigin.transform.position.y;
        Debug.Log("half size of pusher : " + ySize);
    }

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
        float degreeAngle = angle * Mathf.Rad2Deg;
        Debug.Log("angle into degrees : " +degreeAngle);
        Quaternion rot;
        float newAngle;
        if (degreeAngle < 90) //If the angle is lower than 90 degrees, it means that the pusher is supposed to be lower that the body's center
        {
            newAngle = -90 + degreeAngle;
            newAngle = Mathf.Clamp(newAngle, minAngleThreshold, 0); // We clamp the angle to a minimum angle
            rot= Quaternion.Euler(0,0,newAngle);
        }
        else //If the angle is greater than 90 degrees, it means that the pusher is supposed to be higher that the body's center
        {
            newAngle = degreeAngle-90;
            newAngle = Mathf.Clamp(newAngle, 0, maxAngleThreshold); // We clamp the angle to a maximum angle
            rot= Quaternion.Euler(0,0,newAngle);
        }
        arm.transform.rotation = rot;

        
        //Position
        Vector3 pusherPos = transform.position;
        Vector3 rayPos = rayOrigin.transform.position;
        float yPos = pusherPos.y ;
        Vector3 raycastPosition = new Vector3(rayPos.x, rayPos.y + 0.02f, 0);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.down, rayLength, layerMask); //Hits the 'Environment' layer
        Debug.DrawRay(rayPos,Vector3.down,Color.red);

        if (hit.collider)
        {
            Debug.Log("Hit!");
            yPos = hit.point.y + ySize;
            float yDistance = body.transform.position.y - yPos;
            Debug.Log("distance : " + yDistance);
            if (yDistance > maxDistance)
            {
                yPos = body.transform.position.y - maxDistance ;
            }
        }
        transform.position = new Vector2(body.transform.position.x + distanceWithBody, yPos);
    }
}
