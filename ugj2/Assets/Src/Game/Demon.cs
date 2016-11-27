using UnityEngine;
using System.Collections;

public class Demon : MonoBehaviour {

	[SerializeField]
	private int speed=1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position =  Vector3.MoveTowards(transform.position, GameCore.instance.Player.transform.position, speed * Time.deltaTime);
	}
}
