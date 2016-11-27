using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameCore : MonoBehaviour {

	public Maze mazePrefab;


	[SerializeField]
	private Water waterPrefab;

	[SerializeField]
	private Demon DemonPrefab;

	public Maze.GenerationMode mode;

	public static Maze mazeInstance;

	public DialogsCore dialogSystem;

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

	[SerializeField]
	MazeLighter lPregab;

	[SerializeField]
	private int lightersLimit;

	[SerializeField]
	Image foreground;

	public Sprite[] ZombieEyeSprites;

	private List<Zombie> zombies = new List<Zombie>();

	public bool PikedUpWater { get	{ return water == null; } }

	public Texture2D cursorTexture;

	[SerializeField]
	private AudioClip deathSound;


	private void Start()
	{
		//Cursor.SetCursor(cursorTexture, Vector3.zero, CursorMode.Auto);
		GameOver = true;
		instance = this;
		Destroy( FindObjectOfType<Maze>().gameObject);
		StartCoroutine(BeginGame());
		m_shiftCdTime = ShiftCd;
		m_zombieCdTime = ZombieSpawnCd;
    }


	private void Update()
	{
		/*if (Input.GetKeyDown(KeyCode.R))
		{
			RestartGame();
		}

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			BigLight.SetActive(!BigLight.activeSelf);
		}*/

		if (Input.GetKeyDown(KeyCode.Space))
		{
			CleanDialog();
		}

		if (mazeInstance!=null && mazeInstance.Ready)
		{
			m_shiftCdTime -= Time.deltaTime;
			if (m_shiftCdTime < 0)
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

		if (water != null && water.coords.x == Player.coordinates.x && water.coords.y == Player.coordinates.y)
		{
			water.PickUp();
			StartCoroutine(DemonSpawn());
        }
	}

	IEnumerator DemonSpawn()
	{
		yield return new WaitForSeconds(3);
		CleanDialog();
		dialogSystem.OnWaterPickUp();	
		var deamon = Instantiate(DemonPrefab);
		deamon.transform.position = Player.transform.position;
	}

	[SerializeField]
	private float FadeTime = 0.5f;

	private IEnumerator BeginGame()
	{	
		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.Init(mode);
		
		water = Instantiate(waterPrefab);
		water.coords = new IntVector2(Random.Range(1, MazeCoords.MazeSize.x-1 ), 16);

		water.transform.position = MazeCoords.CellToWorldCoords(water.coords);

		GameOver = false;
		//StartCoroutine(mazeInstance.Generate());
		mazeInstance.Generate();
		dude.gameObject.transform.position = dudeStartPoint.position;

		dialogSystem.OnGameStart();

		for (int i = 0; i < lightersLimit; i++)
		{
			var l = Instantiate(lPregab);
			l.transform.parent = mazeInstance.gameObject.transform;
			l.transform.position = MazeCoords.CellToWorldCoords(MazeCoords.RandomCoords);
		}
		yield return null;

		foreground.CrossFadeAlpha(0f, FadeTime, false);
		yield return new WaitForSeconds(FadeTime);

	}

	private void CleanDialog()
	{
		foreach (var d in FindObjectsOfType<DialogLine>())
		{
			Destroy(d.gameObject);
		}
	}

	public bool GameOver { private set; get; }

	public IEnumerator EndGame()
	{
		if (GameOver)
			yield break;
		GameOver = true;
		CleanDialog();
		dialogSystem.OnGameEnd();
	}

	public IEnumerator RestartGame(bool byDeath)
	{
		if (GameOver)
			yield break;
		GameOver = true;
		if (byDeath)
		{	
            AudioSource.PlayClipAtPoint(deathSound, Player.transform.position);
		}
		foreground.CrossFadeAlpha(1f, FadeTime, false);
		yield return new WaitForSeconds(Mathf.Max(FadeTime, deathSound.length));

		foreach (var z in zombies)
		{
			Destroy(z.gameObject);
		}
		zombies.Clear();

		foreach (var d in FindObjectsOfType<DialogLine>())
		{
			Destroy(d.gameObject);
		}
		dialogSystem.DropIndexes();

		if (water != null)
			Destroy(water.gameObject);

		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);		
		StartCoroutine( BeginGame());
	}
}
