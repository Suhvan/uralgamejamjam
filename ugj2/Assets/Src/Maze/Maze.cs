using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	public IntVector2 size;

	public float cellSize = 0.16f;

	public MazeCell cellPrefab;

	public Pool mazePool;

	private MazeCell[,] cellPool;

	private List<MazeCell> activeCells = new List<MazeCell>();

	public MazeRoomSettings[] roomSettings;

	private List<MazeRoom> rooms = new List<MazeRoom>();

	[Range(0f, 1f)]
	public float doorProbability;

	public IntVector2 StartCoords
	{
		get
		{
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.y));
		}
	}

	private MazeRoom CreateRoom(int indexToExclude)
	{	
		var settingsIndex = Random.Range(0, roomSettings.Length);
		if (settingsIndex == indexToExclude)
		{
			settingsIndex = (settingsIndex + 1) % roomSettings.Length;
		}
		MazeRoom newRoom = new MazeRoom(rooms.Count, settingsIndex);
		rooms.Add(newRoom);
		return newRoom;
	}

	public void ShiftMaze()
	{
		Ready = false;
		activeCells = new List<MazeCell>();
		var litCells = RemoveDarkCells();
		foreach (var cell in litCells)
		{
			cell.ValidateEdges();
			if (!cell.IsFullyInitialized)
			{
				activeCells.Add(cell);
			}
		}
		//StartCoroutine(Generate());
		Generate();
    }

	private List<MazeCell> RemoveDarkCells()
	{
		List<MazeCell> litCells = new List<MazeCell>();
        for (int x = 0; x < size.x; x++)
		{
			for (int y=0; y < size.y; y++)
			{
				if (!cellPool[x, y].Lit)
				{
					//Destroy(cellPool[x, y].gameObject);
					cellPool[x, y].ClearEdges();
                    cellPool[x, y].gameObject.SetActive(false);
                }
				else
				{
					litCells.Add(cellPool[x, y]);
                }
			}
		}
		return litCells;
    }

	public bool Ready { get; private set; } 

	//public  IEnumerator Generate()
	public void Generate()
	{	
		while (activeCells.Count > 0)
		{
			//yield return null;
			DoGenerationStep();
        }
		Ready = true;
    }

	public void Init(GenerationMode mode)
	{
		Ready = false;
		this.mode = mode;

		cellPool = new MazeCell[size.x, size.y];

		mazePool.PreparePool( size);

		foreach (var rs in roomSettings)
		{
			rs.Init();
		}

		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				CreateCell(new IntVector2(x, y));
            }
		}
		var newCell = GetCellFromPool(StartCoords);
		newCell.Initialize(CreateRoom(-1));
		activeCells.Add(newCell);
	}

	public enum GenerationMode
	{
		FIRST,
		LAST, 
		MEDIUM,
		RANDOM
	}

	private GenerationMode mode;


	private void DoGenerationStep()
	{
		int currentIndex = 0;
        switch (mode)
        {
			case GenerationMode.FIRST:
				currentIndex = 0;
				break;
			case GenerationMode.LAST:
				currentIndex = activeCells.Count - 1;
				break;
			case GenerationMode.RANDOM:
				currentIndex = Random.Range(0, activeCells.Count);
				break;
			case GenerationMode.MEDIUM:
				currentIndex = activeCells.Count / 2;
				break;
		}
		
		MazeCell currentCell = activeCells[currentIndex];

		if (currentCell.IsFullyInitialized)
		{
			activeCells.RemoveAt(currentIndex);
			return;
		}
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
		MazeCell neighbor = GetCell(coordinates);

		if (!ContainsCoordinates(coordinates))
		{
			CreateWall(currentCell, GetCell(coordinates), direction);
			return;
		}

		if (neighbor == null)
		{
			neighbor = GetCellFromPool(coordinates);
			CreatePassage(currentCell, neighbor, direction);
			activeCells.Add(neighbor);
		}
		else if (currentCell.room.Id == neighbor.room.Id)
		{
			CreatePassage(currentCell, neighbor, direction, doorsPissible: false);
		}
		else if (neighbor != null && neighbor.Lit)
		{
			CreatePassage(currentCell, neighbor, direction);
		}
		else
		{
			CreateWall(currentCell, neighbor, direction);
		}	
		
	}


	private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction, bool doorsPissible = true )
	{	
        MazePassage passage = mazePool.GetPassage();
		passage.Initialize(cell, otherCell, direction);
		passage = mazePool.GetPassage();
		passage.Initialize(otherCell, cell, direction.GetOpposite());
		passage.DoorPassage = Random.value < doorProbability;
		if (!doorsPissible)
			return;

		if (Random.value < doorProbability )
		{
			otherCell.Initialize(CreateRoom(cell.room.SettingsIndex));
		}
		else
		{
			otherCell.Initialize(cell.room);
		}
	}

	private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
	{
		MazeWall wall = mazePool.GetWall();
        wall.Initialize(cell, otherCell, direction);
		if (otherCell != null)
		{
			wall = mazePool.GetWall();
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}
	}

	public MazeCell GetCell(IntVector2 coordinates)
	{
		if (!ContainsCoordinates(coordinates) || cellPool[coordinates.x, coordinates.y].gameObject.activeSelf == false)
			return null;		
		return cellPool[coordinates.x, coordinates.y];
	}

	public bool ContainsCoordinates(IntVector2 coordinate)
	{
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.y >= 0 && coordinate.y < size.y;
	}

	private MazeCell GetCellFromPool(IntVector2 coordinates)
	{
		if (!ContainsCoordinates(coordinates))
			return null;
		cellPool[coordinates.x, coordinates.y].gameObject.SetActive(true);
        return cellPool[coordinates.x, coordinates.y];
		
	}

	private MazeCell CreateCell(IntVector2 coordinates)
	{
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
		cellPool[coordinates.x, coordinates.y] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.y;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector2((coordinates.x - size.x / 2) * cellSize, (coordinates.y - size.y / 2) * cellSize);
		newCell.gameObject.SetActive(false);
		return newCell;
	}
}
