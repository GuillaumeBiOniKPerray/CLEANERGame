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

        Vector3 vectorToTarget = link.transform.position - damper.transform.position;
        float distanceToLink = Vector3.Distance(link.transform.position, damper.transform.position);
        float xScale = damper.transform.localScale.y + distanceToLink;
        Vector3 newScale = new Vector3(xScale,damper.transform.localScale.y,damper.transform.localScale.z); // We adjust the size of the damper thanks to the distance from the body ("link" here).
        damper.transform.localScale = newScale;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        damper.transform.rotation = q;
        
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
            if (yDistance > maxDistance)
            {
                yPos = body.transform.position.y - maxDistance ;
            }
        }
        else
        {
            yPos = body.transform.position.y - maxDistance ;
        }
        transform.position = new Vector2(body.transform.position.x + distanceWithBody, yPos);
        

        
    }
}
