using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	public IntVector2 size;

	public float cellSize = 0.16f;

	public MazeCell cellPrefab;
	public MazePassage passagePrefab;
	public MazeWall wallPrefab;

	private MazeCell[,] cells;

	private List<MazeCell> activeCells = new List<MazeCell>();

	public IntVector2 StartCoords
	{
		get
		{
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.y));
		}
	}

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//public  IEnumerator Generate()
	public void Generate()
	{	
		cells = new MazeCell[size.x, size.y];
		Init();
        IntVector2 coordinates = StartCoords;
		while (activeCells.Count > 0)
		{
			//yield return null;
			DoGenerationStep();
        }
	}

	private void Init()
	{
		activeCells.Add(CreateCell(StartCoords));
	}

	private void DoGenerationStep()
	{	
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];

		if (currentCell.IsFullyInitialized)
		{
			activeCells.RemoveAt(currentIndex);
			return;
		}
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
		if (ContainsCoordinates(coordinates) && GetCell(coordinates) == null)
		{
			MazeCell neighbor = GetCell(coordinates);
			if (neighbor == null)
			{
				neighbor = CreateCell(coordinates);
				CreatePassage(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}
			else
			{
				CreateWall(currentCell, neighbor, direction);				
			}
		}
		else
		{
			CreateWall(currentCell, null, direction);
		}
	}


	private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
	{
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
	{
		MazeWall wall = Instantiate(wallPrefab) as MazeWall;
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null)
		{
			wall = Instantiate(wallPrefab) as MazeWall;
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}
	}

	public MazeCell GetCell(IntVector2 coordinates)
	{
		return cells[coordinates.x, coordinates.y];
	}

	public bool ContainsCoordinates(IntVector2 coordinate)
	{
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.y >= 0 && coordinate.y < size.y;
	}

	private MazeCell CreateCell(IntVector2 coordinates)
	{
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
		cells[coordinates.x, coordinates.y] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.y;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector2((coordinates.x - size.x / 2) * cellSize, (coordinates.y - size.y / 2) * cellSize);
		return newCell;
	}
}
