using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBehavior : MonoBehaviour
{
    public GameObject body;
    public float distanceWithBody;
    public float maxDistance;
    
    public GameObject connectedPoint;
    public GameObject damper;
    public GameObject link;
    public bool isLeft;

    public GameObject rayOrigin;
    public float rayLength;
    public LayerMask layerMask;
    
    public float maxAngleThreshold;
    private float minAngleThreshold;

    private float yDistanceRayOrigin;
    private float distanceFromLink;
    
    private void Start()
    {
        minAngleThreshold = -maxAngleThreshold;
        yDistanceRayOrigin = rayOrigin.transform.position.y - transform.position.y;
        
    }

    // Update is called once per frame
    private void Update()
    {
        damper.transform.position = transform.position;
        
        Debug.Log("-------------------------------------");
        
        //Rotation
        const float c = 0.5f;
        float a = body.transform.position.y - connectedPoint.transform.position.y;
        a = Mathf.Clamp(a, -c, c);
        Debug.Log("a : " +a );
        float cosAngle = a / c;
        Debug.Log("cosAngle : " +cosAngle);
        float angle = Mathf.Acos(cosAngle);
        float degreeAngle = angle * Mathf.Rad2Deg;
        Debug.Log("angle into degrees : " +degreeAngle);
        Quaternion rot = Quaternion.identity;
        float newAngle;
        if (degreeAngle < 90) // For right track
        {
            
            newAngle = degreeAngle-90;;
            newAngle = Mathf.Clamp(newAngle, minAngleThreshold, maxAngleThreshold); // We clamp the angle to a minimum angle
            if(!isLeft) newAngle = -newAngle;
            Debug.Log("final angle : " +newAngle);
            rot= Quaternion.Euler(0,0,newAngle);
        }
//        else //If the angle is greater than 90 degrees, it means that the pusher is supposed to be higher that the body's center
//        {
//            newAngle = degreeAngle-90;
//            newAngle = Mathf.Clamp(newAngle, 0, maxAngleThreshold); // We clamp the angle to a maximum angle
//            rot= Quaternion.Euler(0,0,newAngle);
//        }
        damper.transform.rotation = rot;

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
            yPos = hit.point.y - yDistanceRayOrigin;
            float yDistance = body.transform.position.y - yPos;
//            Debug.Log("distance : " + yDistance);
            distanceFromLink = Vector3.Distance(link.transform.position, transform.position);
            Debug.Log("distance : " + distanceFromLink);
//            if (distanceFromLink > 0.2f)
//            {
//                Debug.Log("Stretch the " + damper + "!");
//                float yDeformer = damper.transform.localScale.y + distanceFromLink;
//                Vector3 newSize = new Vector3(damper.transform.localScale.x,yDeformer,damper.transform.localScale.z);
//                damper.transform.localScale = newSize;
//            }
            if (yDistance > maxDistance)
            {
                yPos = body.transform.position.y - maxDistance ;
            }
        }
        transform.position = new Vector2(body.transform.position.x + distanceWithBody, yPos);
        
    }
}
