using UnityEngine;
using System.Collections;

public class VictoryTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (GameCore.instance.PikedUpWater)
		{
			var dude = other.gameObject.GetComponent<Dude>();
			if (dude != null)
			{
				StartCoroutine( GameCore.instance.EndGame());
				Debug.Log("Victory");
			}
		}
	}
}
