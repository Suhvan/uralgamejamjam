using UnityEngine;
using System.Collections;

public class DestroyInSeconds : MonoBehaviour {

	[SerializeField]
	private float destroyTime;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(destroyTime);
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
