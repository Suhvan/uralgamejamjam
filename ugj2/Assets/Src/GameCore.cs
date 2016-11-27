using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCore : MonoBehaviour {

	public Maze mazePrefab;

	public Maze.GenerationMode mode;

	public static Maze mazeInstance;

	public static GameCore instance { private set; get; }

	public int ShiftCd = 30;

	[SerializeField]
	private int ZombieSpawnCd = 15;

	[SerializeField]
	GameObject BigLight;

	[SerializeField]
	Transform dudeStartPoint;

	[SerializeField]
	Water water;

	[SerializeField]
	Dude dude;

	public Dude Player { get { return dude; } }

	[SerializeField]
	private float m_shiftCdTime = 0;

	[SerializeField]
	private float m_zombieCdTime = 0;

	[SerializeField]
	private int ZombieLimit = 5;

	[SerializeField]
	Zombie zPrefab;

	public Sprite[] ZombieEyeSprites;

	private List<Zombie> zombies = new List<Zombie>();

	private void Start()
	{
		instance = this;
		Destroy( FindObjectOfType<Maze>().gameObject);
		BeginGame();
		m_shiftCdTime = ShiftCd;
		m_zombieCdTime = ZombieSpawnCd;
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			RestartGame();
		}

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			BigLight.SetActive(!BigLight.activeSelf);
		}

		if (mazeInstance!=null && mazeInstance.Ready)
		{
			m_shiftCdTime -= Time.deltaTime;
			if (m_shiftCdTime < 0)
			//if (Input.GetKeyDown(KeyCode.Space))
			{
				m_shiftCdTime = ShiftCd;
				mazeInstance.ShiftMaze();
			}

			if (zombies.Count < ZombieLimit)
			{
				m_zombieCdTime -= Time.deltaTime;
				if (m_zombieCdTime < 0)
				{
					m_zombieCdTime = ZombieSpawnCd;
                    zombies.Add(Instantiate(zPrefab));
				}
			}
        }

		if (water.coords.x == Player.coordinates.x && water.coords.y == Player.coordinates.y)
		{
			RestartGame();
        }
	}

	private void BeginGame()
	{   
		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.Init(mode);

		water.coords = new IntVector2(Random.Range(0, MazeCoords.MazeSize.x ), Random.Range(9, MazeCoords.MazeSize.y));
		water.transform.position = MazeCoords.CellToWorldCoords(water.coords);
		//StartCoroutine(mazeInstance.Generate());
		mazeInstance.Generate();
		dude.gameObject.transform.position = dudeStartPoint.position;
	}	
	

	public void RestartGame()
	{
		foreach (var z in zombies)
		{
			Destroy(z.gameObject);
		}
		zombies.Clear();

		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		BeginGame();
	}
}
