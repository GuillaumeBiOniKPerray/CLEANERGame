using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashManager : MonoBehaviour {

    public int trashValue = 2;

    public List<Sprite> spriteList = new List<Sprite>();
    
	// Use this for initialization
	private void Start ()
	{
		if (spriteList.Count > 0)
		{
			Sprite newSprite = spriteList[GetRandomInt(spriteList.Count)];
			GetComponent<SpriteRenderer>().sprite = newSprite;
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            Destroy(GetComponent<Rigidbody2D>());
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    private int GetRandomInt(int max)
    {
	    return Random.Range(0, max);
    }
}
