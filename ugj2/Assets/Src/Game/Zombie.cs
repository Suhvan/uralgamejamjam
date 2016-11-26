using UnityEngine;

public class Zombie : MonoBehaviour {

	public MazeCell curCell;

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
			zombieAnim.SetInteger("Direction", (int)curDirection);
			zombieAnim.SetTrigger("DirectionChange");
			eyes.sprite = GameCore.instance.ZombieEyeSprites[(int)curDirection];
		}

	}

	public bool decidedDirection = false;

	public bool MoveInProgress;

	private Vector2 target;

	[SerializeField]
	private Animator zombieAnim;

	[SerializeField]
	private SpriteRenderer eyes;

	[SerializeField]
	private float speed;

	private Maze CurMaze
	{
		get
		{
			return GameCore.mazeInstance;
		}
	}

	void Start()
	{
		gameObject.transform.position = MazeCoords.CellToWorldCoords(MazeCoords.RandomCoords);
    }


	void Update() {
		curCell = CurMaze.GetCell(MazeCoords.WorldToCellCoords(transform.position));

		if (!MoveInProgress)
		{
			CheckDirection();
			if (!decidedDirection)
			{
				curDirection = curCell.RandomPassage;
				decidedDirection = true;
			}
			target = MazeCoords.CellToWorldCoords(curCell.coordinates + curDirection.ToIntVector2());
			MoveInProgress = true;
        }

		transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
		if (transform.position.x == target.x && transform.position.y == target.y)
			MoveInProgress = false;

		
	}

	private void CheckDirection()
	{
		var edge = curCell.GetEdge(curDirection) as MazePassage;
		decidedDirection = edge != null;
	}
}
