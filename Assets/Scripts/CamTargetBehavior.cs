using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTargetBehavior : MonoBehaviour
{

    public GameObject player;
    private Rigidbody2D playerRB;
    private Vector3 playerPos;
    public float camTargetSpeed;
    public float yOffset;
    
    private void Start()
    {
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        playerPos = player.transform.position;
        Vector3 playerVelocity = playerRB.velocity;
        float xVel = playerVelocity.x;
        float yVel = playerVelocity.y;
        yVel = Mathf.Clamp(yVel,0,1);
//        Debug.Log("velocity X : " + xVel);
        float newPosX = player.transform.position.x + xVel;
        float newPosY = player.transform.position.y+ yOffset + yVel;
        Vector3 newPos = new Vector3(newPosX, newPosY, transform.position.z);
        float dist = Vector3.Distance(transform.position, newPos);
        float mag = Vector3.Magnitude(newPos - transform.position);
        transform.position = Vector3.Lerp(transform.position, newPos, dist*camTargetSpeed);
    }
}
