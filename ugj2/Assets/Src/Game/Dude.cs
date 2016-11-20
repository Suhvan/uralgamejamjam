
using UnityEngine;

class Dude : MonoBehaviour
{
	[SerializeField]
	private float maxSpeed = 10.0f;

	Rigidbody2D body;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
	}


	void FixedUpdate()
	{
		body.velocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed, Input.GetAxis("Vertical") * maxSpeed);
	}
}

