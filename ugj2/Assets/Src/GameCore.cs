using UnityEngine;
using System.Collections;

public class GameCore : MonoBehaviour {

	public Maze mazePrefab;

	public Maze.GenerationMode mode;

	public static Maze mazeInstance;

	private void Start()
	{
		BeginGame();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			RestartGame();
		}
		
		if (mazeInstance!=null && mazeInstance.Ready)
		{
			mazeInstance.ShiftMaze();
		}
	}

	private void BeginGame()
	{	
		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.Init();
		//StartCoroutine(mazeInstance.Generate(mode));
		mazeInstance.Generate(mode);
	}	
	

	private void RestartGame()
	{
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		BeginGame();
	}
}
