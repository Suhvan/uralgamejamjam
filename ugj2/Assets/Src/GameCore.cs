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
	private float m_shiftCdTime = 0;

	private void Start()
	{
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

		if (mazeInstance!=null && mazeInstance.Ready)
		{
			m_shiftCdTime -= Time.deltaTime;
			//if (m_shiftCdTime < 0)
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
	}	
	

	private void RestartGame()
	{
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		BeginGame();
	}
}
