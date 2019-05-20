using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrashManager : MonoBehaviour {

    public int trashValue = 2;

    public List<Sprite> spriteList = new List<Sprite>();

    public enum TrashType
    {
	    GLASS, METAL, GENERIC
    }

    public TrashType trashType;
    
    //Sounds
    private AudioSource audioSource;
    public AudioClip[] metalTrash;
    public AudioClip[] glassTrash;
    public AudioClip[] genericTrash;
    
	// Use this for initialization
	private void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		if (spriteList.Count > 0)
		{
			Sprite newSprite = spriteList[GetRandomInt(spriteList.Count)];
			GetComponent<SpriteRenderer>().sprite = newSprite;
			SetTrashType(newSprite);
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

    public void PlayTrashClip()
    {
	    if (trashType == TrashType.GLASS)
	    {
		    int indexToSelect = Random.Range(0, glassTrash.Length - 1);
		    audioSource.clip = glassTrash[indexToSelect];
	    }
	    if (trashType == TrashType.METAL)
	    {
		    int indexToSelect = Random.Range(0, metalTrash.Length - 1);
		    audioSource.clip = metalTrash[Random.Range(0, metalTrash.Length-1)];
	    }
	    if (trashType == TrashType.GENERIC)
	    {
		    int indexToSelect = Random.Range(0, genericTrash.Length - 1);
		    audioSource.clip = genericTrash[Random.Range(0, genericTrash.Length-1)];
	    }

	    audioSource.pitch = RandomPitch();
	    audioSource.Play();
	    Destroy(GetComponent<BoxCollider2D>());
	    Destroy(GetComponent<SpriteRenderer>());
	    StartCoroutine(DestroyAfterSound(audioSource.clip.length));
    }

    private void SetTrashType(Sprite currentSprite)
    {
	    if (CompareSprite(currentSprite,0) || CompareSprite(currentSprite, 3))
	    {
		    trashType = TrashType.METAL;
	    }
	    
	    else if (CompareSprite(currentSprite,1) || CompareSprite(currentSprite, 2))
	    {
		    trashType = TrashType.GLASS;
	    }
	    else if (CompareSprite(currentSprite,4))
	    {
		    trashType = TrashType.GENERIC;
	    }
    }

    private float RandomPitch()
    {
	    return Random.Range(0.2f, 0.7f);
    }
    

    private bool CompareSprite(Sprite currentSprite, int index)
    {
	    return currentSprite == spriteList[index];
    }

    private IEnumerator DestroyAfterSound(float clipDuration)
    {
	    yield return new WaitForSeconds(clipDuration);
	    Destroy(this.gameObject);
    }
}
