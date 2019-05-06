using System;
using System.Collections;
using System.Collections.Generic;
//using TreeEditor;
//using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Networking;

public class PusherBehavior : MonoBehaviour
{

    public GameObject body;
    public float distanceWithBody;
    public GameObject arm;
    public float armLength;
    
    public LayerMask layerMask;
//    public GameObject[] rayOrigins;
    public GameObject currentRayOrigin;
    public float rayLength;
    public float maxAngleThreshold;
    public float maxDistance;

    public bool isMoving;
    public bool stopTakingTrash;
    
    private float minAngleThreshold;
    private Vector3 vectorDistance;
    private Vector3 linkPosition;
    private float ySize;

    private void Start()
    {
        minAngleThreshold = -maxAngleThreshold;
//        currentRayOrigin = rayOrigins[0];
        ySize = transform.position.y - currentRayOrigin.transform.position.y;
        Debug.Log("ray origin : " + currentRayOrigin);
    }

    private void LateUpdate()
    {
        Debug.Log("-----------------------------------");
        
//        if(isMoving) return; // If the animation is on, we don't recalculate the pusher + arm position
        
        //Rotation

        float c = armLength;
        float a = body.transform.position.y - arm.transform.position.y;
        Vector3 corner = new Vector3(transform.position.x,body.transform.position.y,0);
        Debug.DrawLine(body.transform.position,corner,Color.red);
        a = Mathf.Clamp(a, -c, c);
//        Debug.Log("a : " +a );
        float cosAngle = a / c;
//        Debug.Log("cosAngle : " +cosAngle);
        float tempAngle = Mathf.Acos(cosAngle);
        float degreeAngle = tempAngle * Mathf.Rad2Deg;
//        Debug.Log("angle into degrees : " +degreeAngle);
        Quaternion rot;
        float newAngle;
        if (degreeAngle < 90) //If the angle is lower than 90 degrees, it means that the pusher is supposed to be lower that the body's center
        {
            newAngle = -90 + degreeAngle;
//            Debug.Log("arm angle = " + newAngle);
            newAngle = Mathf.Clamp(newAngle, minAngleThreshold, 0); // We clamp the angle to a minimum angle
            rot= Quaternion.Euler(0,0,newAngle);
        }
        else //If the angle is greater than 90 degrees, it means that the pusher is supposed to be higher that the body's center
        {
            newAngle = degreeAngle-90;
//            Debug.Log("arm angle = " + newAngle);
            newAngle = Mathf.Clamp(newAngle, 0, maxAngleThreshold); // We clamp the angle to a maximum angle
            rot= Quaternion.Euler(0,0,newAngle);
        }
        arm.transform.rotation = rot;

            //Position
            Vector3 pusherPos = transform.position;
            Vector3 rayPos = currentRayOrigin.transform.position;
            float yPos = pusherPos.y;
            Vector3 raycastPosition = new Vector3(rayPos.x, rayPos.y + 0.02f, 0);
            RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.down, rayLength, layerMask); //Hits the 'Environment' layer
            Debug.DrawRay(rayPos,Vector3.down,Color.red);

            if (hit.collider)
            {
                yPos = hit.point.y + ySize;
                float yDistance = body.transform.position.y - yPos;
//            Debug.Log("distance : " + yDistance);
                if (yDistance > maxDistance)
                {
                    yPos = body.transform.position.y - maxDistance ;
                }
            }
            transform.position = new Vector2(body.transform.position.x + distanceWithBody, yPos);
            linkPosition = transform.GetChild(0).position;
            arm.transform.position = linkPosition;
            Debug.Log("FINISHED UPDATE!!");
    }

//    public void SwitchPusherDirection()
//    {
//        if (currentRayOrigin == rayOrigins[0]) //If the player is going right
//        {
//            currentRayOrigin = rayOrigins[1]; //we swap its origin to the opposite origin
//            
//        }
//        else
//        {
//            currentRayOrigin = rayOrigins[0];
//        }
//
//        distanceWithBody = -distanceWithBody;
//    }
}
