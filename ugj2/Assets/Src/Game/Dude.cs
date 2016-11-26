
using UnityEngine;

class Dude : MonoBehaviour
{
	[SerializeField]
	private float maxSpeed = 10.0f;

	Rigidbody2D body;

	[SerializeField]
	private MazeLighter LighterPrefab;

	private MazeLighter lighter;

	public IntVector2 curCell;

	void Awake()
	{
		body = GetComponent<Rigidbody2D>();
		lighter = GetComponentInChildren<MazeLighter>();
	}


	void FixedUpdate()
	{
		body.velocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed, Input.GetAxis("Vertical") * maxSpeed);
	}

	public void Update()
	{
		
		// Distance from camera to object.  We need this to get the proper calculation.
		float camDis = Camera.main.transform.position.y - lighter.transform.position.y;

		// Get the mouse position in world space. Using camDis for the Z axis.
		Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDis));

		float AngleRad = Mathf.Atan2(mouse.y - lighter.transform.position.y, mouse.x - lighter.transform.position.x);
		float angle = (180 / Mathf.PI) * AngleRad;

		lighter.transform.rotation = Quaternion.Euler(0,0, angle - 90);

		/*if (Input.GetKeyDown(KeyCode.Space))
		{
			lighter = Instantiate(LighterPrefab);
			lighter.transform.position = transform.position;
		}*/
	}
}

