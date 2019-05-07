using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BorderScript : MonoBehaviour
{
    public enum Direction
    {
        PUSHLEFT, PUSHRIGHT
    }

    public Direction direction;
    public float pushForce;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Trashball"))
        {
            Rigidbody2D trashRb = other.GetComponent<Rigidbody2D>();
            float trashMass = trashRb.mass;
            pushForce = trashMass / 2;
            switch (direction)
            {
                case Direction.PUSHLEFT:
                    trashRb.AddForce(Vector2.left * pushForce);
                    break;
                case Direction.PUSHRIGHT:
                    trashRb.AddForce(Vector2.right * pushForce);
                    break;
                default:
                    break;
            }
        }
    }
}
