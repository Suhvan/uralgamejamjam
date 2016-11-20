
using UnityEngine;

class Dude : MonoBehaviour
{
	[SerializeField]
	private float maxSpeed = 10.0f;

	Rigidbody2D body;

	[SerializeField]
	private MazeLighter LighterPrefab;

	private MazeLighter lighter;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
		
    }


	void FixedUpdate()
	{
		body.velocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed, Input.GetAxis("Vertical") * maxSpeed);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			lighter = Instantiate(LighterPrefab);
			lighter.transform.position = transform.position;
		}
	}
}

