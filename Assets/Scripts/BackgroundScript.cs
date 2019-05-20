using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    private float length;
    private float startPosX;
    private float startPosY;
    public GameObject cam;
    public float parallaxEffectX;
    private float parallaxEffectY;
    public float parallaxYFactor;
    
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = transform.parent.gameObject;
        parallaxEffectY = parallaxYFactor*parallaxEffectX / 100;
        Debug.Log("Parallax effects Y : " + parallaxEffectY);
    }

    void FixedUpdate()
    {
        float distX = (cam.transform.position.x * parallaxEffectX);
        float distY = (cam.transform.position.y * parallaxEffectY);

        transform.position = new Vector3(startPosX + distX, startPosY+distY, transform.position.z);
    }
}
