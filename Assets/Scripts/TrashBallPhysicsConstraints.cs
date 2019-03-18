using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBallPhysicsConstraints : MonoBehaviour {

    private Rigidbody2D rb;

    [Range(0.01f, 1f)] public float playerSlowFactor = 0.05f;
    [Range(0.01f, 1f)] public float ballSlowFactor = 1f;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
    }
	
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject player = collision.gameObject;
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            
            if(PlayerOffsetY(player) > transform.position.y)
            {
                //Debug.Log("Player is above the ball's center");
                rb.velocity = new Vector2(playerRb.velocity.x * ballSlowFactor, rb.velocity.y);
                playerRb.velocity = new Vector3(playerRb.velocity.x * playerSlowFactor, playerRb.velocity.y);
            }
            else
                rb.velocity = new Vector2(playerRb.velocity.x, rb.velocity.y);
        }
    }

    public float PlayerOffsetY(GameObject playerGO)
    {
        SpriteRenderer rend = playerGO.GetComponent<SpriteRenderer>();
        Bounds bnds = rend.bounds;
        float ySize = rend.bounds.size.y;
        float yOffset = ySize / 2;
        float yPos = playerGO.transform.position.y - yOffset;
        return yPos;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        rb.velocity = Vector2.zero;
    }
}
