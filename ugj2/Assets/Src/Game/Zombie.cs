using System.Collections.Generic;
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

	private bool m_aggred;

	public bool Aggred
	{
		get
		{
			
			return m_aggred;
		}
		set
		{
			eyes.gameObject.SetActive(value);
			m_aggred = value;
		}

	}

	[SerializeField]
	private Animator zombieAnim;

	[SerializeField]
	private SpriteRenderer eyes;

	[SerializeField]
	private float speed;

	[SerializeField]
	private int srchMaxDepth;


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
            if (Aggred)
			{
				var path = SearchPath(curCell.coordinates, GameCore.instance.Player.coordinates, srchMaxDepth);
				if (path != null)
				{
					var coords = path[path.Count - 1].realCoord;
					if (coords.y == curCell.coordinates.y)
					{
						if (coords.x > curCell.coordinates.x)
						{
							curDirection = MazeDirection.RIGHT;
						}
						else
							curDirection = MazeDirection.LEFT;
					}				
					else
					{

						if (coords.y > curCell.coordinates.y)
						{
							curDirection = MazeDirection.UP;
						}
						else
							curDirection = MazeDirection.DOWN;
					}
                    target = MazeCoords.CellToWorldCoords(coords);
					MoveInProgress = true;
				}
			}

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
        }

		transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
		if (transform.position.x == target.x && transform.position.y == target.y)
			MoveInProgress = false;
	}

	private List<PathNode> SearchPath(IntVector2 startPos, IntVector2 destPos, int maxDepth)
	{
		if (startPos.x == destPos.x && startPos.y == destPos.y)
		{
			return null;
		}

		int mapLngth = maxDepth * 2 + 1;
        PathNode[,] searchMap = new PathNode[mapLngth, mapLngth];
		PathNode initNode = new PathNode(startPos, 1, new IntVector2(maxDepth, maxDepth));
		searchMap[maxDepth, maxDepth] = initNode;
        List<PathNode> srchNodes = new List<PathNode>();
		srchNodes.Add(initNode);

		PathNode targetNode = null;
		PathNode curNode = srchNodes[0];

		MazeDirection dir;
		while (srchNodes.Count >= 1 && targetNode == null && curNode.searchIndx < maxDepth)
		{
			srchNodes.RemoveAt(0);
            var cell = CurMaze.GetCell(curNode.realCoord);
			for (int i = 0; i < MazeDirections.Count && targetNode == null; i++)
			{
				dir = (MazeDirection)i;
				var fCoords = curNode.fakeCoord + dir.ToIntVector2();

				if (searchMap[fCoords.x, fCoords.y] != null)
					continue;
				
				var pass = cell.GetEdge(dir) as MazePassage;

				if (GoodPass(pass))
				{
					PathNode newNode = new PathNode(curNode.realCoord + dir.ToIntVector2(), curNode.searchIndx+1, curNode.fakeCoord + dir.ToIntVector2());
					searchMap[newNode.fakeCoord.x, newNode.fakeCoord.y] = newNode;
					srchNodes.Add(newNode);
					if (newNode.realCoord.x == destPos.x && newNode.realCoord.y == destPos.y)
					{
						targetNode = newNode;
					}
                }
			}
			
			curNode = srchNodes[0];
		}
		if (targetNode == null)
			return null;
		srchNodes.Clear();
		int pathLength = targetNode.searchIndx;
	
		while (srchNodes.Count != pathLength-1)
		{
			srchNodes.Add(targetNode);
			var cell = CurMaze.GetCell(targetNode.realCoord);

			for (int i = 0; i < MazeDirections.Count; i++)
			{
				dir = (MazeDirection)i;
				var fCoords = targetNode.fakeCoord + dir.ToIntVector2();
				if (fCoords.x < mapLngth && fCoords.y < mapLngth && searchMap[fCoords.x, fCoords.y] != null && searchMap[fCoords.x, fCoords.y].searchIndx == targetNode.searchIndx - 1)
				{
					
					var pass = cell.GetEdge(dir) as MazePassage;
					if (GoodPass(pass))
					{
						targetNode = searchMap[fCoords.x, fCoords.y];
						break;
					}
					
                }				
			}
		}
		return srchNodes;

	}

	private bool GoodPass(MazePassage mp)
	{
		return mp != null && !mp.isEntry;
	}


	private void CheckDirection()
	{
		var edge = curCell.GetEdge(curDirection) as MazePassage;
		if (edge == null)
		{
			decidedDirection = false;
			return;
		}
		if (edge.otherCell == null)
		{
			decidedDirection = false;
            return;
        }
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		var dude = other.gameObject.GetComponent<Dude>();
		if (dude != null)
		{
			GameCore.instance.RestartGame();
		}
	}
}
