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
	GameObject BigLight;

	[SerializeField]
	Transform dudeStartPoint;

	[SerializeField]
	Dude dude;

	public Dude Player { get { return dude; } }

	[SerializeField]
	private float m_shiftCdTime = 0;

	[SerializeField]
	Zombie zPrefab;

	public Sprite[] ZombieEyeSprites;

	private List<Zombie> zombies = new List<Zombie>();

	private void Start()
	{
		instance = this;
		Destroy( FindObjectOfType<Maze>().gameObject);
		Application.targetFrameRate = 30;
		BeginGame();
		m_shiftCdTime = ShiftCd;
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

		if (Input.GetKeyDown(KeyCode.Z))
		{
			zombies.Add(Instantiate(zPrefab));
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
		}
	}

	private void BeginGame()
	{	
		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.Init(mode);
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
