using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

	public IntVector2 coords;
	public AudioClip pickUpSound;
		 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PickUp()
	{
		AudioSource.PlayClipAtPoint(pickUpSound, transform.position);
		Destroy(gameObject);
	}
}
