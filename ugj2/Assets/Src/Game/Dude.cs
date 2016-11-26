
using UnityEngine;

public class Dude : MonoBehaviour
{
	[SerializeField]
	private float maxSpeed = 10.0f;

	Rigidbody2D body;

	[SerializeField]
	private MazeLighter LighterPrefab;

	[SerializeField]
	public Sprite[] idleSprites;

	[SerializeField]
	private SpriteRenderer manSprite;

	private MazeLighter lighter;

	public IntVector2 coordinates;

	private MazeDirection _lightDirection;

	private MazeDirection _curDirection;
	public MazeDirection curDirection
	{
		get
		{
			return _curDirection;
		}
		set
		{
			_curDirection = value;
			manAnim.SetInteger("Direction", (int)curDirection);
			manAnim.SetTrigger("DirectionChange");
		}

	}

	[SerializeField]
	private Animator manAnim;

	void Awake()
	{
		body = GetComponent<Rigidbody2D>();
		lighter = GetComponentInChildren<MazeLighter>();
	}


	void FixedUpdate()
	{
		body.velocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed, Input.GetAxis("Vertical") * maxSpeed);

		if (body.velocity.x == 0 && body.velocity.y == 0)
		{
			manAnim.enabled = false;
			manSprite.sprite = idleSprites[(int)_lightDirection];
			return;
		}
		manAnim.enabled = true;

		MazeDirection newDir = MazeDirection.DOWN;
		if (Mathf.Abs(body.velocity.x) > Mathf.Abs(body.velocity.y))
		{
			if (body.velocity.x > 0)
				newDir = MazeDirection.RIGHT;
			else
				newDir = MazeDirection.LEFT;
		}
		else
		{
			if (body.velocity.y > 0)
				newDir = MazeDirection.UP;
			else
				newDir = MazeDirection.DOWN;
		}
		

		if (curDirection != newDir)
		{
			curDirection = newDir;
		}
            

	}

	public void Update()
	{
		
		// Distance from camera to object.  We need this to get the proper calculation.
		float camDis = Camera.main.transform.position.y - lighter.transform.position.y;

		// Get the mouse position in world space. Using camDis for the Z axis.
		Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDis));

		float AngleRad = Mathf.Atan2(mouse.y - lighter.transform.position.y, mouse.x - lighter.transform.position.x);
		float angle = (180 / Mathf.PI) * AngleRad;

		if (angle >= 45 && angle < 135)
		{
			_lightDirection = MazeDirection.UP;
		}
		else if (angle > -135 && angle <= -45)
		{
			_lightDirection = MazeDirection.DOWN;
		}
		else if (angle < -135 || angle >= 135)
		{
			_lightDirection = MazeDirection.LEFT;
		}
		else
		{
			_lightDirection = MazeDirection.RIGHT;
		}

		lighter.transform.rotation = Quaternion.Euler(0,0, angle - 90);

		coordinates = MazeCoords.WorldToCellCoords(transform.position);
		if (Input.GetKeyDown(KeyCode.P))
		{
			manAnim.enabled = !manAnim.enabled;
		}
		
	}
}

