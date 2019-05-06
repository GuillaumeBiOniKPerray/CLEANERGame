using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelBehavior : MonoBehaviour
{
    
    public Rigidbody2D playeRb;
    public float normalizedVelocity;
    public float rotationSpeed;
    public float maxVelocity;
    public float rotToApply;
    
    // Update is called once per frame
    private void LateUpdate()
    {
        float normalizedDirection = playeRb.velocity.normalized.x;
        float rbMagnitude = playeRb.velocity.magnitude;
//        float rbMagnitude = playeRb.velocity.x;
        Debug.Log("rbMagnitude = " + rbMagnitude);
        normalizedVelocity = rbMagnitude / maxVelocity;
        rotToApply = normalizedVelocity * rotationSpeed *Time.deltaTime;
        transform.Rotate(0f,0f,-rotToApply * normalizedDirection);
    }
}
