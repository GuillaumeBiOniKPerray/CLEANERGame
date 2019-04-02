using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackScript : MonoBehaviour
{

    public GameObject body;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.DrawLine(transform.position, body.transform.position, Color.red);
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
        Ray ray = new Ray();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.2f);
        if (hit.collider.gameObject)
        {
            Debug.Log(hit.collider.gameObject);
        }
    }

    private float PlayerDistance()
    {
        float dist = Vector3.Distance(body.transform.position, transform.position);
        return dist;
    }
}
