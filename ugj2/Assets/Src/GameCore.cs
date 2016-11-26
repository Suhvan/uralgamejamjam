using UnityEngine;
using System.Collections;

public class GameCore : MonoBehaviour {

	public Maze mazePrefab;

	public Maze.GenerationMode mode;

	public static Maze mazeInstance;

	public int ShiftCd = 30;

	[SerializeField]
	GameObject BigLight;

	[SerializeField]
	Dude dude;

	[SerializeField]
	private float m_shiftCdTime = 0;

	[SerializeField]
	Zombie zPrefab;

	private void Start()
	{
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
			Instantiate(zPrefab);
		}

		if (mazeInstance!=null && mazeInstance.Ready)
		{
			//m_shiftCdTime -= Time.deltaTime;
			if (m_shiftCdTime < 0)
			if (Input.GetKeyDown(KeyCode.Space))
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
		//dude.gameObject.transform.position = MazeCoords.CellToWorldCoords(MazeCoords.RandomCoords);
	}	
	

	private void RestartGame()
	{
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		BeginGame();
	}
}
