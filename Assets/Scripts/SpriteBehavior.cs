using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBehavior : MonoBehaviour
{
    public Rigidbody2D playeRb;
//    public float speed;
    public float maxAngle;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        float normalizedDirection = playeRb.velocity.normalized.x;
        float rbMagnitude = playeRb.velocity.magnitude;
        float clampedMag = Mathf.Clamp01(rbMagnitude);
        Debug.Log("velocity = " + clampedMag);
        float rotToApply = clampedMag * maxAngle;
        transform.eulerAngles = new Vector3(0, 0, -rotToApply*normalizedDirection);
    }
}
