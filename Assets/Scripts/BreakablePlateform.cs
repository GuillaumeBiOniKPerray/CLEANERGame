using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlateform : MonoBehaviour {

    //Walking : When something touches the plateform it "fades" and destroys, if specified the plateform reappears after a short amount of time
    //Velocity : The plateform can be destroyed if an force is applied to it (plateform doesn't respawn)

    //Type of plateform (Only chose one!)
    public bool walkingBased; 
    public bool velocityBased;

    //Velocity Based Plateform variables :
    [Tooltip("use this variable if 'velocity based' is toggled")]
    public float breakingForceNeeded;

    //Walking Based variables :
    [Tooltip("use this variable if 'walking based' is toggled")]
    public float fadeSpeed;
    [Tooltip("use this variable if you want the plateform to respawn")]
    public bool canRespawn;
    [Tooltip("use this variable if 'canrespawn' is toggled, to set the time cooldown timer")]
    public float respawnTime;

    private Color initialColor; // We save the initial color of the plateform in order to make it reappear (if specified)

    private bool fade;

    private void Start ()
    {
        // Checking if everything is ok about the variables
        if (!velocityBased && !walkingBased)
            Debug.LogWarning("You're supposed to choose between walking or velocity based behavior on the breakable plateform : " + gameObject.name);
        if (velocityBased && walkingBased)
            Debug.LogWarning("You're supposed to choose between walking or velocity based behavior on the breakable plateform : " + gameObject.name + " only one option will be accepted");
        initialColor = GetComponent<SpriteRenderer>().color;
    }

    private void Update ()
    {
        if(fade)
            StartFadingPlateform();
    }

    private void OnTriggerEnter2D(Collider2D collision) //The behavior of the Breakable Platform depends on whether it is velocity based or walking based.
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
            {
                if(walkingBased) // The walking based platform can be triggered by the player
                {
                    fade = true;
                }

                break;
            }
            case "Trashball":
            {
                if (walkingBased) // The walking based platform can also be triggered by the trashball
                {
                    fade = true;
                }

                if(velocityBased) //The velocity based platform can only be triggered by the trashball
                {
                    // We need to compare the velocity of the incoming trashball to check if it is superior to the threshold
                    GameObject ball = collision.gameObject;
                    Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D>();
                    Vector2 velocity = ballRB.velocity;
                    float xVel = velocity.x;
                    float yVel = velocity.y;
                    if (yVel < 0) yVel = yVel * -1;
                    if (xVel < 0) xVel = xVel * -1;
                    if (yVel >= breakingForceNeeded) Destroy(gameObject); 
                    if (xVel >= breakingForceNeeded) Destroy(gameObject);
                }

                break;
            }
            default:
                break;
        }
    }

    private void StartFadingPlateform()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        Color currentColor = sRenderer.color;
        Color fadedColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
        sRenderer.color = Color.Lerp(currentColor, fadedColor, Time.deltaTime*fadeSpeed);
        if (!(sRenderer.color.a <= 0.2f)) return;
        if (canRespawn)
        {
            StartCoroutine(WaitUntilRespawn());
            ToggleComponents(false);
        }
        else
            Destroy(this.gameObject);
        fade = false;
    }

    private void ToggleComponents(bool onOff)
    {
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D coll in colliders)
        {
            coll.enabled = onOff;
        }
        GetComponent<SpriteRenderer>().enabled = onOff;
    }

    private IEnumerator WaitUntilRespawn()
    {
        yield return new WaitForSeconds(respawnTime);
        GetComponent<SpriteRenderer>().color = initialColor;
        ToggleComponents(true);
    }
}
